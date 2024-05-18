using SHS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public class Sword : WeaponBase
{
    public enum SwordState
    {
        Wait,
        Attack
    }

    public SwordState currentState = SwordState.Wait;

    public Player player; // Player 타입으로 player 변수 선언
    public Vector3 offset = new Vector3(1, 0, 0); // 플레이어로부터의 상대적 위치

    float speed = 10.0f;
    float attackDistance = 25.0f;
    private Vector3 attackPosition;
    private bool isReturning = false;

    private void Awake()
    {
        baseDamage = 10.0f;
    }
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
        if(player.scanner.nearestTarget != null)
        {
            float distanceSqr = (player.scanner.nearestTarget.position - transform.position).sqrMagnitude;
            
            if(distanceSqr <= attackDistance)
            {
                currentState = SwordState.Attack;
            }
            else
            {
                currentState = SwordState.Wait;
            }
        }
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

            if(player.scanner.nearestTarget != null)
            {
                Vector3 direction = player.scanner.nearestTarget.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            }
        }
    }

    void HandleAttack()
    {
        if (player != null && player.scanner.nearestTarget != null)
        {
            if (!isReturning)
            {
                // 공격 위치 계산
                attackPosition = player.scanner.nearestTarget.position;
                // 적을 향해 이동
                transform.position = Vector3.MoveTowards(transform.position, attackPosition, speed * Time.deltaTime);

                // 적에게 도달했는지 확인
                if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
                {
                    isReturning = true; // 원래 위치로 돌아가기
                }
            }
            else
            {
                // 플레이어 기준 원래 위치로 돌아가기
                Vector3 originalPosition = player.transform.position + offset;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

                // 원래 위치에 도달했는지 확인
                if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
                {
                    isReturning = false; // 공격 종료
                    currentState = SwordState.Wait;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }
}
