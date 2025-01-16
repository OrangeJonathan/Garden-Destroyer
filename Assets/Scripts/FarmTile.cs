using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmTile : MonoBehaviour
{
    public event Action<float> onTileHarvest;
    [Header("Farm Tile Settings")]
    public Plant plant;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isPlanted = false;
    [SerializeField] private float growth = 0.0f;
    [Range(0, 4)][SerializeField] private int growthStage = 0;
    [SerializeField] private float tileValue;
    private bool isGrown = false;
    [SerializeField] private SpriteRenderer plantSpriteRenderer;

    [Header("Upgrades")]
    [SerializeField] private float growthRateMultiplierUpgrade = 1.0f;
    [SerializeField] private float growthRateMultiplierUpgradePrice = 10.0f;
    public float GrowthRateMultiplierUpgradePrice => growthRateMultiplierUpgradePrice;
    [SerializeField] private TMP_Text growthRateMultiplierUpgradePriceText;
    [SerializeField] private float tileValueMultiplierUpgrade = 1.0f;
    [SerializeField] private float tileValueMultiplierUpgradePrice = 10.0f;
    public float TileValueMultiplierUpgradePrice => tileValueMultiplierUpgradePrice;
    [SerializeField] private TMP_Text tileValueMultiplierUpgradePriceText;

    [Header("UI")]
    [SerializeField] private Button unlockButton;
    [SerializeField] private TMP_Text costValueText;
    [SerializeField] private Button upgradeGrowthRateButton;
    [SerializeField] private Button upgradeTileValueButton;
    [SerializeField] private Slider growthStageSlider;
    
    


    [Header("Colours")]
    [SerializeField] private ColourSwatch lockedColour;
    [SerializeField] private ColourSwatch unlockedColour;


    private ShopManager shopManager;

    public void Start()
    {
        isUnlocked = false;
        unlockButton.onClick.AddListener(TryPurchaseTile);
        unlockButton.gameObject.SetActive(false);
        growthStageSlider.gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().color = lockedColour.SwatchColour;
        growthRateMultiplierUpgradePriceText.SetText("+ Speed " + "(" + growthRateMultiplierUpgradePrice.ToString() + ")");
        tileValueMultiplierUpgradePriceText.SetText("+ Value " + "(" + tileValueMultiplierUpgradePrice.ToString() + ")");
        shopManager = ShopManager.Instance;
        UpdateCostValueText();

        upgradeGrowthRateButton.onClick.AddListener(() => shopManager.UpgradeGrowthRate(this));
        upgradeTileValueButton.onClick.AddListener(() => shopManager.UpgradeTileValue(this));
    }

    public void Update()
    {
        Grow();
    }

    public void PlantSeed(Plant newPlant)
    {
        plant = newPlant;
        isPlanted = true;
        growth = 0.0f;
        growthStageSlider.gameObject.SetActive(true);
        isGrown = false;
        SetGrowthSlider(0, plant.GrowthTime);
        UpdateSprite();
    }
    public void OnMouseDown()
    {
        if (ShopManager.IsShopOpen) return;
        HarvestPlant();
    }
    private void HarvestPlant()
    {   
        Debug.Log("clicked");
        if (!isPlanted) return;
        Debug.Log("harvested");

        onTileHarvest.Invoke(tileValue);

        plant = null;
        isPlanted = false;
        growth = 0.0f;
        growthStage = 0;
        tileValue = 0;
        isGrown = false;
        UpdateGrowthSlider(0);
        growthStageSlider.gameObject.SetActive(false);
        UpdateSprite();
    }

    private void Grow()
    {
        if (!isPlanted || isGrown) return;

        growth += plant.GrowthRate * Time.deltaTime * growthRateMultiplierUpgrade;
        if (growth >= plant.GrowthTime)
        {
            isGrown = true;
            growth = plant.GrowthTime; // Clamp growth to max
        }
        UpdateGrowthSlider(growth);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (plant == null || plant.plantSprites == null || plant.plantSprites.Length == 0) 
        {
            plantSpriteRenderer.sprite = null;
            return;
        }
    
        float[] growthThresholds = { 0.25f, 0.5f, 0.75f, 1.0f };
        float[] priceMultipliers = { 0.25f, 0.5f, 0.75f, 1.0f };
    
        for (int i = growthThresholds.Length - 1; i >= 0; i--)
        {
            if (growth >= plant.GrowthTime * growthThresholds[i])
            {
                growthStage = i + 1;
                tileValue = plant.HarvestPrice * priceMultipliers[i] * tileValueMultiplierUpgrade;
                plantSpriteRenderer.sprite = plant.plantSprites[growthStage];
                return;
            }
        }
    
        growthStage = 0;
        tileValue = 0;
        plantSpriteRenderer.sprite = plant.plantSprites[growthStage];
    }

    public bool IsPlanted()
    {
        return isPlanted;
    }

    public bool IsUnlocked
    {
        get { return isUnlocked; }
    }

    private void TryPurchaseTile()
    {
        if (ShopManager.Instance == null)
        {
            Debug.LogError("ShopManager instance is not set.");
            return;
        }
        shopManager.PurchaseTile(this);
    }

    public void Unlock()
    {
        isUnlocked = true;
        gameObject.GetComponent<SpriteRenderer>().color = unlockedColour.SwatchColour;
        unlockButton.gameObject.SetActive(false);
        shopManager.CheckUnlockedTiles();
    }

    public void SetUnlockButtonVisibility(bool isVisible)
    {
        Debug.Log("Setting unlock button visibility to: " + isVisible);
        unlockButton.gameObject.SetActive(isVisible);
    }

    public void UpdateCostValueText()
    {
        if (costValueText != null)
        {
            costValueText.SetText(shopManager.CurrentTilePrice.ToString());
        }
    }

    public void UpgradeGrowthRate()
    {
        growthRateMultiplierUpgrade *= 1.1f; 
        growthRateMultiplierUpgradePrice *= 2.0f; 
        growthRateMultiplierUpgradePriceText.SetText("+ Speed " + "(" + growthRateMultiplierUpgradePrice.ToString() + ")");
    }

    public void UpgradeTileValue()
    {
        tileValueMultiplierUpgrade *= 1.1f; // Example upgrade logic
        tileValueMultiplierUpgradePrice *= 2.0f; // Example upgrade logic
        tileValueMultiplierUpgradePriceText.SetText("+ Value " + "(" + tileValueMultiplierUpgradePrice.ToString() + ")");

    }

    private void SetGrowthSlider(float minValue, float maxValue)
    {
        growthStageSlider.minValue = minValue;
        growthStageSlider.maxValue = maxValue;
    }

    private void UpdateGrowthSlider(float value)
    {
        growthStageSlider.value = value;
    }

}

