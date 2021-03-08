using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] Transform CameraTransform;
    [SerializeField] Vector3[] bounds;
    Vector2 TouchPos;
    Vector2 NewTouchPos;
    bool Dragging = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Dragging)
            {
                NewTouchPos = Input.mousePosition;
            }
            else
            {
                TouchPos = Input.mousePosition;
                NewTouchPos = Input.mousePosition;
                Dragging = true;
            }
        }
        else
        {
            Dragging = false;
        }
        Vector2 Diff;
        if (Dragging)
        {
            Diff = (TouchPos - NewTouchPos) * 0.004f;
        }
        else
        {
            Diff = Vector2.zero;
        }
        TouchPos = NewTouchPos;
        Transform tc = CameraTransform;
        Vector3 p = tc.localPosition + Vector3.right * Diff.x * tc.position.y + Vector3.forward * Diff.y * tc.position.y + tc.forward * GetZoom();
        if (p.x <= bounds[0].x || p.x >= bounds[1].x)
        {
            p.x = tc.localPosition.x;
        }
        if (p.y <= bounds[0].y || p.y >= bounds[1].y)
        {
            p.y = tc.localPosition.y;
        }
        if (p.z <= bounds[0].z || p.z >= bounds[1].z)
        {
            p.z = tc.localPosition.z;
        }
        tc.localPosition = p;
    }

    float GetZoom()
    {
        return Input.mouseScrollDelta.y;
    }

}
