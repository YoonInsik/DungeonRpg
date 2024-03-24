using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    //���� �⺻������ ������ ���� ��.
    public class Enemy : MonoBehaviour
    {

        [Header("�� ��ü ����")]
        [SerializeField] EnemyStat m_stat;

        [SerializeField] float now_hp;

        private void Start()
        {
            now_hp = m_stat.hp;
        }

        public EnemyStat Get_MyStat()
        {
            return m_stat;
        }

        [Header("����Ʈ")]
        [SerializeField] ParticleSystem ptc_damaged;
        [SerializeField] ParticleSystem ptc_dead;

        public void Damaged(float _damage)
        {
            now_hp -= _damage;
            if (now_hp <= 0)
            {
                Dead();
                return;
            }
            ptc_damaged.Play();
        }

        void Dead()
        {
            ptc_dead.Play();
            EnemySpawner.ReturnObject(this);
        }
    }
}