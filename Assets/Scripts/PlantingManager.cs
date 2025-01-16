using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantingManager : MonoBehaviour
{
    [Header("Settings")]
    private float timeBetweenPlanting = 5.0f;
    private float timeSinceLastPlanting = 0.0f;
    private bool isPlanting = false;

    [Header("Farm Tiles")]
    [SerializeField] private FarmTile[] farmTiles;
    public FarmTile[] FarmTiles => farmTiles;

    [Header("Plants")]
    [SerializeField] private List<Plant> plantTypes = new List<Plant>();

    [Header("NPC")]
    [SerializeField] private NPC npc;

    public void Update()
    {
        AssignPlantingTaskToNPC();
    }

    private void AssignPlantingTaskToNPC()
    {
        if (isPlanting)
        {
            return;
        }

        if (timeSinceLastPlanting < timeBetweenPlanting)
        {
            timeSinceLastPlanting += Time.deltaTime;
            return;
        }

        FarmTile randomTile = farmTiles[Random.Range(0, farmTiles.Length)];
        Plant randomPlant = plantTypes[Random.Range(0, plantTypes.Count)];

        if (randomTile.IsPlanted() || !randomTile.IsUnlocked) return;

        npc.AssignPlantingTask(randomTile, randomPlant);
        isPlanting = true;
    }

    public void OnPlantingCompleted()
    {
        isPlanting = false;
        timeSinceLastPlanting = 0.0f;
    }

    public List<Plant> GetPlantTypes()
    {
        return plantTypes;
    }

    public void SetPlantTypes(List<Plant> plants)
    {
        plantTypes = plants;
    }

    public void AddPlantType(Plant plant)
    {
        if (!plantTypes.Contains(plant))
        {
            plantTypes.Add(plant);
        }
    }
}