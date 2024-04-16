using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SixBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    float interval = 2.0f;
    float elapsedTime = 0.0f;
    int count = 6;
    Player player;

    private IObjectPool<Bomb> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bomb>(CreateBomb, OnGetBomb, OnReleaseBomb, OnDestroyBomb, maxSize: 12);
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
        for (int i = 0; i < count; i++)
        {
            var bomb = pool.Get();
            bomb.transform.parent = transform;
            Vector3 angle = Vector3.forward * 360 * i / count;
            bomb.transform.Rotate(angle);
            Vector3 dir = bomb.transform.position - transform.position;
            bomb.Shoot(dir.normalized);
        }
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
