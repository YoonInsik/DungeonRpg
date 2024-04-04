using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSetData", menuName = "SO/TileSetData")]
public class TileSetData : ScriptableObject
{
    public TileBase floorTile;
    public TileBase wallTile;
}
