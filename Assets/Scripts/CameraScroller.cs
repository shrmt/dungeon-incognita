using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    public float dragSpeed = 2;
    public float boundsOffset = 1;
    public Camera cam;

    private Vector3 dragOrigin;
    private float outerLeft = -10f;
    private float outerRight = 10f;

    public void SetBounds(Vector2 first, Vector2 second)
    {
        outerLeft = first.x - boundsOffset;
        outerRight = second.x + boundsOffset;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, 0);

        dragOrigin = Input.mousePosition;

        var camSizeX = cam.orthographicSize * cam.aspect;

        if (move.x > 0f)
        {
            if (transform.position.x + camSizeX < outerRight)
            {
                transform.Translate(move, Space.World);
            }
        }
        else
        {
            if (transform.position.x - camSizeX > outerLeft)
            {
                transform.Translate(move, Space.World);
            }
        }
    }
}
