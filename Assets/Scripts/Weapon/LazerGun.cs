using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LazerGun : WeaponBase
{
    public GameObject lazerPrefab;

    private IObjectPool<Lazer> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Lazer>(CreateLazer, OnGetLazer, OnReleaseLazer, OnDestroyLazer, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.position = player.transform.position + offset;

        if (player?.scanner?.nearestTarget == null) return;
        direction = player.scanner.nearestTarget.position - transform.position;

        if (direction.magnitude <= data.range)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Fire();
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void Fire()
    {
        if (elapsedTime < data.interval * player.ATKCooldownDelicacy()) return;

        var lazer = pool.Get();
        lazer.transform.position = transform.position; // ���� ��ġ ����
        lazer.transform.localScale = lazer.transform.localScale * player.ATKRangeDelicacy();
        lazer.Shoot(direction.normalized, CalculateDamage(), data.speed);

        elapsedTime = 0.0f;
    }

    private Lazer CreateLazer()
    {
        Lazer lazer = Instantiate(lazerPrefab).GetComponent<Lazer>();
        lazer.SetManagedPool(pool);
        return lazer;
    }

    private void OnGetLazer(Lazer lazer)
    {
        lazer.gameObject.SetActive(true);
    }

    private void OnReleaseLazer(Lazer lazer)
    {
        lazer.gameObject.SetActive(false);
    }

    private void OnDestroyLazer(Lazer lazer)
    {
        Destroy(lazer.gameObject);
    }
}
