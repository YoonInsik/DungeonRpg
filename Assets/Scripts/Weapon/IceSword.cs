using SHS;
using System.Collections;
using UnityEngine;

public class IceSword : WeaponBase
{
    public SwordState currentState = SwordState.Scan;

    public Vector3 offset = new Vector3(0, 2, 0); // 플레이어로부터의 상대적 위치

    private Vector3 direction;
    private Vector3 originalPosition;

    private void Awake()
    {
        attackScale = transform.localScale;
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

        // 공격 위치 계산
        originalPosition = player.transform.position + offset;
        Vector3 attackPosition = originalPosition + direction * data.range;

        // 적을 향해 이동
        transform.position = Vector3.MoveTowards(transform.position, attackPosition, data.speed * Time.deltaTime * player.ATKSpeedDelicacy());

        // 적에게 도달했는지 확인
        if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
        {
            currentState = global::SwordState.Return; // 원래 위치로 돌아가기
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
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);

            // 적의 속도 감소
            StartCoroutine(ReduceEnemySpeed(collision, 0.5f, 2.0f)); // 속도를 50%로 줄이고, 2초 동안 지속
        }
    }

    private IEnumerator ReduceEnemySpeed(Collider2D enemyCollider, float reductionFactor, float duration)
    {
        Enemy enemyStat = enemyCollider.GetComponent<Enemy>();
        if (enemyStat.speedMultiply != 0)
        {
            enemyStat.speedMultiply = reductionFactor; // 적의 속도 감소
            yield return new WaitForSeconds(duration);
            enemyStat.speedMultiply = 1; // 원래 속도로 복원
        }
    }
}
