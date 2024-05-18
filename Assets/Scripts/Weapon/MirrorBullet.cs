using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MirrorBullet : WeaponBase
{
    Player player;
    public Vector3 direction;
    float speed = 10.0f;
    private IObjectPool<MirrorBullet> managedPool;
    private IObjectPool<Bullet> bulletPool;
    private bool isReleased = false;
    public GameObject normalBulletPrefab; // �Ϲ� �Ѿ��� �������� ����

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 30);
        baseDamage = 1.0f;
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
            managedPool.Release(this); // �Ѿ��� Ǯ�� ��ȯ
            isReleased = true; // �̹� ��ȯ�� ���·� ǥ��
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyMirrorBullet(); // �Ѿ� ��ȯ
            SpawnNormalBullets(); // �Ϲ� �Ѿ� ����
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    void SpawnNormalBullets()
    {
        // �÷��̾� �������� �Ϲ� �Ѿ� �߻�
        Vector3 reflectionDirection = (player.transform.position - transform.position).normalized;
        InstantiateAndShoot(normalBulletPrefab, reflectionDirection);

        // �翷���� �Ϲ� �Ѿ� �߰� ����
        Vector3 sideDirectionRight = Quaternion.Euler(0, 0, 15) * reflectionDirection;
        Vector3 sideDirectionLeft = Quaternion.Euler(0, 0, -15) * reflectionDirection;
        InstantiateAndShoot(normalBulletPrefab, sideDirectionRight);
        InstantiateAndShoot(normalBulletPrefab, sideDirectionLeft);
    }

    void InstantiateAndShoot(GameObject bulletPrefab, Vector3 direction)
    {
        //GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        //Bullet bulletComponent = bulletObject.GetComponent<Bullet>();
        var bullet = bulletPool.Get();
        bullet.transform.position = transform.position + direction.normalized;
        bullet.Shoot(direction.normalized);
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyMirrorBullet");
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(normalBulletPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(bulletPool);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}

