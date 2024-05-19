using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

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
        if (elapsedTime > interval * player.ATKCooldownDelicacy())
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
            bomb.transform.parent = null;
            bomb.transform.position = transform.position;

            float angle = 180 * i / count;
            bomb.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 dir = bomb.transform.up;
            bomb.Shoot(dir);
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
