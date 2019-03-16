using DebugLogging;
using System.Collections.Generic;
using static MouseController;

public class UnitSelection : AbstractMouseBehavior
{
    public override void Execute(HexInfo hexInfo, PositionInfo positionInfo, CameraInfo camInfo, SelectionController selectionController)
    {
        List<Unit> us = hexInfo.CurrentHex.Units;

        if (us.Count > 0)
        {
            DebugLogger.Log(LogLevel.Informational, "Unit(s) exist on this tile!", GetType());
            Unit previousUnit = null;

            // if no unit is currently selected, then simply select the first unit on this hex.
            if (selectionController.SelectedUnit == null)
            {
                selectionController.SelectedUnit = us[0];
            }
            else
            {
                // find the index of the current selected unit
                int currentIndex = us.FindIndex(u => u.Equals(selectionController.SelectedUnit));
                previousUnit = selectionController.SelectedUnit; // just hold this in memory for a bit to log later
                if (currentIndex == -1)
                {
                    // a unit on a different hex was selected
                    selectionController.SelectedUnit = us[0];
                }
                else if (currentIndex == us.Count - 1)
                {
                    // the same tile was selected!
                    // if this is the last unit on this hex, then de select the unit
                    selectionController.SelectedUnit = null;
                }
                else
                {
                    // the same tile was selected!
                    // select the next unit on this hex
                    selectionController.SelectedUnit = us[currentIndex + 1];
                }
            }

            // update the hex map's hex path so the pathfinding guideline resets.
            hexInfo.HexPath = selectionController.SelectedUnit?.GetHexPath();

            LogMessage(previousUnit, selectionController.SelectedUnit);
        }

        MarkComplete();
    }

    private void LogMessage(Unit previousUnit, Unit nextUnit)
    {
        string previousUnitName = previousUnit != null ? $"{previousUnit.Name} ({previousUnit.Guid})" : "null";
        string nextUnitName = nextUnit != null ? $"{nextUnit.Name} ({nextUnit.Guid})" : "null";


        DebugLogger.Log(LogLevel.Informational, $"Unit Selection: {previousUnitName} deselected; {nextUnitName} selected.", GetType());
    }
}
