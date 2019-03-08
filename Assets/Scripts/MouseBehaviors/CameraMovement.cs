using DebugLogging;
using UnityEngine;
using static MouseController;

public class CameraMovement : AbstractMouseBehavior
{
    public override void Execute(HexInfo hexInfo, PositionInfo positionInfo, CameraInfo camInfo, SelectionController selectionController)
    {
        if (Input.GetMouseButtonUp(0))
        {
            DebugLogger.Log(LogLevel.Informational, "Cancelling camera drag.", GetType());
            MarkComplete();
            return;
        }

        // Right now, all we need are camera controls
        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

        Vector3 diff = positionInfo.lastMouseGroundPlanePosition - hitPos;
        Camera.main.transform.Translate(diff, Space.World);

        positionInfo.lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);
    }
}
