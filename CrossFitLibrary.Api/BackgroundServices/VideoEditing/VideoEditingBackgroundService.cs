using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using CrossFitLibrary.Api.Settings;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossFitLibrary.Api.BackgroundServices.VideoEditing;

public class VideoEditingBackgroundService : BackgroundService
{
    private readonly ChannelReader<EditVideoChannelMessage> _channelReader;
    private readonly IFileManager _fileManagerLocal;
    private readonly ILogger<VideoEditingBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;


    public VideoEditingBackgroundService(
        Channel<EditVideoChannelMessage> channel,
        ILogger<VideoEditingBackgroundService> logger,
        IServiceProvider serviceProvider,
        IFileManager fileManagerLocal)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _fileManagerLocal = fileManagerLocal;
        _channelReader = channel.Reader;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // We are waiting for the channel to be written to.
        while (await _channelReader.WaitToReadAsync(stoppingToken))
        {
            // The message written onto the channel is of type  EditVideoMessage.
            // We write on the channel from the SubmissionController
            
            
            var video_info = await _channelReader.ReadAsync(stoppingToken);
            var input_video_path = _fileManagerLocal.TemporarySavePath(video_info.VideoFileName);
            var output_video_name = CrossFitLibraryConstants.Files.GenerateConvertedVideoFileName();
            var output_thumbnail_name = CrossFitLibraryConstants.Files.GenerateThumbnailFileName(output_video_name);
            var output_video_path = _fileManagerLocal.TemporarySavePath(output_video_name);
            var output_thumbnail_path = _fileManagerLocal.TemporarySavePath(output_thumbnail_name);
            try
            {
                // The objective is to convert the video in the background to make it smaller
                // we use a console app called ffmpeg to do de conversion

                // We need to build an object that contains all the information to convert the video
                // We will pass it as parameter to the process.start
                var startInfo = new ProcessStartInfo
                {
                    // FileName is the path of the ffmpeg.exe console tool
                    FileName = _fileManagerLocal.GetFFmpegPath(),

                    // -vn / -an / -sn / -dn options can be used to skip inclusion of video, audio, subtitle and data streams 
                    // We use the -vf argument to specify a simple graph video filter (audio filter would be -af)
                    // todo: Find a solution for the thumbnail format (the ratio is not good)
                    // todo: Handle vertical videos, the scaling doesn't seem to work properly
                    Arguments =
                        $"-y -i {input_video_path} -an -vf scale=540x380 {output_video_path} -ss 00:00:00 -vframes 1 -vf scale=540x380 {output_thumbnail_path}",

                    // We use the UseShellExecute property to launch the process without a console window
                    // We use the CreateNoWindow option to hide the console window
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // We launch the ffmpeg process using the system.diagnotics namespace
                using (var process = Process.Start(startInfo))
                {
                    process.Start();
                    // We ask the System.Diagnostics.Process component to wait for the process to finish and exit by itself
                    process.WaitForExit();
                }

                if (!_fileManagerLocal.FileExists(output_video_name))
                {
                    throw new Exception("FFMPEG.exe failed to convert the video.");
                }

                if (!_fileManagerLocal.FileExists(output_thumbnail_name))
                {
                    throw new Exception("FFMPEG.exe failed to generate a video thumbnail.");
                }


                using (var scope = _serviceProvider.CreateScope())
                {
                    // We open a new scope to get the AppDbContext because of the background nature of the service
                    // It won't work if we inject the AppDbContext in the constructor of this background service class
                    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var submission = ctx.Submissions.FirstOrDefault(s =>
                        s.Id.Equals(video_info.SubmissionId));

                    submission.Video = new Video
                    {
                        VideoUrl = _fileManagerLocal.GetFileUrl(output_video_name, FileType.Video),
                        ThumbnailUrl = _fileManagerLocal.GetFileUrl(output_thumbnail_name, FileType.Image) 
                    };


                    // Todo: Upload video to cloud storage and update submission.Video using the url as value

                    // This is a boolean flag to filter the submissions that have been already processed before sending them to the client
                    submission.VideoProcessed = true;
                    await ctx.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Error while processing video {video_info.VideoFileName}");

                _fileManagerLocal.DeleteFile(video_info.VideoFileName);
                _fileManagerLocal.DeleteFile(output_thumbnail_name);
                throw;
            }
            finally
            {
                _fileManagerLocal.DeleteFile(video_info.VideoFileName);
            }
        }
    }
}