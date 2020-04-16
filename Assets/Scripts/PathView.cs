using System;
using UnityEngine;

public class PathView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRend;

    public void SetStatus(LocStatus status)
    {
        switch (status)
        {
            case LocStatus.Current:
                spriteRend.color = Color.white;
                break;
            case LocStatus.NonActive:
            case LocStatus.Active:
                spriteRend.color = Color.grey;
                break;
            default:
                throw new NotImplementedException(status.ToString());
        }
    }

    public void SetPos(LocView first, LocView second)
    {
        var firstPos = first.transform.position;
        var secondPos = second.transform.position;

        SetPos(
            firstPos + (secondPos - firstPos).normalized * first.Radius,
            secondPos + (firstPos - secondPos).normalized * second.Radius);    
    }

    private void SetPos(Vector2 start, Vector2 end)
    {
        var distance = (end - start).magnitude;
        spriteRend.size = new Vector2(distance, spriteRend.size.y);

        var dir = end - start;
        transform.position = start + dir / 2;
        transform.right = dir;
    }
}
