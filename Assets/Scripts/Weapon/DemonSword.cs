using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSword : WeaponBase
{
    public enum DemonSwordState
    {
        Wait,
        Attack
    }

    public DemonSwordState currentState = DemonSwordState.Wait;

    public Player player; // Player Ÿ������ player ���� ����
    public Vector3 offset = new Vector3(-1, 1, 0); // �÷��̾�κ����� ����� ��ġ

    float speed = 10.0f;
    float attackDistance = 25.0f;
    private Vector3 attackPosition;
    private bool isReturning = false;

    private void Awake()
    {
        baseDamage = 7.0f;
        attackScale = transform.localScale;
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

            if (distanceSqr <= attackDistance)
            {
                currentState = DemonSwordState.Attack;
            }
            else
            {
                currentState = DemonSwordState.Wait;
            }
        }
        switch (currentState)
        {
            case DemonSwordState.Wait:
                HandleWaiting();
                break;
            case DemonSwordState.Attack:
                HandleAttack();
                break;
        }
    }

    void HandleWaiting()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;

            if (player.scanner.nearestTarget != null)
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
                    currentState = DemonSwordState.Wait;
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
            player.HP += Mathf.RoundToInt(damage * 0.1f);
        }
    }
}
