using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private IGameView view;
    private Dungeon dungeon;
    private Dictionary<ILoc, ILocView> locViews;
    private List<ILoc> activeLocs = new List<ILoc>();
    private List<ILoc> discoveredLocs = new List<ILoc>();
    private List<Player> deadPlayers = new List<Player>();
    public bool IsWin { get; private set; }
    public ILoc CurrentLoc { get; private set; }

    public GameController(IGameView view, Dungeon dungeon)
    {
        this.view = view;
        this.dungeon = dungeon;
        locViews = view.CreateDungeon(dungeon);
    }

    public void StartGame(Player player)
    {
        view.SetName($"{player.Name} {player.FamilyName}");
        view.SetHealth(player.Health, 0);
        view.SetMana(player.Mana, 0);

        dungeon.ForEachLoc(l =>
        {
            UpdateLoc(l, LocStatus.NonActive);
        });

        var loc = dungeon.Entrance;
        var result = loc.EnterLoc(player);
        HandleLocEntryResult(loc, result, player);
    }

    public bool IsDead(Player player)
    {
        return deadPlayers.Contains(player);
    }

    public void HandleLocEntryResult(ILoc loc, ILocEntryResult result, Player player)
    {
        if (result is ChoiceRequiredResult choiceRequired)
        {
            view.ChooseOption(choiceRequired, o =>
            {
                HandleLocEntryResult(loc, o.Result, player);
            });
            return;
        }

        DeactivateLocs();
        CurrentLoc = loc;
        discoveredLocs.Add(loc);
        activeLocs.Add(loc);
        UpdateLoc(loc, LocStatus.Current);
        view.MovePlayer(GetView(loc));

        if (result is OkResult okResult)
        {
            if (okResult.DeltaHealth != 0)
            {
                player.Health += okResult.DeltaHealth;
                view.SetHealth(player.Health, okResult.DeltaHealth);
            }

            if (okResult.DeltaMana != 0)
            {
                player.Mana += okResult.DeltaMana;
                view.SetMana(player.Mana, okResult.DeltaMana);
            }

            if (player.Health <= 0)
            {
                deadPlayers.Add(player);
                DeactivateLocs();
                view.ShowDeath();
                return;
            }
            else if (!string.IsNullOrEmpty(okResult.Text))
            {
                view.ShowPopup(okResult.Text);
            }
        }

        if (loc is ExitLoc)
        {
            IsWin = true;
            view.ShowWin();
            return;
        }

        foreach (var l in loc.NextLocs)
        {
            UpdateLoc(l, LocStatus.Active);

            view.SetLocAction(GetView(l), () =>
            {
                var r = l.EnterLoc(player);
                HandleLocEntryResult(l, r, player);
            });

            activeLocs.Add(l);
        }
    }

    private ILocView GetView(ILoc loc)
    {
        return locViews[loc];
    }

    private void UpdateLoc(ILoc loc, LocStatus status)
    {
        var isHidden = !discoveredLocs.Contains(loc);
        view.UpdateLocView(GetView(loc), status, isHidden);
    }

    private void DeactivateLocs()
    {
        foreach (var loc in activeLocs)
        {
            var locView = locViews[loc];
            view.SetLocAction(locView, () => { });
            UpdateLoc(loc, LocStatus.NonActive);
        }

        activeLocs.Clear();
    }
}
