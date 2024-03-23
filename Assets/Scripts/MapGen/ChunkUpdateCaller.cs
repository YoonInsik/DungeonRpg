using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkUpdateCaller : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            MapManager.Instance.ReloadChunks();
        }
    }
}
