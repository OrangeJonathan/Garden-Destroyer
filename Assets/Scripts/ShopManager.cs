using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance { get; private set; }
    public static bool IsShopOpen { get; set; }

    [Header("Setup")]
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private PlantingManager plantingManager;

    [Header("Tile Shop")]
    private FarmTile[] farmTiles;
    [SerializeField] private int unlockedTiles;
    [SerializeField] private int baseTilePrice = 10;
    [SerializeField] private int currentTilePrice;
    public int CurrentTilePrice => currentTilePrice;

    [Header("Plant Shop")]
    [SerializeField] private Plant[] plantsInShop;
    [SerializeField] private ShopSquare[] shopSquares;

    #region Setup
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Start()
    {
        currentTilePrice = baseTilePrice;
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure all tiles are initialized
        farmTiles = plantingManager.FarmTiles;
        SetupShopSquares();
        CheckUnlockedTiles();
    }
    #endregion

    #region Tile Shop
    public void PurchaseTile(FarmTile tile)
    {
        if (currencyManager.Currency < currentTilePrice)
        {
            Debug.Log("Not enough currency to purchase tile");
            return;
        }

        if (!tile.IsUnlocked)
        {
            currencyManager.SubtractCurrency(currentTilePrice);
            tile.Unlock();
            CheckUnlockedTiles();
            currentTilePrice = (int)(baseTilePrice * Math.Pow(2, unlockedTiles));
            UpdateAllCostValueTexts();
        }
    }

    public void CheckUnlockedTiles()
    {
        unlockedTiles = 0;

        foreach (var tile in farmTiles)
        {
            Debug.Log("tile: " + tile + ", IsUnlocked: " + tile.IsUnlocked);
            if (tile.IsUnlocked)
            {
                unlockedTiles++;
            }
        }

        for (int i = 0; i < farmTiles.Length; i++)
        {
            Debug.Log("Setting unlock button visibility for tile " + i + ": " + (i == unlockedTiles));
            farmTiles[i].SetUnlockButtonVisibility(i == unlockedTiles);
        }
    }

    private void UpdateAllCostValueTexts()
    {
        foreach (var tile in farmTiles)
        {
            tile.UpdateCostValueText();
        }
    }

    public void UpgradeGrowthRate(FarmTile tile)
    {
        if (currencyManager.Currency < tile.GrowthRateMultiplierUpgradePrice)
        {
            Debug.Log("Not enough currency to upgrade growth rate");
            return;
        }
        currencyManager.SubtractCurrency(tile.GrowthRateMultiplierUpgradePrice);
        tile.UpgradeGrowthRate();
    }

    public void UpgradeTileValue(FarmTile tile)
    {
        if (currencyManager.Currency < tile.TileValueMultiplierUpgradePrice)
        {
            Debug.Log("Not enough currency to upgrade tile value");
            return;
        }
        currencyManager.SubtractCurrency(tile.TileValueMultiplierUpgradePrice);
        tile.UpgradeTileValue();
    }

    #endregion


    #region Plant Shop

    private void SetupShopSquares()
    {
        foreach (var shopSquare in shopSquares)
        {
            AddPlantToShopSquare(shopSquare);
        }
    }
    public bool BuyPlant(Plant plant)
    {
        if (currencyManager.Currency < plant.PlantCost)
        {
            Debug.Log("Not enough currency to buy plant");
            return false;
        }
        currencyManager.SubtractCurrency(plant.PlantCost);
        plantingManager.AddPlantType(plant);
        return true;
    }

    public void AddPlantToShopSquare(ShopSquare shopSquare)
    {
        if (plantsInShop.Length == 0)
        {
            Debug.Log("No more plants in shop");
            shopSquare.SetPlant(null);
            return;
        }
        Plant plant = plantsInShop[0];
        shopSquare.SetPlant(plant);
        plantsInShop = plantsInShop[1..];
    }

    public void RemovePlantFromShopSquare(ShopSquare shopSquare)
    {
        shopSquare.SetPlant(null);
    }

    public void AddPlantToShopList(Plant plant)
    {

    }




    #endregion
}