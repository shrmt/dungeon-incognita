using System;
using System.Collections.Generic;

public static class DungeonLogic
{
    public static bool IsDungeonValid(this Dungeon dungeon)
    {
        if (dungeon == null) throw new ArgumentNullException("dungeon");
        if (dungeon.Entrance == null) throw new ArgumentNullException("dungeon.Entrance");
        return dungeon.Entrance.IsLocValid();
    }

    public static bool IsDungeonPassable(this Dungeon dungeon, Player player)
    {
        if (dungeon == null) throw new ArgumentNullException("dungeon");
        if (player == null) throw new ArgumentNullException("player");
        throw new NotImplementedException();
    }

    public static Dictionary<int, IList<ILoc>> GetLeveledLocs(this Dungeon dungeon)
    {
        var levelLocs = new Dictionary<int, IList<ILoc>>();
        var passedLocs = new List<ILoc>();

        void WriteLocLevelRecursively(ILoc loc, int level)
        {
            if (passedLocs.Contains(loc)) return;

            passedLocs.Add(loc);

            if (levelLocs.ContainsKey(level))
            {
                levelLocs[level].Add(loc);
            }
            else
            {
                levelLocs.Add(level, new List<ILoc> { loc });
            }

            if (loc is ExitLoc) return;

            foreach (var l in loc.NextLocs)
            {
                WriteLocLevelRecursively(l, level + 1);
            }
        }

        WriteLocLevelRecursively(dungeon.Entrance, 0);

        return levelLocs;
    }

    public static void ForEachLoc(this Dungeon dungeon, Action<ILoc> action)
    {
        var locs = new List<ILoc>();

        void ForLocRecursively(ILoc loc)
        {
            if (locs.Contains(loc)) return;

            action(loc);
            locs.Add(loc);

            if (loc is ExitLoc) return;

            foreach (var l in loc.NextLocs)
            {
                ForLocRecursively(l);
            }
        }

        ForLocRecursively(dungeon.Entrance);
    }
}

public interface ILocEntryResult { }

public class OkResult : ILocEntryResult
{
    public int DeltaHealth;
    public int DeltaMana;
    public string Text;

    public OkResult(int deltaHealth = 0, int deltaMana = 0, string text = null)
    {
        DeltaHealth = deltaHealth;
        DeltaMana = deltaMana;
        Text = text;
    }
}

public class ChoiceRequiredResult : ILocEntryResult
{
    public readonly string Text;
    public readonly ChoiceOption[] Options;

    public ChoiceRequiredResult(string text, params ChoiceOption[] options)
    {
        Text = text;
        Options = options;
    }
}

public class ChoiceOption
{
    public readonly string Text;
    public readonly ILocEntryResult Result;
    public readonly bool IsAvailable;

    public ChoiceOption(string text, ILocEntryResult result, bool isAvailable = true)
    {
        Text = text;
        Result = result;
        IsAvailable = isAvailable;
    }
}
