using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_HpManager : MonoBehaviour
{
    public static Player_HpManager instance;

    [Header("����")]
    [SerializeField] Player_SpriteColorControl player_scc;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        NowHp = MaxHp = GetComponent<Player>().GetBaseHP();


    }


    [Header("ü�� - Ȯ�ο�")]
    [SerializeField] int MaxHp;
    [SerializeField] int NowHp;

    public int Get_NowHp()
    {
        return NowHp;
    }

    //ȸ��
    public void Recovery(int _heal)
    {
        NowHp += _heal;

        NowHp = Mathf.Clamp(NowHp, 0, MaxHp);
    }

    [Header("����")]
    [SerializeField] GameObject damagescale;
    [SerializeField] AudioSource[] damamged_sound;

    //����
    public void Damaged(int _damage)
    {
        NowHp -= _damage;

        GameObject ds = Instantiate(damagescale);
        ds.transform.position = transform.position + Vector3.up * 1f;
        ds.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 8f + Vector2.right * Random.Range(-5f, 5f), ForceMode2D.Impulse);
        ds.GetComponent<TextMeshPro>().color = Color.red;
        ds.GetComponent<TextMeshPro>().text = _damage.ToString();

        damamged_sound[Random.Range(0, damamged_sound.Length)].Play();
        player_scc.Damaged_Red();

        if (NowHp <= 0)
        {
            Dead();
        }
    }

    //����ó��
    void Dead()
    {

    }

}
