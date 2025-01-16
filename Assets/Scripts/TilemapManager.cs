using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap nonWalkableTilemap;
    [SerializeField] private LayerMask walkableLayer; // Layer mask for walkable tiles
    [SerializeField] private LayerMask nonWalkableLayer; // Layer mask for non-walkable tiles

    public bool IsTileWalkable(Vector3Int cellPosition)
    {
        // First, check if the tile is within the walkable layer
        if (walkableTilemap.HasTile(cellPosition))
        {
            return true; // This tile is walkable
        }

        // Then, check if the tile is within the non-walkable layer
        if (nonWalkableTilemap.HasTile(cellPosition))
        {
            return false; // This tile is non-walkable
        }

        return false; // If tile is not found, assume it's non-walkable
    }
}
