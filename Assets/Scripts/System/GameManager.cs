using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public const int MAXSTAGE = 5;
    
    [SerializeField] private int stage;
    public int Stage { get { return stage; } private set { stage = value; } }

    void Start()
    {
        Stage = 0;

        UnitManager.Instance.SpawnPlayer(Vector2Int.zero);
        MapManager.Instance.InitMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopAllCoroutines();
        }
    }

    public void EnterChunk()
    {
        stage++;
        MapManager.Instance.ReloadChunks();
        MapManager.Instance.CurChunk.InvokeEvent();
    }
}
