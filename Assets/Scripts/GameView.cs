using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IGameView
{
    Dictionary<ILoc, ILocView> CreateDungeon(Dungeon dungeon);
    void ChooseOption(ChoiceRequiredResult choiceResult, Action<ChoiceOption> onChoosed);
    void ShowPopup(string text);
    void MovePlayer(ILocView locView);
    void SetName(string name);
    void SetHealth(int health, int delta);
    void SetMana(int mana, int delta);
    void ShowDeath();
    void ShowWin();
    void SetLocAction(ILocView locView, Action onClick);
    void UpdateLocView(ILocView locView, LocStatus status, bool isHidden);
}

public class GameView : MonoBehaviour, IGameView
{
    [SerializeField] private LocView entrancePrefab;
    [SerializeField] private LocView exitPrefab;
    [SerializeField] private LocView monsterPrefab;
    [SerializeField] private LocView campPrefab;
    [SerializeField] private PathView pathPrefab;
    [SerializeField] private Vector2 locOffset = new Vector2(3, 2);
    [SerializeField] private Vector2 locRandOffset = new Vector2(0.7f, 0.7f);
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private CameraScroller camScroller;
    [SerializeField] private ChoiceView choiceView;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject deathPanel;

    public Dictionary<ILoc, ILocView> CreateDungeon(Dungeon dungeon)
    {
        Debug.Log("Draw dungeon");

        var locViews = new Dictionary<ILoc, ILocView>();

        var leveledLocs = dungeon.GetLeveledLocs();

        foreach (var level in leveledLocs.Keys)
        {
            InstantiateLevel(level, leveledLocs[level], locViews);
        }

        ConnectLocsRecursively(dungeon.Entrance, locViews);

        SetCamBounds(locViews, leveledLocs);

        return locViews;
    }

    public void ChooseOption(ChoiceRequiredResult choiceResult, Action<ChoiceOption> onChoosed)
    {
        var options = choiceResult.Options
            .Select(o => new OptionViewModel(o.Text, o.IsAvailable))
            .ToArray();
        choiceView.Show(choiceResult.Text, options, i => onChoosed(choiceResult.Options[i]));
    }

    public void ShowPopup(string text)
    {
        choiceView.Show(text, new[] { new OptionViewModel("OK", true) }, i => { });
    }

    public void MovePlayer(ILocView locView)
    {
        //Debug.Log($"Move player to loc");
    }

    public void SetName(string name)
    {
        nameText.SetText(name);
    }

    public void SetHealth(int health, int delta)
    {
        healthText.SetText($"Health: {health}");
    }

    public void SetMana(int mana, int delta)
    {
        manaText.SetText($"Mana: {mana}");
    }

    public void ShowDeath()
    {
        Debug.Log("DEATH!!!");
        deathPanel.SetActive(true);
    }

    public void ShowWin()
    {
        Debug.Log("WIN!!!");
        winPanel.SetActive(true);
    }

    public void SetLocAction(ILocView locView, Action onClick)
    {
        locView.ToLocView().SetAction(onClick);
    }

    public void UpdateLocView(ILocView locView, LocStatus status, bool isHidden)
    {
        locView.ToLocView().SetStatus(status, isHidden);
    }

    private void ConnectLocsRecursively(ILoc loc, Dictionary<ILoc, ILocView> locViews)
    {
        if (loc is ExitLoc) return;

        foreach (var l in loc.NextLocs)
        {
            ConnectLocs(locViews[loc].ToLocView(), locViews[l].ToLocView());
            ConnectLocsRecursively(l, locViews);
        }
    }

    private void ConnectLocs(LocView first, LocView second)
    {
        var path = Instantiate(pathPrefab, first.transform);
        path.SetPos(first, second);
        first.PathViews.Add(path);
    }

    private void InstantiateLevel(int level, IList<ILoc> locs, Dictionary<ILoc, ILocView> locViews)
    {
        for (int i = 0; i < locs.Count; i++)
        {
            var loc = locs[i];
            var locView = InstantiateLoc(loc);

            locView.name = loc.GetType().Name + " " + level;

            var offset = new Vector2(
                level * locOffset.x,
                GetLocVerticalPos(i, locs.Count));
            var randOffset = new Vector2(
                Random.Range(-locRandOffset.x, locRandOffset.x),
                Random.Range(-locRandOffset.y, locRandOffset.y));
            locView.transform.position = offset + randOffset;

            locViews[loc] = locView;
        }
    }

    private float GetLocVerticalPos(int index, int total)
    {
        if (total == 1) return 0;
        if (total == 2)
        {
            if (index == 0) return locOffset.y / 2;
            else return -locOffset.y / 2;
        }
        else if (total == 3)
        {
            if (index == 0) return locOffset.y;
            else if (index == 1) return 0;
            else return -locOffset.y;
        }
        throw new NotImplementedException($"Total: {total}");
    }

    private LocView InstantiateLoc(ILoc loc)
    {
        var prefab = GetLocViewPrefab(loc);
        var locView = Instantiate(prefab);
        return locView;
    }

    private LocView GetLocViewPrefab(ILoc loc)
    {
        if (loc is EntranceLoc)
        {
            return entrancePrefab;
        }
        else if (loc is ExitLoc)
        {
            return exitPrefab;
        }
        else if (loc is MonsterLoc)
        {
            return monsterPrefab;
        }
        else if (loc is CampLoc)
        {
            return campPrefab;
        }
        else throw new NotImplementedException(loc.GetType().Name);
    }

    private void SetCamBounds(Dictionary<ILoc, ILocView> locViews,
        Dictionary<int, IList<ILoc>> leveledLocs)
    {
        var maxLevel = leveledLocs.Keys.Max();
        camScroller.SetBounds(
            locViews[leveledLocs[0].First()].ToLocView().transform.position,
            locViews[leveledLocs[maxLevel].First()].ToLocView().transform.position);
    }
}

public class MockGameView : IGameView
{
    public void ChooseOption(ChoiceRequiredResult choiceResult, Action<ChoiceOption> onChoosed)
    {
        var option = choiceResult.Options.First();
        onChoosed(option);
    }

    public Dictionary<ILoc, ILocView> CreateDungeon(Dungeon dungeon)
    {
        var locViews = new Dictionary<ILoc, ILocView>();
        dungeon.ForEachLoc(l => locViews.Add(l, new MockLocView()));
        return locViews;
    }

    public void MovePlayer(ILocView locView)
    {
        
    }

    public void SetHealth(int health, int delta)
    {
        
    }

    public void SetLocAction(ILocView locView, Action onClick)
    {
        
    }

    public void SetMana(int mana, int delta)
    {
        
    }

    public void SetName(string name)
    {
        
    }

    public void ShowDeath()
    {
        
    }

    public void ShowPopup(string text)
    {
        
    }

    public void ShowWin()
    {
        
    }

    public void UpdateLocView(ILocView locView, LocStatus status, bool isHidden)
    {
        
    }
}

public class MockLocView : ILocView
{

}
