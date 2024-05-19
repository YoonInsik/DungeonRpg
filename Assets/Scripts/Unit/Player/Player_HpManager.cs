using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_HpManager : MonoBehaviour
{
    public static Player_HpManager instance;

    Player m_player;

    [Header("수치확인용")]
    [SerializeField] int check_maxhp;
    [SerializeField] int check_nowhp;

    [Header("연결")]
    [SerializeField] Player_SpriteColorControl player_scc;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        m_player = GetComponent<Player>();

        check_maxhp = check_nowhp = m_player.MaxHP;
    }
    /*
    public int Get_NowHp()
    {
        return NowHp;
    }
    */

    //회복
    public void Recovery(int _heal)
    {
        m_player.HP += _heal;

        m_player.HP = Mathf.Clamp(m_player.HP, 0, m_player.MaxHP);

        check_nowhp = m_player.HP;
        check_maxhp = m_player.MaxHP;
    }

    [Header("피해")]
    [SerializeField] GameObject damagescale;
    [SerializeField] AudioSource[] damamged_sound;

    //피해
    public void Damaged(int _damage)
    {
        if (m_player.HP <= 0)
        {
            return;
        }

        m_player.HP -= _damage;

        GameObject ds = Instantiate(damagescale);
        ds.transform.position = transform.position + Vector3.up * 1f;
        ds.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8f + Vector2.right * Random.Range(-5f, 5f), ForceMode2D.Impulse);
        ds.GetComponent<TextMeshPro>().color = Color.red;
        ds.GetComponent<TextMeshPro>().text = _damage.ToString();

        damamged_sound[Random.Range(0, damamged_sound.Length)].Play();
        player_scc.Damaged_Red();

        check_nowhp = m_player.HP;
        check_maxhp = m_player.MaxHP;

        if (m_player.HP <= 0)
        {
            Dead();
        }
    }

    //죽음처리
    void Dead()
    {
        GameOverPanel.instance.Object_On(false);
    }

}
