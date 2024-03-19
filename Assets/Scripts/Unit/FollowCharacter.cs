using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public GameObject player;
    public float speed = 1.0f; //0.01f
    private Vector3 pos;


    // Update is called once per frame
    void LateUpdate()
    {
        pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, speed);
    }
}
