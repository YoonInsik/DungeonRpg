using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private AbstractMapGen mapGen;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> wallPositions;
    public Dictionary<Vector2Int, Tile> tileDict;

    public void GenerateMap()
    {
        mapGen.GenerateDungeon(out floorPositions, out wallPositions);
        UpdateMap();
    }

    private void UpdateMap()
    {
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        tilemapVisualizer.PaintWallTile(wallPositions);
    }
}

public class Tile
{
    public BaseUnit occupiedUnit;
}
