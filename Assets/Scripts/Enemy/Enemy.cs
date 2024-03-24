using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    //적의 기본설정을 저장해 놓는 곳.
    public class Enemy : MonoBehaviour
    {

        [Header("적 개체 설정")]
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

        [Header("이펙트")]
        [SerializeField] ParticleSystem ptc_damaged;
        [SerializeField] ParticleSystem ptc_dead;
        [SerializeField] GameObject damagescale;

        public void Damaged(float _damage)
        {
            now_hp -= _damage;
            if (now_hp <= 0)
            {
                Dead();
                return;
            }
            GameObject ds = Instantiate(damagescale, transform);
            ds.GetComponent<TextMeshPro>().text = _damage.ToString();
            Destroy_DamageScale(ds);
            ptc_damaged.Play();
        }

        IEnumerator Destroy_DamageScale(GameObject _obj)
        {
            int a = 0;

            while(a < 1)
            {
                a ++;
                yield return new WaitForSeconds(1f);
            }

            Destroy(_obj);
        }

        void Dead()
        {
            Instantiate(ptc_dead, transform.position, Quaternion.identity);
            EnemySpawner.ReturnObject(this);
        }
    }
}