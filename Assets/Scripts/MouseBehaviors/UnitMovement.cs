using DebugLogging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MouseController;

public class UnitMovement : AbstractMouseBehavior
{
    public override void Execute(HexInfo hexInfo, PositionInfo positionInfo, CameraInfo camInfo, SelectionController selectionController)
    {
        if (Input.GetMouseButtonUp(1) || selectionController.SelectedUnit == null)
        {
            DebugLogger.Log(LogLevel.Informational, "Complete unit movement.", GetType());

            if (selectionController.SelectedUnit != null)
            {
                selectionController.SelectedUnit.SetHexPath(hexInfo.HexPath);

                // TODO: Tell Unit and/or HexMap to process unit movement

                StartCoroutine(hexInfo.HexMap.DoUnitMoves(selectionController.SelectedUnit));
            }

            MarkComplete();
            return;
        }

        // We have a selected unit
        // Look at the hex under our mouse
        // Is this a different hex than before (or we don't already have a path)
        if (hexInfo.HexPath == null || hexInfo.CurrentHex != hexInfo.PreviousHex)
        {
            // Do a pathfinding search to that hex
            hexInfo.HexPath = QPath.QPath.FindPath<Hex>(hexInfo.HexMap, selectionController.SelectedUnit, selectionController.SelectedUnit.Hex, hexInfo.CurrentHex, Hex.CostEstimate);
        }
    }
}
