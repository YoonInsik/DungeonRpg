using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        UnitManager.Instance.SpawnPlayer(Vector2Int.zero);
    }

}
