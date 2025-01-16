using UnityEngine;
using UnityEngine.UI;

public class ShopToggle : MonoBehaviour
{
    [SerializeField] private PlantingManager plantingManager;

    private bool isShopOpen = false;
    private FarmTile[] farmTiles;

    public void Start()
    {
        farmTiles = plantingManager.FarmTiles;
    }
    public void ToggleShop()
    {
        Debug.Log("Toggling Shop" + isShopOpen);
        isShopOpen = !isShopOpen;
        ShopManager.IsShopOpen = isShopOpen;
        foreach (var farmTile in farmTiles)
        {
            if (!farmTile.IsUnlocked) continue;
            var tileUpgrades = farmTile.gameObject.transform.Find("Canvas/TileUpgrades").gameObject;
            tileUpgrades.SetActive(isShopOpen);
        }
    }
}