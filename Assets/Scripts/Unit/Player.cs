using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseUnit
{
    public Vector3 newPos;
    public float speed = 3.0f;
    void Update()
    {
        newPos.x = Input.GetAxisRaw("Horizontal");
        newPos.y = Input.GetAxisRaw("Vertical");

        transform.position += newPos.normalized * speed * Time.deltaTime;
    }
}
