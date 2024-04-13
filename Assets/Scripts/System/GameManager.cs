using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public const int MAXSTAGE = 5;
    
    [SerializeField] private int stage;
    public int Stage { get { return stage; } private set { stage = value; } }

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float timer;

    void Start()
    {
        Stage = 0;

        UnitManager.Instance.SpawnPlayer(Vector2Int.zero);
        MapManager.Instance.InitMap();

        timer = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator StartTimer()
    {
        timer = 10;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timeText.text = Mathf.Round(timer).ToString();

            yield return null;
        }
    }

    public void EnterChunk()
    {
        stage++;
        MapManager.Instance.ReloadChunks();
        MapManager.Instance.CurChunk.InvokeEvent();
    }
}
