using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSword : WeaponBase
{
    public SwordState currentState = SwordState.Scan;

    private Vector3 direction;
    private Vector3 originalPosition;

    private void Awake()
    {
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

        switch (currentState)
        {
            case global::SwordState.Scan:
                HandleWaiting();
                HandleScanning();
                break;
            case global::SwordState.Attack:
                HandleAttack();
                break;
            case global::SwordState.Return:
                HandleReturning();
                break;
        }
    }

    void HandleScanning()
    {
        if (player.scanner.nearestTarget != null)
        {
            float distance = (player.scanner.nearestTarget.position - player.transform.position).magnitude;

            if (distance <= data.range)
            {
                // ���� ��ġ ���
                currentState = global::SwordState.Attack;
            }
        }
    }

    void HandleWaiting()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;

            if (player.scanner.nearestTarget != null)
            {
                direction = (player.scanner.nearestTarget.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    void HandleAttack()
    {
        transform.parent = null;

        originalPosition = player.transform.position + offset;
        Vector3 attackPosition = originalPosition + direction * data.range;

        // ���� ���� �̵�
        transform.position = Vector3.MoveTowards(transform.position, attackPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
        {
            currentState = global::SwordState.Return; // ���� ��ġ�� ���ư���
        }
    }

    void HandleReturning()
    {
        transform.parent = player.transform;

        originalPosition = player.transform.position + offset;

        // �÷��̾� ���� ���� ��ġ�� ���ư���
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // ���� ��ġ�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            currentState = global::SwordState.Scan;
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
