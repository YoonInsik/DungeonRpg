using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public class Sword : MonoBehaviour
{
    public enum SwordState
    {
        Wait,
        Attack
    }

    public SwordState currentState = SwordState.Wait;

    public Player player; // Player 타입으로 player 변수 선언
    public Vector3 offset = new Vector3(1, 0, 0); // 플레이어로부터의 상대적 위치

    void Start()
    {
        // 플레이어 게임 오브젝트를 태그를 통해 찾고, Player 컴포넌트를 가져옵니다.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }

    }

    void Update()
    {
        switch (currentState)
        {
            case SwordState.Wait:
                HandleWaiting();
                break;
            case SwordState.Attack:
                HandleAttack();
                break;
        }
    }

    void HandleWaiting()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;

            Vector3 direction = player.scanner.nearestTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }
    }

    void HandleAttack()
    {

    }
}
