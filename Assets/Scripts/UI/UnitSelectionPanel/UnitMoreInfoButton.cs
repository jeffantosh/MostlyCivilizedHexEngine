using UnityEngine;
using UnityEditor;

public class UnitMoreInfoButton : MonoBehaviour
{

    public void MoreInfo()
    {
        // TODO: Do something better here.
        SelectionController sc = GameObject.FindObjectOfType<SelectionController>();
        Unit unit = sc.SelectedUnit;
        string title = $"Additional Properties for {unit?.Name}";
        string message = $"GUID: {unit?.Guid}\nCan Build Cities: {unit?.CanBuildCities}";
        EditorUtility.DisplayDialog(title, message, "Ok");
    }
}
