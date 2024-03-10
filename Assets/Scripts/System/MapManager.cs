using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public List<TileData> tiles;
}

public class TileData
{
    public BaseUnit occupiedUnit;

}
