using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "ScriptableObjects/Plant")]
public class Plant : ScriptableObject
{
    [Header("Plant Settings")]
    [SerializeField] private string plantName;
    public string PlantName => plantName;
    [SerializeField] private string plantDescription;
    public string PlantDescription => plantDescription;
    [SerializeField] private float growthTime = 5.0f;
    public float GrowthTime => growthTime;
    [SerializeField] private float growthRate = 0.1f;
    public float GrowthRate => growthRate;
    [SerializeField] private int harvestPrice = 10;
    public int HarvestPrice => harvestPrice;
    [SerializeField] private int plantCost = 10;
    public int PlantCost => plantCost;

    [Header("Plant Sprites")]
    public Sprite[] plantSprites;

    
}
