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

        //인스턴스
        MeatMart meatmart;

        [Header("적 개체 설정")]
        [SerializeField] EnemyStat m_stat;

        [SerializeField] float now_hp;

        private void Start()
        {
            meatmart = MeatMart.instance;

            now_hp = m_stat.hp;

            now_burrow = true;

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
            if (now_burrow)
                return; 

            now_hp -= _damage;
            if (now_hp <= 0)
            {
                Dead();
                return;
            }
            GameObject ds = Instantiate(damagescale);
            ds.transform.position = transform.position;
            ds.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8f + Vector2.right * Random.Range(-5f, 5f), ForceMode2D.Impulse);
            ds.GetComponent<TextMeshPro>().text = _damage.ToString();
            ptc_damaged.Play();
        }

        /* 작동 안됨, 오브젝트가 비활성화되면서 꺼지는듯, 직접 파괴시킴
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
        */

        [Header("출몰")]
        public bool now_burrow;
        [SerializeField] ParticleSystem burrow_par;
        [SerializeField] GameObject burrow_image;
        [SerializeField] GameObject Model;

        public void Start_Burrowing()
        {
            burrow_par.Play();

            Invoke("Burrow_End", Random.Range(3f, 5f));
        }

        public void Dead_Resetting()
        {
            now_burrow = true;

            burrow_image.SetActive(true);
            Model.SetActive(false);
        }

        void Burrow_End()
        {
            now_burrow = false;

            burrow_par.Stop();
            burrow_image.SetActive(false);
            Model.SetActive(true);
        }


        [Header("죽음")]

        [Header("고기드랍")]
        [SerializeField] int meat_num;
        [Range(0, 100)]
        [SerializeField] float meatdrop_rate;


        void Dead()
        {
            //경험치 드랍
            var exp = ObjectPoolManager.Instance.GetGo("Exp");
            exp.transform.position = transform.position;

            //고기 드랍
            if (Random.Range(0, 100) < meatdrop_rate)
            {
                Instantiate(MapManager.Instance.CurChunk.data.dropTable[0].item, transform.position, Quaternion.identity);
            }

            // if (Random.Range(0, 100) < meatdrop_rate)
            // {
            //     if (meatmart.Meats[meat_num] == null)
            //         Debug.LogError("해당 고기는 존재하지 않습니다");
            //     else
            //         Instantiate(meatmart.Meats[meat_num], transform.position, Quaternion.identity);
            // }

            Instantiate(ptc_dead, transform.position, Quaternion.identity);

            Dead_Resetting();

            EnemySpawner.ReturnObject(this);
        
        }
    }
}