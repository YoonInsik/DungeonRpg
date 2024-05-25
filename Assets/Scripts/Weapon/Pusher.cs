using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : WeaponBase
{
    public SwordState currentState = SwordState.Scan;

    float knockbackForce = 5.0f; // �о�� ��
    private Vector3 direction;
    private Vector3 originalPosition;

    private void Awake()
    {
        attackScale = transform.localScale;
    }
    void Start()
    {
        player = GetComponentInParent<Player>();

    }

    void Update()
    {
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
                GetComponent<Collider2D>().enabled = true;
                currentState = SwordState.Attack;
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

        // 공격 위치 계산
        originalPosition = player.transform.position + offset;
        Vector3 attackPosition = originalPosition + direction * data.range;

        // 적을 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, attackPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // 적에게 도달했는지 확인
        if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
        {
            currentState = SwordState.Return; // 원래 위치로 돌아가기
        }
    }

    void HandleReturning()
    {
        transform.parent = player.transform;

        originalPosition = player.transform.position + offset;

        // 플레이어 기준 원래 위치로 돌아가기
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // 원래 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            currentState = SwordState.Scan;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ������ ������ ����
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);

            // ���� �о��
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Force);
            }
        }
    }
}