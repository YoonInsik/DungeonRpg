using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DemonGun : MonoBehaviour
{
    public GameObject demonBulletPrefab;
    public float interval = 0.2f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<DemonBullet> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<DemonBullet>(CreateDemonBullet, OnGetDemonBullet, OnReleaseDemonBullet, OnDestroyDemonBullet, maxSize: 30);
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
        Vector3 dir = targetPos - transform.position;
        //var demonBullet = Instantiate(demonBulletPrefab, transform.position, Quaternion.identity).GetComponent<DemonBullet>();
        var demonBullet = pool.Get();
        demonBullet.transform.position = transform.position + dir.normalized;
        demonBullet.Shoot(dir.normalized);
    }

    private DemonBullet CreateDemonBullet()
    {
        DemonBullet demonBullet = Instantiate(demonBulletPrefab).GetComponent<DemonBullet>();
        demonBullet.SetManagedPool(pool);
        return demonBullet;
    }

    private void OnGetDemonBullet(DemonBullet demonBullet)
    {
        demonBullet.gameObject.SetActive(true);
    }

    private void OnReleaseDemonBullet(DemonBullet demonBullet)
    {
        demonBullet.gameObject.SetActive(false);
    }

    private void OnDestroyDemonBullet(DemonBullet demonBullet)
    {
        Destroy(demonBullet.gameObject);
    }
}
