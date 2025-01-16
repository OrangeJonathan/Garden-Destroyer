using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [SerializeField] private PlantingManager plantingManager;
    
    [Header("Currency")]
    [SerializeField] private float currency = 0;
    public float Currency => currency;
    [SerializeField] private TMP_Text currencyText;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find all FarmTile instances and subscribe to their onHarvest events
        FarmTile[] farmTiles = plantingManager.FarmTiles;
        foreach (var farmTile in farmTiles)
        {
            farmTile.onTileHarvest += OnPlantHarvested;
        }
        currencyText.SetText(currency.ToString());
    }

    public void AddCurrency(float amount)
    {
        currency += amount;
        currencyText.SetText(currency.ToString());
    }

    public void SubtractCurrency(float amount)
    {
        currency -= amount;
        currencyText.SetText(currency.ToString());
    }

    public void OnPlantHarvested(float harvestPrice)
    {
        AddCurrency(harvestPrice);
    }
}
