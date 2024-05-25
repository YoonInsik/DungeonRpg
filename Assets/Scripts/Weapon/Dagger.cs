using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : WeaponBase
{
    public SwordState currentState = SwordState.Scan;

    private Vector3 direction;
    private Vector3 originalPosition;
    private Collider2D col;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        attackScale = transform.localScale;
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.localScale = attackScale * player.ATKRangeDelicacy();

        switch (currentState)
        {
            case SwordState.Scan:
                HandleWaiting();
                HandleScanning();
                break;
            case SwordState.Attack:
                HandleAttack();
                break;
            case SwordState.Return:
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
                if (elapsedTime < data.interval * player.ATKCooldownDelicacy()) return;

                currentState = SwordState.Attack;
                elapsedTime = 0.0f;
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
        col.enabled = true;

        originalPosition = player.transform.position + offset;
        Vector3 attackPosition = originalPosition + direction * data.range;

        // ���� ���� �̵�
        transform.position = Vector3.MoveTowards(transform.position, attackPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
        {
            currentState = SwordState.Return; // ���� ��ġ�� ���ư���
        }
    }

    void HandleReturning()
    {
        transform.parent = player.transform;
        col.enabled = false;

        originalPosition = player.transform.position + offset;

        // �÷��̾� ���� ���� ��ġ�� ���ư���
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // ���� ��ġ�� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            currentState = SwordState.Scan;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damaged(CalculateDamage());
        }
    }
}
