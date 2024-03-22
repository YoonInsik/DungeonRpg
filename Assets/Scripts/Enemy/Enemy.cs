using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    //적의 기본설정을 저장해 놓는 곳.
    public class Enemy : MonoBehaviour
    {

        [Header("적 개체 설정")]
        [SerializeField] EnemyStat m_stat;

        public EnemyStat Get_MyStat()
        {
            return m_stat;
        }

    }
}