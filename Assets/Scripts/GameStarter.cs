using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public static Dungeon Dungeon;
    public static int LevelIndex = -1;

    [SerializeField] private GameView view;

    private GameController controller;
    private string familyName;

    public void StartGameWithNewPlayer()
    {
        var player = CreatePlayer();
        controller.StartGame(player);
    }

    public void StartGame(string playerName, string familyName)
    {
        this.familyName = familyName;
        var player = CreatePlayer(playerName);
        controller.StartGame(player);
    }

    private void Start()
    {
        controller = new GameController(view, Dungeon);
    }

    private Player CreatePlayer(string name = null)
    {
        return new Player()
        {
            Name = name ?? NameHelper.GenerateName(),
            FamilyName = familyName,
            Health = 5,
            Mana = 0,
        };
    }
}
