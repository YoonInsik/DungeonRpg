using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform player;
    public float speed = 1.0f; //0.01f
    private Vector3 pos;

    void Start()
    {
        player = UnitManager.Instance.player.transform;
        if (player == null) Debug.Log("error");
    }

    private void LateUpdate()
    {
        pos = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }
}
