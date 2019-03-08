using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        selectionController = GameObject.FindObjectOfType<SelectionController>();
	}

    public Text Title;
    public Text Movement;
    public Text HexPath;
    public Text Strength;

    public GameObject CityBuildButton;

    SelectionController selectionController;
    	
	// Update is called once per frame
	void Update () {

        Unit selectedUnit = selectionController.SelectedUnit;
        if(selectedUnit != null)
        {
            Title.text = selectedUnit.Name; //selectedUnit.guid.ToString();//$"{selectedUnit.Name} {selectedUnit.guid.ToString()}";

            Movement.text = $"Movement: {selectedUnit.MovementRemaining}/{selectedUnit.Movement}";

            Hex[] hexPath = selectionController.SelectedUnit.GetHexPath();
            string hexPathLength = hexPath == null ? "0" : hexPath.Length.ToString();
            HexPath.text = $"Queued Movement: {hexPathLength}";
            Strength.text = $"Strength: {selectedUnit.Strength}";

            if( selectionController.SelectedUnit.CanBuildCities && selectionController.SelectedUnit.Hex.City == null)
            {
                CityBuildButton.SetActive( true );
            }
            else
            {
                CityBuildButton.SetActive( false );
            }

        }

	}
}
