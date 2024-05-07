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
    public GameObject normalBulletPrefab; // 일반 총알의 프리팹을 참조

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
            managedPool.Release(this); // 총알을 풀로 반환
            isReleased = true; // 이미 반환된 상태로 표시
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Enemy"))
    {
        DestroyMirrorBullet(); // 총알 반환
        SpawnNormalBullets(); // 일반 총알 생성
        collision.GetComponent<Enemy>().Damaged(damage);
    }
}

    void SpawnNormalBullets()
    {
        // 플레이어 방향으로 일반 총알 발사
        Vector3 reflectionDirection = (player.transform.position - transform.position).normalized;
        InstantiateAndShoot(normalBulletPrefab, reflectionDirection);

        // 양옆으로 일반 총알 추가 생성
        Vector3 sideDirectionRight = Quaternion.Euler(0, 0, 15) * reflectionDirection;
        Vector3 sideDirectionLeft = Quaternion.Euler(0, 0, -15) * reflectionDirection;
        InstantiateAndShoot(normalBulletPrefab, sideDirectionRight);
        InstantiateAndShoot(normalBulletPrefab, sideDirectionLeft);
    }

    void InstantiateAndShoot(GameObject bulletPrefab, Vector3 direction)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bulletObject.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Shoot(direction.normalized); // 총알의 방향을 정규화하여 전달
        }
        else
        {
            Debug.LogError("Bullet component not found on the instantiated object.");
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyMirrorBullet");
    }
}

