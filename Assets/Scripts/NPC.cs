using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour
{
    public float speed = 2.0f;
    [SerializeField] private FarmTile targetTile;
    private Plant plantToPlant;
    [SerializeField] private PlantingManager plantingManager;

    [Header("NavMesh")]
    [SerializeField] private NavMeshAgent navMeshAgent;

    public float distance;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    public void AssignPlantingTask(FarmTile tile, Plant plant)
    {
        targetTile = tile;
        plantToPlant = plant;
    }

    void Update()
    {
        if (targetTile != null)
        {
            MoveTowardsTile();
        }
    }

    private void MoveTowardsTile()
    {
        navMeshAgent.SetDestination(targetTile.transform.position);
        Debug.DrawLine(transform.position, targetTile.transform.position, Color.red);

        distance = Vector3.Distance(transform.position, targetTile.transform.position);

        if (Vector3.Distance(transform.position, targetTile.transform.position) < 1f)
        {
            PlantSeed();
        }
    }

    private void PlantSeed()
    {
        if (!targetTile.IsPlanted() && targetTile.IsUnlocked)
        {
            targetTile.PlantSeed(plantToPlant);
        }
        targetTile = null;
        plantingManager.OnPlantingCompleted();
    }
}