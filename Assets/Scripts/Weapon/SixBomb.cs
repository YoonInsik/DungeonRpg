using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class SixBomb : WeaponBase
{
    public GameObject bombPrefab;
    int count = 6;

    private IObjectPool<Bomb> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bomb>(CreateBomb, OnGetBomb, OnReleaseBomb, OnDestroyBomb, maxSize: 12);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > data.interval)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 range = targetPos - transform.position;

        if (range.magnitude > data.range)
            return;

        for (int i = 0; i < count; i++)
        {
            var bomb = pool.Get();
            bomb.transform.parent = null;
            bomb.transform.position = transform.position;

            float angle = 180 * i / count;
            bomb.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 dir = bomb.transform.up;
            bomb.Shoot(dir, CalculateDamage(), data.speed);
        }
            
        elapsedTime = 0.0f;
    }

    private Bomb CreateBomb()
    {
        Bomb bomb = Instantiate(bombPrefab).GetComponent<Bomb>();
        bomb.SetManagedPool(pool);
        return bomb;
    }

    private void OnGetBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
    }

    private void OnReleaseBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }

    private void OnDestroyBomb(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }
}
