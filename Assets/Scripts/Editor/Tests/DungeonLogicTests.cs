using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class DungeonLogicTests
{
    [Test]
    public void IsDungeonValid_True()
    {
        var dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                new MonsterLoc(new ExitLoc()),
                new MonsterLoc(new ExitLoc())
            ),
        };

        Assert.IsTrue(DungeonLogic.IsDungeonValid(dungeon));
    }

    [Test]
    public void IsDungeonValid_False()
    {
        var dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                new EntranceLoc()
            ),
        };

        Assert.IsFalse(DungeonLogic.IsDungeonValid(dungeon));
    }

    [Test]
    public void IsDungeonPassable()
    {
        var dungeon = CreateTestDungeon();

        var player = new Player()
        {
            Health = 5,
            Mana = 0,
        };

        Assert.AreEqual(1, GetWinsCount(dungeon, player, Array.Empty<int>(), 0, 0));
    }

    private (int, int) GetWinsCount(Dungeon dungeon, Player playerTemplate, int[] nextLocChoices, int wins, int total)
    {
        var controller = new GameController(new MockGameView(), dungeon);

        var choices = new Queue<int>(nextLocChoices);
        var player = new Player()
        {
            Health = playerTemplate.Health,
            Mana = playerTemplate.Mana,
        };

        controller.StartGame(player);

        while (!controller.IsWin || !controller.IsDead(player))
        {
            if (controller.CurrentLoc is ExitLoc) break;

            ILoc loc;

            if (choices.Count > 0)
            {
                var choice = choices.Dequeue();
                loc = controller.CurrentLoc.NextLocs[choice];
            }
            else
            {
                for (int i = 1; i < controller.CurrentLoc.NextLocs.Length; i++)
                {
                    var tempLocChoices = nextLocChoices.Concat(new[] { i }).ToArray();
                    var ret = GetWinsCount(dungeon, playerTemplate, tempLocChoices, 0, 0);
                    wins += ret.Item1;
                    total += ret.Item2;
                }

                loc = controller.CurrentLoc.NextLocs[0];
                nextLocChoices = nextLocChoices.Concat(new[] { 0 }).ToArray();
            }

            var result = loc.EnterLoc(player);
            controller.HandleLocEntryResult(loc, result, player);
        }

        if (controller.IsWin)
        {
            UnityEngine.Debug.Log(string.Join(" ", nextLocChoices));
        }

        return (wins + (controller.IsWin ? 1 : 0), total + 1);
    }

    private Dungeon CreateTestDungeon()
    {
        var exit = new ExitLoc();
        var exit2 = new ExitLoc();

        var l0_1 = new CampLoc(exit);
        var l0_2 = new CampLoc(exit2);
        var exit3 = new ExitLoc();

        var l1_1 = new CampLoc(l0_1, l0_2);
        var l1_2 = new MonsterLoc(l0_2);
        var l1_3 = new MonsterLoc(exit3);

        var l2_1 = new CampLoc(l1_1, l1_2);
        var l2_2 = new MonsterLoc(l1_3);

        var l4_1 = new CampLoc(l2_1);
        var l4_2 = new MonsterLoc(l2_1, l2_2);

        var l5_1 = new MonsterLoc(l4_1);
        var l5_2 = new MonsterLoc(l4_1);
        var l5_3 = new CampLoc(l4_2);

        var l6_1 = new MonsterLoc(l5_1, l5_2, l5_3);
        var l6_2 = new CampLoc(l5_3);

        return new Dungeon()
        {
            Entrance = new EntranceLoc(
                l6_1, l6_2
            ),
        };
    }
}
