using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuUi : MonoBehaviour
{
    public void StartLevel1()
    {
        var exit = new ExitLoc();

        var l1_1 = new MonsterLoc(exit);
        var l1_2 = new MonsterLoc(exit);

        var l2_1 = new CampLoc(l1_1, l1_2);
        var l2_3 = new MonsterLoc(exit);

        var l3_1 = new CampLoc(l2_1);
        var l3_2 = new MonsterLoc(l2_3, l2_3);
        var l3_3 = new MonsterLoc(l2_3);

        var l4_1 = new CampLoc(l3_1, l3_2);
        var l4_2 = new MonsterLoc(l3_2, l3_3);

        var l5_1 = new MonsterLoc(l4_1);
        var l5_2 = new MonsterLoc(l4_1, l4_2);
        var l5_3 = new CampLoc(l4_2);

        GameStarter.Dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                l5_1,
                l5_2,
                l5_3
            ),
        };

        GoToGame();
    }

    public void StartLevel2()
    {
        var exit = new ExitLoc();

        var l0_1 = new CampLoc(exit);

        var l1_1 = new CampLoc(l0_1);
        var l1_2 = new MonsterLoc(l0_1);
        var l1_3 = new CampLoc(l0_1);

        var l2_1 = new CampLoc(l1_1, l1_2);
        var l2_3 = new MonsterLoc(l1_3);

        var l3_1 = new MonsterLoc(l2_1);
        var l3_2 = new CampLoc(l2_1);
        var l3_3 = new MonsterLoc(l2_3);

        var l4_1 = new MonsterLoc(l3_1);
        var l4_2 = new MonsterLoc(l3_2, l3_3);

        var l5_1 = new MonsterLoc(l4_1);
        var l5_2 = new CampLoc(l4_1);
        var l5_3 = new MonsterLoc(l4_2);

        var l6_1 = new CampLoc(l5_1, l5_2, l5_3);
        var l6_2 = new MonsterLoc(l5_3);

        var l7_1 = new CampLoc(l6_1, l6_2);

        GameStarter.Dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                l7_1
            ),
        };

        GoToGame();
    }

    public void StartLevel3()
    {
        var exit = new ExitLoc();

        var l1_1 = new MonsterLoc(exit);
        var l1_2 = new MonsterLoc(exit);
        var l1_3 = new CampLoc(exit);

        var l2_1 = new CampLoc(l1_1, l1_2);
        var l2_3 = new MonsterLoc(l1_3);

        var l3_1 = new CampLoc(l2_1);
        var l3_2 = new MonsterLoc(l2_3, l2_3);
        var l3_3 = new CampLoc(l2_3);

        var l4_1 = new MonsterLoc(l3_1);
        var l4_2 = new CampLoc(l3_2, l3_3);

        var l5_1 = new MonsterLoc(l4_1);
        var l5_2 = new CampLoc(l4_1, l4_2);
        var l5_3 = new CampLoc(l4_2);

        var l6_1 = new CampLoc(l5_1, l5_2, l5_3);
        var l6_2 = new MonsterLoc(l5_3);

        GameStarter.Dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                l6_1,
                l6_2
            ),
        };

        GoToGame();
    }

    public void StartLevel4()
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

        GameStarter.Dungeon = new Dungeon()
        {
            Entrance = new EntranceLoc(
                l6_1, l6_2
            ),
        };

        GoToGame();
    }

    private void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
