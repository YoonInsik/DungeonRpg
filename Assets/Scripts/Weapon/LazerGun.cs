using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LazerGun : MonoBehaviour
{
    public GameObject lazerPrefab;
    public float interval = 0.2f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<Lazer> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Lazer>(CreateLazer, OnGetLazer, OnReleaseLazer, OnDestroyLazer, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > interval * player.ATKCooldownDelicacy())
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
        var lazer = pool.Get();
        lazer.transform.position = transform.position; // 로켓 위치 설정
        lazer.Shoot(dir); // 로켓에 방향 전달
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
