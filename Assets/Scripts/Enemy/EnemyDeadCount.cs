using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadCount : MonoBehaviour
{
    public static EnemyDeadCount instance;

    private void Awake()
    {
        instance = this;
    }
    
    [SerializeField] int dead_count;

    public void Counting()
    {
        dead_count++;
    }

    public int Use_Counting()
    {
        int save_count = dead_count;
        dead_count = 0;

        return save_count;
    }
}
