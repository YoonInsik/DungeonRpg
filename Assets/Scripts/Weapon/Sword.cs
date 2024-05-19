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

    public Player player; // Player Ÿ������ player ���� ����
    public Vector3 offset = new Vector3(1, 0, 0); // �÷��̾�κ����� ����� ��ġ

    float speed = 10.0f;
    float attackDistance = 25.0f;
    private Vector3 attackPosition;
    private bool isReturning = false;

    private void Awake()
    {
        attackScale = transform.localScale;
        baseDamage = 10.0f;
    }
    void Start()
    {
        // �÷��̾� ���� ������Ʈ�� �±׸� ���� ã��, Player ������Ʈ�� �����ɴϴ�.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }

    }

    void Update()
    {
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        if (player.scanner.nearestTarget != null)
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
                // ���� ��ġ ���
                attackPosition = player.scanner.nearestTarget.position;
                // ���� ���� �̵�
                transform.position = Vector3.MoveTowards(transform.position, attackPosition, speed * Time.deltaTime * player.ATKSpeedDelicacy());

                // ������ �����ߴ��� Ȯ��
                if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
                {
                    isReturning = true; // ���� ��ġ�� ���ư���
                }
            }
            else
            {
                // �÷��̾� ���� ���� ��ġ�� ���ư���
                Vector3 originalPosition = player.transform.position + offset;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime * player.ATKSpeedDelicacy());

                // ���� ��ġ�� �����ߴ��� Ȯ��
                if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
                {
                    isReturning = false; // ���� ����
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
