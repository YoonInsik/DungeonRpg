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
                        StartCoroutine(TargetSword());
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

    IEnumerator TargetSword()
    {
        while (true) // ���� ������ ��� ����
        {
            if (player.scanner.nearestTarget != null) // ���� ����� Ÿ���� ���� ��쿡�� ����
            {
                Vector3 targetDirection = player.scanner.nearestTarget.transform.position - transform.position;
                float currentAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                currentAngle -= 90;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            yield return null; // ���� �����ӱ��� ��ٸ�
        }
    }
    IEnumerator SwingSword()
    {
        Vector3 directionToTarget = (player.scanner.nearestTarget.position - transform.position).normalized;
        Quaternion rotationOffset = Quaternion.Euler(0, 0, angleOffset);
        Vector3 newDirection = rotationOffset * directionToTarget;
        float distance = Vector3.Distance(transform.position, player.scanner.nearestTarget.position);
        transform.position = transform.position + newDirection * distance;

        yield return new WaitForSeconds(0.5f); // ���� �̵� �� ��� ���

        // Ÿ���� ���� �ֵθ��� ����
        Vector3 swingDirection = (player.scanner.nearestTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, swingDirection);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);
            yield return null;
        }

        // ���� ��ġ�� ȸ������ ���ư���
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
        while (true) // ���� ������ ��� ����
        {
            if (player.scanner.nearestTarget != null) // Ÿ���� �����Ǿ� ������ ����
            {        // Ÿ�� ��ġ�� �̵�
                while (Vector3.Distance(transform.position, player.scanner.nearestTarget.position) > 0.1f) // Ÿ�ٰ��� �Ÿ��� 0.1f �̻��� �� ��� ����
                {
                    Vector3 direction = (player.scanner.nearestTarget.position - transform.position).normalized; // Ÿ�� ���� ���� ���
                    transform.position += direction * speed * Time.deltaTime; // Ÿ�� �������� �̵�
                    yield return null; // ���� �����ӱ��� ��ٸ�
                }

                // ���� ��ġ�� ���ư�
                while (transform.position != originalPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }

    public Player player;
    private Quaternion originalRotation; // ���� ȸ��
    bool isActive = false;
    float currentAngle;
    float sqrDistance;
    Vector3 targetDirection;
    Vector3 difference;
    Vector3 originalPosition;
    public float swingSpeed = 10f; // �ֵθ��� �ӵ�
    public float angleOffset = 30f; // �̵� ���� ������
    float speed = 5.0f;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        int pattern = Random.Range(0, 1);
        difference = player.scanner.nearestTarget.position - player.transform.position;
        sqrDistance = difference.sqrMagnitude;
        if (player.scanner.nearestTarget != null && sqrDistance < 25)
        {
            if (pattern == 0)
            {
                state = SwordState.Thrust;
            }
            if (pattern == 1)
            {
                state = SwordState.Swing;
            }
        }
        else
        {
            state = SwordState.Wait;
        }
    }
}
