using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Models.Abstractions;
using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Data;

public class VersionMigrationContext
{
    private readonly AppDbContext _ctx;

    public VersionMigrationContext(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void Migrate(ModerationItem modItem)
    {
        var source = GetSource(modItem.Type);

        var current = source.FirstOrDefault(x => x.Id == modItem.Current);
        var target = source.FirstOrDefault(x => x.Id == modItem.Target);

        if (target is null)
        {
            throw new InvalidOperationException("Target not found");
        }

        if (current != null)
        {

            if (target.Version - current.Version <= 0)
            {
                throw new InvalidVersionException(
                    $"Current version is {current.Version}, Target version is {target.Version} for {modItem.Type}.");
            }
            
            current.Active = false;

            // Grabbing all open moderation items and bump the target version accordingly
            // We just added a new version so we want to point next updates to this new id number
            List<ModerationItem> outdatedModerationItems = _ctx.ModerationItems
                .Where(x => !x.Deleted  && x.Type == modItem.Type && x.Id != modItem.Id)
                .ToList();

            foreach (var item in outdatedModerationItems)
            {
                item.Current = target.Id;
            }
        }

        target.Active = true;

    }


    private IQueryable<VersionedModel> GetSource(string type)
    {
        if (type == ModerationItemTypes.Trick)
        {
            return _ctx.Tricks;
        }

        throw new ArgumentException(nameof(type));
    }


    public class InvalidVersionException : Exception
    {
        public InvalidVersionException(string message) : base(message)
        {
        }
    }
}