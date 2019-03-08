using UnityEngine;
using static MouseController;

public interface IMouseBehavior
{
    void Execute(HexInfo hexInfo, PositionInfo positionInfo, CameraInfo camInfo, SelectionController selectionController);
    bool IsAsync();
    bool IsComplete();
    Vector3 MouseToGroundPlane(Vector3 mousePos);
}
