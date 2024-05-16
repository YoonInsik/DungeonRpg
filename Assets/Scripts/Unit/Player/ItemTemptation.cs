using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemptation: MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float range;
    public Collider2D[] exp;


    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        exp = Physics2D.OverlapCircleAll(player.transform.position, range);
        foreach(Collider2D col in exp)
        {
            if (col.CompareTag("Exp")){
                float distance = Vector2.Distance(player.transform.position, col.transform.position);
                if(distance < range)
                {
                    col.transform.position = Vector2.MoveTowards(col.transform.position, player.transform.position, speed * Time.deltaTime);
                }
            }
        }
    }
}
