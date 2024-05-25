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
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bomb>(CreateBomb, OnGetBomb, OnReleaseBomb, OnDestroyBomb, maxSize: 12);
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

        for (int i = 0; i < count; i++)
        {
            var bomb = pool.Get();
            bomb.transform.parent = null;
            bomb.transform.localScale = bomb.transform.localScale * player.ATKRangeDelicacy();
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
