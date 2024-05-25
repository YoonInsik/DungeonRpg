using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RocketLuncher : WeaponBase
{
    public GameObject rocketPrefab;

    private IObjectPool<Rocket> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Rocket>(CreateRocket, OnGetRocket, OnReleaseRocket, OnDestroyRocket, maxSize: 30);
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

        var rocket = pool.Get();
        rocket.player = player;
        rocket.attackScale = rocket.transform.localScale;
        rocket.transform.position = transform.position + direction.normalized; // ���� ��ġ ����
        rocket.transform.localScale = rocket.transform.localScale * player.ATKRangeDelicacy();
        rocket.Shoot(direction, CalculateDamage(), data.speed);

        elapsedTime = 0.0f;
    }

    private Rocket CreateRocket()
    {
        Rocket rocket = Instantiate(rocketPrefab).GetComponent<Rocket>();
        rocket.SetManagedPool(pool);
        return rocket;
    }

    private void OnGetRocket(Rocket rocket)
    {
        rocket.gameObject.SetActive(true);
    }

    private void OnReleaseRocket(Rocket rocket)
    {
        rocket.gameObject.SetActive(false);
    }

    private void OnDestroyRocket(Rocket rocket)
    {
        Destroy(rocket.gameObject);
    }
}
