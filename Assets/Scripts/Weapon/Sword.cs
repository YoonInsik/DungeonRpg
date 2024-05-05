using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public class Sword : MonoBehaviour
{
    protected enum SwordState
    {
        Wait = 0,
        Swing,
        Thrust
    }

    SwordState state = SwordState.Wait;

    protected SwordState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case SwordState.Wait:
                        //StartCoroutine(TargetSword());
                        break;
                    case SwordState.Swing:
                        StartCoroutine(SwingSword());
                        break;
                    case SwordState.Thrust:
                        StartCoroutine(ThrustSword());
                        break;
                }
            }
        }
    }

    /*IEnumerator TargetSword()
    {
        while (true) // 무한 루프로 계속 실행
        {
            if (player.scanner.nearestTarget != null)
            {
                Vector3 targetDirection = player.scanner.nearestTarget.transform.position - transform.position;
                float currentAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                currentAngle -= 90;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            yield return null;
        }
    }*/
    IEnumerator SwingSword()
    {
        Vector3 directionToTarget = (player.scanner.nearestTarget.position - transform.position).normalized;
        Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);
        Vector3 newDirection = rotationOffset * directionToTarget;
        float distance = Vector3.Distance(transform.position, player.scanner.nearestTarget.position);
        transform.position = transform.position + newDirection * distance;

        yield return new WaitForSeconds(0.5f);

        Vector3 swingDirection = (player.scanner.nearestTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, swingDirection);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, swingSpeed * Time.deltaTime);
            yield return null;
        }
        while (Quaternion.Angle(transform.rotation, originalRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, swingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator ThrustSword()
    {
        while (true) // 무한 루프로 계속 실행
        {
            if (player.scanner.nearestTarget != null) // 타겟이 설정되어 있으면 실행
            {        // 타겟 위치로 이동
                while (Vector3.Distance(transform.position, player.scanner.nearestTarget.position) > 0.1f) // 타겟과의 거리가 0.1f 이상일 때 계속 실행
                {
                    Vector3 direction = (player.scanner.nearestTarget.position - transform.position).normalized; // 타겟 방향 벡터 계산
                    transform.position += direction * speed * Time.deltaTime; // 타겟 방향으로 이동
                    yield return null; // 다음 프레임까지 기다림
                }

                // 원래 위치로 돌아감
                while (transform.position != originalPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }

    public Player player;
    private Quaternion originalRotation; // 원래 회전
    bool isActive = false;
    float currentAngle;
    float sqrDistance;
    Vector3 targetDirection;
    Vector3 difference;
    Vector3 originalPosition;
    public float swingSpeed = 10f; // 휘두르는 속도
    public float angleOffset = 30f; // 이동 각도 오프셋
    public int pattern;
    float speed = 5.0f;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        int n = transform.GetSiblingIndex();
        if(n == 0)
        {
            originalPosition = player.transform.position + new Vector3(1.0f, 0.0f, 0.0f);
        }
        else if (n == 1)
        {
            originalPosition = player.transform.position + new Vector3(-1.0f, 0.0f, 0.0f);
        }
        else if (n == 2)
        {
            originalPosition = player.transform.position + new Vector3(0.5f, 0.866f, 0.0f);
        }
        else if (n == 3)
        {
            originalPosition = player.transform.position + new Vector3(-0.5f, 0.866f, 0.0f);
        }
        else if (n == 4)
        {
            originalPosition = player.transform.position + new Vector3(-0.5f, -0.866f, 0.0f);
        }
        else if (n == 5)
        {
            originalPosition = player.transform.position + new Vector3(0.5f, -0.866f, 0.0f);
        }

        originalRotation = transform.rotation;
    }

    private void Update()
    {
        pattern = Random.Range(0, 1);
        difference = player.scanner.nearestTarget.position - player.transform.position;
        sqrDistance = difference.sqrMagnitude;
        Vector3 targetDirection = player.scanner.nearestTarget.transform.position - transform.position;
        float currentAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        currentAngle -= 90;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        if (sqrDistance < 16)
        {
            if (pattern == 0)
            {
                State = SwordState.Thrust;
            }
            if (pattern == 1)
            {
                State = SwordState.Swing;
            }
        }
        else if (sqrDistance > 16)
        {
            State = SwordState.Wait;
        }
    }
}
