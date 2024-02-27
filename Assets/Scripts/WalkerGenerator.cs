using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum TileType
    {
        FLOOR,
        WALL,
        EMPTY
    }

    public TileType[,] tilemapHandler;
    public List<WalkerObject> walkers;
    public Tilemap tilemap;
    public Tile floor;
    public Tile wall;

    public int mapWidth = 30;
    public int mapHeight = 30;
    public int maximumWalkers = 10;
    public int tileCount = default;
    public float fillPercentage = 0.4f;
    public float waitTime = 0.05f;

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        // �� �� Ÿ�Ϸ� ����
        tilemapHandler = new TileType[mapWidth, mapHeight];

        for (int x = 0; x < tilemapHandler.GetLength(0); x++)
        {
            for (int y = 0; y < tilemapHandler.GetLength(1); y++)
            {
                tilemapHandler[x, y] = TileType.EMPTY;
            }
        }

        // ��Ŀ ����Ʈ
        walkers = new List<WalkerObject>();

        // �� �߾ӿ� ù��° ��Ŀ ��ġ
        Vector3Int tileCenter = new Vector3Int(tilemapHandler.GetLength(0) / 2, tilemapHandler.GetLength(1) / 2, 0);
        WalkerObject curWalker = new WalkerObject(new Vector2(tileCenter.x, tileCenter.y), GetDirection(), 0.5f);
        tilemapHandler[tileCenter.x, tileCenter.y] = TileType.FLOOR;
        tilemap.SetTile(tileCenter, floor);
        walkers.Add(curWalker);

        tileCount++;

        StartCoroutine(CreateFloors());
    }

    private Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    public int maxEvadeDistance = 30;


    private Vector2 GetDirectionEvadeOtherWalker()
    {
        // ����
        // �������� ���� ���� ���
        // �ٸ� ��Ŀ�� �ִ� �ʺ��ٴ� ����ִ� ������ ���� ������ Ȯ���� ����

        float[] weights = new float[4];
        weights[0] = 0.2f;


        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    private IEnumerator CreateFloors()
    {
        // tileCount�� ��ü Ÿ�� ������ ���� �ۼ�Ʈ�� �ɶ����� ����
        while ((float)tileCount / (float)tilemapHandler.Length < fillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.position.x, (int)curWalker.position.y, 0);

                if (tilemapHandler[curPos.x, curPos.y] != TileType.FLOOR)
                {
                    tilemap.SetTile(curPos, floor);
                    tileCount++;
                    tilemapHandler[curPos.x, curPos.y] = TileType.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            //Walker Methods
            ChanceToRemoveWalkers();
            ChanceToCreateNewWalker();
            ChanceToChangeWalkersDirection();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(waitTime);
            }
        }

        StartCoroutine(CreateWalls());
    }

    private void ChanceToRemoveWalkers()
    {
        // ��Ŀ�� 2�� �̻��� �� �� ��Ŀ�� Ȯ���� ���� �����
        int walkerCountBeforeChange = walkers.Count;
        for (int i = 0; i < walkerCountBeforeChange; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count > 1)
            {
                walkers.RemoveAt(i);
                break;
            }
        }
    }

    private void ChanceToChangeWalkersDirection()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange)
            {
                walkers[i].direction = GetDirection();
            }
        }
    }

    private void ChanceToCreateNewWalker()
    {
        // �ִ� ���� ���Ѿȿ��� �������� ��Ŀ�� �� ��Ŀ�� ����
        int walkerCountBeforeChange = walkers.Count;
        for (int i = 0; i < walkerCountBeforeChange; i++)
        {
            if (UnityEngine.Random.value < walkers[i].chanceToChange && walkers.Count < maximumWalkers)
            {
                Vector2 newDir = GetDirection();
                Vector2 newPos = walkers[i].position;

                WalkerObject newWalker = new WalkerObject(newPos, newDir, 0.5f);
                walkers.Add(newWalker);
            }
        }
    }

    private void UpdatePosition()
    {
        for (int i = 0; i < walkers.Count; i++)
        {
            WalkerObject curWalker = walkers[i];
            curWalker.position += curWalker.direction;
            // max�� -2�ϴ� ���� = 0~max-1�� �ֿܰ��� ������ ���̶�
            curWalker.position.x = Mathf.Clamp(curWalker.position.x, 1, tilemapHandler.GetLength(0) - 2);
            curWalker.position.y = Mathf.Clamp(curWalker.position.y, 1, tilemapHandler.GetLength(1) - 2);
            walkers[i] = curWalker;
        }
    }

    private IEnumerator CreateWalls()
    {
        // �ٴ�Ÿ�ϸ� �׸� ���¿��� �ٴ�Ÿ���� �����¿� �˻��ؼ� ����̸� �� �׸���
        for (int x = 0; x < tilemapHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < tilemapHandler.GetLength(1) - 1; y++)
            {
                if (tilemapHandler[x,y] == TileType.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (tilemapHandler[x + 1, y] == TileType.EMPTY)
                    {
                        tilemap.SetTile(new Vector3Int(x + 1, y, 0), wall);
                        tilemapHandler[x + 1, y] = TileType.WALL;
                        hasCreatedWall = true;
                    }
                    if (tilemapHandler[x - 1, y] == TileType.EMPTY)
                    {
                        tilemap.SetTile(new Vector3Int(x - 1, y, 0), wall);
                        tilemapHandler[x - 1, y] = TileType.WALL;
                        hasCreatedWall = true;
                    }
                    if (tilemapHandler[x, y + 1] == TileType.EMPTY)
                    {
                        tilemap.SetTile(new Vector3Int(x, y + 1, 0), wall);
                        tilemapHandler[x, y + 1] = TileType.WALL;
                        hasCreatedWall = true;
                    }
                    if (tilemapHandler[x, y - 1] == TileType.EMPTY)
                    {
                        tilemap.SetTile(new Vector3Int(x, y - 1, 0), wall);
                        tilemapHandler[x, y - 1] = TileType.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(waitTime);
                    }
                }
            }
        }
    }
}
