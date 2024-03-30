using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace SHS
{
    //���� �⺻������ ������ ���� ��.
    public class Enemy : MonoBehaviour
    {

        //�ν��Ͻ�
        MeatMart meatmart;

        [Header("�� ��ü ����")]
        [SerializeField] EnemyStat m_stat;

        [SerializeField] float now_hp;

        private void Start()
        {
            meatmart = MeatMart.instance;

            now_hp = m_stat.hp;
        }

        public EnemyStat Get_MyStat()
        {
            return m_stat;
        }

        [Header("����Ʈ")]
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

        [Header("����")]

        [Header("�����")]
        [SerializeField] int meat_num;
        [Range(0, 100)]
        [SerializeField] float meatdrop_rate;


        void Dead()
        {
            //��� ���
            if (Random.Range(0, 100) < meatdrop_rate)
            {
                if (meatmart.Meats[meat_num] == null)
                    Debug.LogError("�ش� ���� �������� �ʽ��ϴ�");
                else
                    Instantiate(meatmart.Meats[meat_num], transform.position, Quaternion.identity);
            }

            Instantiate(ptc_dead, transform.position, Quaternion.identity);
            EnemySpawner.ReturnObject(this);
        }
    }
}