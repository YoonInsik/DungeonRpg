using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<int, TileBase> tileDict;

    public void PaintTiles(ChunkData chunkData, Tilemap tilemap)
    {
        for (int x = 0; x < MapManager.CHUNKSIZE; x++)
        {
            for (int y = 0; y < MapManager.CHUNKSIZE; y++)
            {
                PaintSingleTile(tilemap, tileDict[chunkData.tileDatas[x, y]], new Vector2Int(x, y));
            }
        }
    }

    public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }
}
