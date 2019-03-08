using DebugLogging;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public LayerMask LayerIDForHexTiles;
    private HexInfo hexInfo;
    private PositionInfo positionInfo;
    private CameraInfo cameraInfo;
    private SelectionController selectionController;
    private IMouseBehavior mouseBehavior;
    private List<IMouseBehavior> constantBehaviors;

    public void ResetMouseBehavior()
    {
        DebugLogger.Log(LogLevel.Informational, "Resetting mouse behavior", GetType());
        mouseBehavior = null;
    }

    // Use this for initialization
    private void Start()
    {
        hexInfo = new HexInfo
        {
            HexMap = GameObject.FindObjectOfType<HexMap>(),
            LineRenderer = transform.GetComponentInChildren<LineRenderer>()
        };
        hexInfo.CurrentHex = MouseToHex();

        positionInfo = new PositionInfo
        {
            lastMousePosition = Input.mousePosition
        };

        cameraInfo = new CameraInfo
        {
            MouseDragThreshold = 1
        };

        selectionController = GameObject.FindObjectOfType<SelectionController>();

        // A list of Mouse Behaviors which run every frame.
        constantBehaviors = new List<IMouseBehavior>
        {
            new ScrollZoom()
        };
    }

    private void Update()
    {
        // Update the current hex position of the mouse
        hexInfo.PreviousHex = hexInfo.CurrentHex;
        hexInfo.CurrentHex = MouseToHex();

        // if there is no mouse behavior, then we should try to find
        // a new behavior to enable. If there is a mouse behavior, 
        // then we should check to see if it's done.
        if (mouseBehavior != null && mouseBehavior.IsComplete())
        {
            ResetMouseBehavior();
        }
        if (mouseBehavior == null)
        {
            AttemptToSetMouseBehavior();
        }

        // If there is a mouse behavior, then execute that behavior
        if (mouseBehavior != null)
        {
            //TODO: Async?
            mouseBehavior.Execute(hexInfo, positionInfo, cameraInfo, selectionController);
        }

        // execute constant behaviors
        constantBehaviors.ForEach(cb => cb.Execute(hexInfo, positionInfo, cameraInfo, selectionController));

        positionInfo.lastMousePosition = Input.mousePosition;
        if (selectionController.SelectedUnit != null)
        {
            DrawPath((hexInfo.HexPath != null) ? hexInfo.HexPath : selectionController.SelectedUnit.GetHexPath());
        }
        else
        {
            DrawPath(null);   // Clear the path display
        }
    }

    /// <summary>
    /// Responsible for assigning a current mouse behavior
    /// </summary>
    private void AttemptToSetMouseBehavior()
    {
        // Check here(?) to see if we are over a UI element,
        // if so -- ignore mouse clicks and such.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // TODO: Do we want to ignore ALL GUI objects?  Consider
            // things like unit health bars, resource icons, etc...
            // Although, if those are set to NotInteractive or Not Block
            // Raycasts, maybe this will return false for them anyway.
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseBehavior = new UnitSelection();
        }
        else if (selectionController.SelectedUnit != null && Input.GetMouseButtonDown(1))
        {
            // We have a selected unit, and we've pushed down the right
            // mouse button, so enter unit movement mode.
            //Update_CurrentFunc = Update_UnitMovement;
            mouseBehavior = new UnitMovement(this.StartCoroutine);

        }
        else if (Input.GetMouseButton(0) &&
            Vector3.Distance(Input.mousePosition, positionInfo.lastMousePosition) > cameraInfo.MouseDragThreshold)
        {
            // Left button is being held down AND the mouse moved? That's a camera drag!
            mouseBehavior = new CameraMovement();
            positionInfo.lastMouseGroundPlanePosition = mouseBehavior.MouseToGroundPlane(Input.mousePosition);
        }
        else if (selectionController.SelectedUnit != null && Input.GetMouseButton(1))
        {
            // We have a selected unit, and we are holding down the mouse
            // button.  We are in unit movement mode -- show a path from
            // unit to mouse position via the pathfinding system.
        }
    }

    /// <summary>
    /// Based on current mouse position, finds current hex
    /// </summary>
    /// <returns>Found Hex or Null</returns>
    private Hex MouseToHex()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        int layerMask = LayerIDForHexTiles.value;

        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask))
        {
            // Something got hit

            // The collider is a child of the "correct" game object that we want.
            GameObject hexGO = hitInfo.rigidbody.gameObject;

            return hexInfo.HexMap.GetHexFromGameObject(hexGO);
        }
        return null;
    }

    

    private void DrawPath(Hex[] hexPath)
    {
        if (hexPath == null || hexPath.Length == 0)
        {
            hexInfo.LineRenderer.enabled = false;
            return;
        }
        hexInfo.LineRenderer.enabled = true;

        Vector3[] ps = new Vector3[hexPath.Length];

        for (int i = 0; i < hexPath.Length; i++)
        {
            GameObject hexGO = hexInfo.HexMap.GetHexGO(hexPath[i]);
            ps[i] = hexGO.transform.position + (Vector3.up * 0.1f);
        }

        hexInfo.LineRenderer.positionCount = ps.Length;
        hexInfo.LineRenderer.SetPositions(ps);
    }


    public class HexInfo
    {
        public HexMap HexMap;
        public Hex CurrentHex;  // CurrentHex and PreviousHex
        public Hex PreviousHex; // represent the 2 most recent
        public Hex[] HexPath;   // hexes that were moused over.
        public LineRenderer LineRenderer;
    }

    public class PositionInfo
    {
        /// <summary>
        /// From Input.mousePosition
        /// </summary>
        public Vector3 lastMousePosition;
        public Vector3 lastMouseGroundPlanePosition;
    }

    public class CameraInfo
    {
        /// <summary>
        /// Threshold of mouse movement to start a drag
        /// </summary>
        public int MouseDragThreshold;
        public Vector3 CameraTargetOffset;
    }
}