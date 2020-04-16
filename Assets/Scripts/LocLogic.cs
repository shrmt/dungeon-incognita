using System;
using System.Linq;

public static class LocLogic
{
    public static bool IsLocValid(this ILoc loc)
    {
        if (loc is ExitLoc)
            return true;
        if (loc == null)
            return false;
        if (loc.NextLocs == null || loc.NextLocs.Length == 0)
            return false;
        return loc.NextLocs.All(l => l.IsLocValid());
    }

    public static ILocEntryResult EnterLoc(this ILoc loc, Player player)
    {
        if (loc is EntranceLoc entrance)
        {
            return new OkResult();
        }
        else if (loc is ExitLoc exit)
        {
            return new ChoiceRequiredResult(
                "After you entered the room, you see a giant dragon sitting on a no less giant chest. What will you do?",
                new ChoiceOption("Rush at him (-5 health)", new OkResult(-5, 0)),
                new ChoiceOption("Cast a fireball (-3 mana)", new OkResult(0, -3), player.Mana >= 3)
            );
        }
        else if (loc is MonsterLoc monster)
        {
            return new OkResult(-2, 0, "Something massive jumps on you and you fall down. Your body is damaged, but your knife strikes immediately. Cave wolf is dead.");
        }
        else if (loc is CampLoc camp)
        {
            return new ChoiceRequiredResult(
                "You take a slow look at the place you just appeared in. Finally you drop your weapon down and decide to take a break. How do you decide to relax?",
                new ChoiceOption("Sleep (+1 health)", new OkResult(1, 0)),
                new ChoiceOption("Meditate (+1 mana)", new OkResult(0, 1)));
        }
        else throw new NotImplementedException(loc.GetType().ToString());
    }
}
