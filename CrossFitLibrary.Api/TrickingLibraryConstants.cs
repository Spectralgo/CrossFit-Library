using System;
using IdentityServer4;

namespace CrossFitLibrary.Api;

public struct TrickingLibraryConstants
{
    public struct Policies
    {
        public const string User = IdentityServerConstants.LocalApi.PolicyName;
        public const string Mod = nameof(Mod);
    }
        
    public struct IdentityResources
    {
        public const string RoleScope = "role";
    }

    public struct Claims
    {
        public const string Role = "role";
    }
        
    public struct Roles
    {
        public const string Mod = nameof(Mod);
    }
        
    public struct Files
    {
            
        public const string TempPrefix = "temp_";
        public const string ConvertedPrefix = "c";
        public const string ThumbnailPrefix = "t";
        public const string ImagePrefix = "p";
        
        public static string GenerateConvertedVideoFileName()
        {
            return $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
        }
        
        public static string GenerateThumbnailFileName(string videoFileName)
        {
            videoFileName = videoFileName.Replace(".mp4", ".jpg");
            // slice out the first letter of the video file name to get ride of the converted prefix
            return $"{ThumbnailPrefix}{videoFileName.Substring(1)}";
        }
        
        public static string GenerateImageFileName()
        {
            return $"{ImagePrefix}{DateTime.Now.Ticks}.jpg";
        }
    }
        
}