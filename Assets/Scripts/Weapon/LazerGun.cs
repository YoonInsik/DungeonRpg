using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LazerGun : WeaponBase
{
    public GameObject lazerPrefab;

    private IObjectPool<Lazer> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Lazer>(CreateLazer, OnGetLazer, OnReleaseLazer, OnDestroyLazer, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > data.interval * player.ATKCooldownDelicacy())
        {
            Fire();
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized; // ��ǥ ���� ����ȭ

        if (dir.magnitude > data.range)
            return;

        var lazer = pool.Get();
        lazer.transform.position = transform.position; // ���� ��ġ ����
        lazer.Shoot(dir.normalized, CalculateDamage(), data.speed);

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
