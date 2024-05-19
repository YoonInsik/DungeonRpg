using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;


public class Bomb : MonoBehaviour
{
    float damage;
    float speed = 5.0f;
    Vector3 direction;
    bool isMoving = true;
    bool isReturned = false;

    private IObjectPool<Bomb> managedPool;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.Translate(direction * speed * Time.fixedDeltaTime * UnitManager.Instance.player.ATKSpeedDelicacy());
        }
    }

    public void Shoot(Vector3 dir, float _damage, float _speed)
    {
        direction = dir;
        damage = _damage;
        speed = _speed;
        isMoving = true;
        isReturned = false;
        Invoke("DestroyBomb", 2.0f);
    }

    public void SetManagedPool(IObjectPool<Bomb> pool)
    {
        managedPool = pool;
    }

    public void DestroyBomb()
    {
        if (!isReturned)
        {
            isReturned = true;
            DeactivateChildren();
            isMoving = false;
            managedPool.Release(this);
            CancelInvoke("DestroyBomb");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ActivateChildren();
            isMoving = false;
            Invoke("DestroyBomb", 0.5f);
        }
    }

    private void DeactivateChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void ActivateChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.GetComponent<Boom>().damage = damage;
        }
    }
}