using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseUnit
{
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow)) ||
            (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            var horizontalValue = Input.GetAxisRaw("Horizontal");
            //var verticalValue = Input.GetAxisRaw("Vertical");

            Vector3 newPos = transform.position;
            newPos.x += horizontalValue;
            //newPos.y += verticalValue;

            transform.position = newPos;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow)) ||
                 (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            //var horizontalValue = Input.GetAxisRaw("Horizontal");
            var verticalValue = Input.GetAxisRaw("Vertical");

            Vector3 newPos = transform.position;
            //newPos.x += horizontalValue;
            newPos.y += verticalValue;

            transform.position = newPos;
        }
    }
}
