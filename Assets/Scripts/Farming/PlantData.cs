using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;

    public int daysToMature = 2;
    public int daysBeforeWither = 2;

    public int sellValue = 10;

    [Header("Models")]
    public GameObject plantedModel;
    public GameObject growingModel;
    public GameObject matureModel;
    public GameObject witheredModel;
}