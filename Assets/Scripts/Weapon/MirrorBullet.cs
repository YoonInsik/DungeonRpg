using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MirrorBullet : MonoBehaviour
{
    Player player;
    public Vector3 direction;
    float speed = 10.0f;
    float damage = 1.0f;
    private IObjectPool<MirrorBullet> managedPool;
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        isReleased = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.fixedDeltaTime * speed);
    }

    public void SetManagedPool(IObjectPool<MirrorBullet> Pool)
    {
        managedPool = Pool;
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        Invoke("DestroyMirrorBullet", 5f);
    }

    public void DestroyMirrorBullet()
    {
        if (!isReleased)
        {
            managedPool.Release(this);
            isReleased = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Enemy"))
    {
        collision.GetComponent<Enemy>().Damaged(damage);

        // 플레이어 방향으로 반사
        Vector3 reflectionDirection = (player.transform.position - transform.position).normalized;
        direction = reflectionDirection;
        Shoot(reflectionDirection);

        // 양옆으로 총알 추가 생성
        Vector3 sideDirectionRight = Quaternion.Euler(0, 0, 15) * reflectionDirection; // 오른쪽으로 15도
        Vector3 sideDirectionLeft = Quaternion.Euler(0, 0, -15) * reflectionDirection; // 왼쪽으로 15도

        CreateAdditionalBullet(sideDirectionRight);
        CreateAdditionalBullet(sideDirectionLeft);

        Invoke("DestroyMirrorBullet", 5f); // 반사된 총알이 일정 시간 후에 사라지도록 설정
    }
}

void CreateAdditionalBullet(Vector3 direction)
{
    MirrorBullet newBullet = managedPool.Get();
    newBullet.transform.position = transform.position;
    newBullet.Shoot(direction);
}

    private void OnDisable()
    {
        CancelInvoke("DestroyMirrorBullet");
    }
}

