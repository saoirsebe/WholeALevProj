using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTileMapVisualiser : MonoBehaviour
{
    [SerializeField]
    private Tilemap wallTilemap;
    [SerializeField]
    private TileBase wallTile;

    public void paintFloorTiles(IEnumerable<Vector2Int> floorPositions) //Defining method for painting
    {
        paintTiles(floorPositions, wallTilemap, wallTile);
    }

    private void paintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        wallTilemap.ClearAllTiles();
    }
}
