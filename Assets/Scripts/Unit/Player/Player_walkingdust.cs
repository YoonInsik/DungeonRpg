using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_walkingdust : MonoBehaviour
{
    [Header("��ƼŬ")]
    [SerializeField] ParticleSystem m_partic;

    [Header("����")]
    [SerializeField] float set_length;
    [SerializeField] float set_time;

    [Header("Ȯ�ο�")]
    [SerializeField] Vector2 save_pos;

   

    // Start is called before the first frame update
    void Start()
    {
        save_pos = transform.position;

        StartCoroutine(Walking());
    }

    IEnumerator Walking()
    {
        while (true)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), save_pos) > set_length )
            {
                save_pos = new Vector2(transform.position.x, transform.position.y);
                m_partic.Play();
            }

            yield return new WaitForSeconds(set_time);
        }
    }
}
