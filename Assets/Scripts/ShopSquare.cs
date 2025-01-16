using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSquare : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Plant plant;
    [SerializeField] private SpriteRenderer plantSpriteRenderer;

    [Header("UI")]
    [SerializeField] private TMP_Text plantName;
    [SerializeField] private TMP_Text costText;

    void Start()
    {
        if (plant == null) 
        {
            plantName.text = "Empty";
            costText.text = "";
            return;
        }
        plantName.text = plant.PlantName;
        costText.text = plant.PlantCost.ToString();
    }

    public void OnMouseDown()
    {
        Debug.Log("ShopSquare clicked");
        if (plant == null) return;

        if (ShopManager.Instance.BuyPlant(plant))
        {
            Debug.Log("Plant purchased");
            ShopManager.Instance.AddPlantToShopSquare(this);
            return;
        }
        // do something if the plant is not purchased
        Debug.Log("Plant not purchased");
    }

    public void SetPlant(Plant plant)
    {
        this.plant = plant;
        if (plant == null) 
        {
            plantName.text = "Empty";
            costText.text = "";
            plantSpriteRenderer.sprite = null;
            return;
        }
        plantName.text = plant.PlantName;
        costText.text = plant.PlantCost.ToString();
        plantSpriteRenderer.sprite = plant.plantSprites[^1];
    }
}
