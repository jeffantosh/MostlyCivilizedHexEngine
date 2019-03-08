using DebugLogging;
using UnityEngine;

public class SkipUnitTurnButton : MonoBehaviour {

    public void SkipUnitTurn()
    {
        SelectionController sc = GameObject.FindObjectOfType<SelectionController>();

        Unit u = sc.SelectedUnit;

        if(u == null)
        {
            DebugLogger.Log(LogLevel.Error, "How is this button active if there's no selected unit?!?!?!", GetType());
            return;
        }

        u.SkipThisUnit = true;

        // Might as well go to the next idle unit

        sc.SelectNextUnit( true );
    }

}
