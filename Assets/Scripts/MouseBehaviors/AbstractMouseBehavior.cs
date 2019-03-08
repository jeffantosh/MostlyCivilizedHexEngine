using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MouseController;

public abstract class AbstractMouseBehavior : MonoBehaviour, IMouseBehavior
{
    private bool isComplete = false;

    public abstract void Execute(HexInfo hexInfo, PositionInfo positionInfo, CameraInfo camInfo, SelectionController selectionController);

    public bool IsAsync()
    {
        return false;
    }
    
    public void MarkComplete()
    {
        isComplete = true;
    }

    public bool IsComplete()
    {
        return isComplete;
    }


    // TODO: Move to helper class so it can be static?
    public Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        // What is the point at which the mouse ray intersects Y=0
        if (mouseRay.direction.y >= 0)
        {
            return Vector3.zero;
        }
        float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }
}
