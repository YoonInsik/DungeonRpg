using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData_", menuName = "SO/ChunkData")]
public class ChunkData : ScriptableObject
{
    public int[,] tileDatas = new int[MapManager.CHUNKSIZE, MapManager.CHUNKSIZE];
}
