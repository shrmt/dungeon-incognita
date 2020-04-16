using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ILocView
{

}

public class LocView : MonoBehaviour, ILocView, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private Sprite mainSprite;
    [SerializeField] private Sprite hiddenSprite;

    public float Radius = 1;
    public List<PathView> PathViews = new List<PathView>();

    private Action onClick = () => {};

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick();
    }

    public void SetAction(Action onClick)
    {
        this.onClick = onClick;
    }

    public void SetStatus(LocStatus status, bool isHidden)
    {
        spriteRend.sprite = isHidden
            ? hiddenSprite
            : mainSprite;

        switch (status)
        {
            case LocStatus.NonActive:
                spriteRend.color = Color.grey;
                break;
            case LocStatus.Active:
                spriteRend.color = Color.white;
                break;
            case LocStatus.Current:
                spriteRend.color = Color.green;
                break;
            default:
                throw new NotImplementedException(status.ToString());
        }

        foreach (var pathView in PathViews)
        {
            pathView.SetStatus(status);
        }
    }
}

public enum LocStatus
{
    NonActive,
    Active,
    Current,
}
