using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS
{
    public class EnemyAI_Boss_A : MonoBehaviour
    {
        //����
        EnemyStat m_stat;
        Transform player_trns;
        Rigidbody2D m_rigid;
        Enemy m_enemy;
        Animator m_ani;

        // Start is called before the first frame update
        void Start()
        {
            m_enemy = GetComponent<Enemy>();
            m_stat = GetComponent<Enemy>().Get_MyStat();
            m_rigid = GetComponent<Rigidbody2D>();
            player_trns = GameObject.FindGameObjectWithTag("Player").transform;
            m_ani = GetComponent<Animator>();

            Set_Sequence();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_enemy.now_burrow)
                return;

            if (now_attack)
                return;


            Move();
        }


        //�÷��̾�� ���ϴ� ����
        Vector2 Direction()
        {
            Vector2 dir = player_trns.position - transform.position ;

            return dir.normalized;
        }

        // ������
        void Move()
        {
            m_rigid.velocity = m_enemy.now_speed * Direction();

            remain = Vector2.Distance(player_trns.position, transform.position);

            switch (Sequence)
            {
                default: Debug.LogWarning("�Ҵ�� �������� ���� �ٽ� �̽��ϴ�."); Set_Sequence(); return;

                case 1: //�����

                    if (remain < Attack1_length)
                    {
                        now_attack = true;
                        m_rigid.velocity = Vector2.zero;
                        m_rigid.bodyType = RigidbodyType2D.Static;
                        Instantiate(smash_warning, transform.position, Quaternion.identity);
                        GroundSmash();
                    }

                    break;

                case 2: //��Ʋ���߻�

                    now_attack = true;
                    m_rigid.velocity = Vector2.zero;
                    m_rigid.bodyType = RigidbodyType2D.Static;
                    StartCoroutine(Shoot_Gatling());

                    break;

                case 3: //ź���߻�

                    now_attack = true;
                    m_rigid.velocity = Vector2.zero;
                    m_rigid.bodyType = RigidbodyType2D.Static;

                    if(Random.Range(0,10) < 5)
                        StartCoroutine(Shoot_Zipsok(true));
                    else
                        StartCoroutine(Shoot_Zipsok(false));

                    break;
            }
        }

        //�÷��̾�� ������ �Ÿ�
        [SerializeField] float remain;
        [SerializeField] bool now_attack;

        [Header("������")]
        [SerializeField] int Sequence;

        void Set_Sequence()
        {
            Sequence = Random.Range(1, 4);
        }

        #region 1 : �����

        [Header("�����")]
        [SerializeField] float Attack1_length;
        [SerializeField] GameObject smash_warning;
        [SerializeField] GameObject smash_effect;

        //�׶��� ���Ž�
        void GroundSmash()
        {
            m_ani.SetTrigger("groundsmash");
        }

        void Ani_GroundSmash_Effect()
        {
            Instantiate(smash_effect, transform.position, Quaternion.identity).GetComponent<EnemyAttackField>().m_enemy = m_enemy;

        }

        void Ani_GroundSmash_End()
        {
            now_attack = false;

            m_rigid.bodyType = RigidbodyType2D.Dynamic;

            Set_Sequence();
        }

        #endregion

        #region 2 : ��Ʋ���߻�

        [Header("��Ʋ���߻�")]
        [SerializeField] GameObject bullet_2;
        [SerializeField] float bullet_2_speed;
        [SerializeField] int bullet_2_count;
        [SerializeField] float bullet_2_cool = 0.15f;

        IEnumerator Shoot_Gatling()
        {
            int a = bullet_2_count;

            while (a > 0)
            {
                GameObject b = Instantiate(bullet_2, transform);
                b.transform.position = transform.position;
                b.GetComponent<EnemyAttackField>().m_enemy = m_enemy;
                b.GetComponent<Rigidbody2D>().velocity = Direction() * bullet_2_speed;

                a--;

                yield return new WaitForSeconds(bullet_2_cool);
            }

            yield return new WaitForSeconds(2f);

            now_attack = false;

            m_rigid.bodyType = RigidbodyType2D.Dynamic;

            Set_Sequence();

        }

        #endregion

        #region 2 : ź���߻�

        [Header("ź���߻�")]
        [SerializeField] GameObject bullet_3;
        [SerializeField] float bullet_3_speed;
        [SerializeField] float bullet_3_cool = 0.15f;

        IEnumerator Shoot_Zipsok(bool up)
        {

            if (up)
            {
                int a = 0;

                while (a < 30)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject b = Instantiate(bullet_3, transform);

                        b.transform.position = transform.position;

                        b.GetComponent<EnemyAttackField>().m_enemy = m_enemy;
                        switch (i)
                        {
                            case 0:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (30f - a) / 30f + Vector2.up * a / 30f).normalized * bullet_3_speed;
                                break;

                            case 1:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.up * (30f - a) / 30f + Vector2.right * a / 30f) * bullet_3_speed;
                                break;

                            case 2:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.right * (30f - a) / 30f + Vector2.down * a / 30f) * bullet_3_speed;
                                break;

                            case 3:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.down * (30f - a) / 30f + Vector2.left * a / 30f) * bullet_3_speed;
                                break;

                        }

                        b.transform.parent = null;
                    }

                    a++;

                    yield return new WaitForSeconds(bullet_3_cool);
                }
            }
            else
            {
                int a = 30;

                while (a > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject b = Instantiate(bullet_3, transform);

                        b.transform.position = transform.position;

                        b.GetComponent<EnemyAttackField>().m_enemy = m_enemy;
                        switch (i)
                        {
                            case 0:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.left * (30f - a) / 30f + Vector2.up * a / 30f).normalized * bullet_3_speed;
                                break;

                            case 1:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.up * (30f - a) / 30f + Vector2.right * a / 30f) * bullet_3_speed;
                                break;

                            case 2:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.right * (30f - a) / 30f + Vector2.down * a / 30f) * bullet_3_speed;
                                break;

                            case 3:
                                b.GetComponent<Rigidbody2D>().velocity = (Vector2.down * (30f - a) / 30f + Vector2.left * a / 30f) * bullet_3_speed;
                                break;

                        }

                        b.transform.parent = null;
                    }

                    a--;

                    yield return new WaitForSeconds(bullet_3_cool);
                }
            }
            yield return new WaitForSeconds(2f);

            now_attack = false;

            m_rigid.bodyType = RigidbodyType2D.Dynamic;

            Set_Sequence();

        }

        #endregion
    }

}