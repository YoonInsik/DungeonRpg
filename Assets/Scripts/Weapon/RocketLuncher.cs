using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RocketLuncher : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float interval = 1.0f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<Rocket> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Rocket>(CreateRocket, OnGetRocket, OnReleaseRocket, OnDestroyRocket, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > interval)
        {
            elapsedTime = 0.0f;
            Fire();
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized; // 목표 방향 정규화
        var rocket = pool.Get();
        rocket.transform.position = transform.position; // 로켓 위치 설정
        rocket.Shoot(dir); // 로켓에 방향 전달
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
