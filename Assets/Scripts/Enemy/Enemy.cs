using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    //���� �⺻������ ������ ���� ��.
    public class Enemy : MonoBehaviour
    {

        [Header("�� ��ü ����")]
        [SerializeField] EnemyStat m_stat;

        public EnemyStat Get_MyStat()
        {
            return m_stat;
        }

    }
}