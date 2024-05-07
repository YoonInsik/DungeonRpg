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
    public GameObject normalBulletPrefab; // �Ϲ� �Ѿ��� �������� ����

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
        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bulletObject.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Shoot(direction.normalized); // �Ѿ��� ������ ����ȭ�Ͽ� ����
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

