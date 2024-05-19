using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Rocket : MonoBehaviour
{
    Player player;
    float speed = 10.0f;
    Vector3 direction;
    bool isMoving = true;
    bool isReturned = false;

    private IObjectPool<Rocket> managedPool;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // 로켓이 전역 좌표계를 기준으로 움직이도록 설정
            transform.Translate(direction * speed * Time.fixedDeltaTime * player.ATKSpeedDelicacy(), Space.World);
        }
    }

    public void Shoot(Vector3 direction)
    {
        this.direction = direction; // 방향을 저장
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90); // 스프라이트가 바라볼 각도 조정
        isMoving = true;
        isReturned = false;
        Invoke("DestroyRocket", 2.0f); // 2초 후 로켓 파괴
    }

    public void SetManagedPool(IObjectPool<Rocket> pool)
    {
        managedPool = pool;
    }

    public void DestroyRocket()
    {
        if (!isReturned)
        {
            isReturned = true;
            DeactivateChildren();
            isMoving = false;
            managedPool.Release(this);
            CancelInvoke("DestroyRocket");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ActivateChildren();
            isMoving = false;
            Invoke("DestroyRocket", 0.5f);
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
        }
    }
}
