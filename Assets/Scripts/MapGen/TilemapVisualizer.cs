using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<int, TileSetData> dict;

    public void PaintTiles(Tilemap tilemap)
    {
        for (int x = 0; x < MapManager.CHUNKSIZE; x++)
        {
            for (int y = 0; y < MapManager.CHUNKSIZE; y++)
            {
                //PaintSingleTileLocal(tilemap, dict[GameManager.Instance.Stage].floorTile, new Vector2Int(x - MapManager.HALFCHUNKSIZE, y - MapManager.HALFCHUNKSIZE));
                PaintSingleTileLocal(tilemap, dict[GameManager.Instance.Stage % dict.Count].floorTile, new Vector2Int(x - MapManager.HALFCHUNKSIZE, y - MapManager.HALFCHUNKSIZE));
            }
        }
    }

    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }
    
    public void PaintSingleTileLocal(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.LocalToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }
}
