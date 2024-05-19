using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSword : WeaponBase
{
    public enum IceSwordState
    {
        Wait,
        Attack
    }

    public IceSwordState currentState = IceSwordState.Wait;

    public Player player; // Player Ÿ������ player ���� ����
    public Vector3 offset = new Vector3(0, 2, 0); // �÷��̾�κ����� ����� ��ġ

    float speed = 10.0f;
    float attackDistance = 25.0f;
    private Vector3 attackPosition;
    private bool isReturning = false;

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
        if (player.scanner.nearestTarget != null)
        {
            float distanceSqr = (player.scanner.nearestTarget.position - transform.position).sqrMagnitude;

            if (distanceSqr <= attackDistance)
            {
                currentState = IceSwordState.Attack;
            }
            else
            {
                currentState = IceSwordState.Wait;
            }
        }
        switch (currentState)
        {
            case IceSwordState.Wait:
                HandleWaiting();
                break;
            case IceSwordState.Attack:
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
                    currentState = IceSwordState.Wait;
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

            // ���� �ӵ� ����
            StartCoroutine(ReduceEnemySpeed(collision, 0.5f, 2.0f)); // �ӵ��� 50%�� ���̰�, 2�� ���� ����
        }
    }

    private IEnumerator ReduceEnemySpeed(Collider2D enemyCollider, float reductionFactor, float duration)
    {
        EnemyStat enemyStat = enemyCollider.GetComponent<Enemy>().Get_MyStat();
        float originalSpeed = enemyStat.speed;
        enemyStat.speed *= reductionFactor; // ���� �ӵ� ����
        yield return new WaitForSeconds(duration);
        enemyStat.speed = originalSpeed; // ���� �ӵ��� ����
    }
}