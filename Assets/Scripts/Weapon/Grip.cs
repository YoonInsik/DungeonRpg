using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour
{
    public Player player;

    Vector3 targetDirection;
    float currentAngle;
    float disAngle = 0;
    float targetAngle = 45.0f;
    float swingSpeed = 90.0f;
    int stage = 0;
    bool IsSwing = false;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (player.scanner.nearestTarget && IsSwing == false)
        {
            TargetSword();
        }
        else if (player.scanner.nearestTarget && IsSwing == true)
        {
            SwingSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsSwing = true;
        }
    }

    void TargetSword()
    {
        targetDirection = player.scanner.nearestTarget.transform.position - transform.position;
        currentAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        currentAngle -= 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
    }

    void SwingSword()
    {
        switch (stage)
        {
            case 0:
                if(disAngle < targetAngle)
                {
                    RotateSword(swingSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    stage = 1;
                    targetAngle = -90.0f;
                }
                break;
            case 1:
                if(disAngle > targetAngle)
                {
                    RotateSword(-swingSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    stage = 2;
                    targetAngle = 45.0f;
                }
                break;
            case 2:
                if(disAngle < targetAngle)
                {
                    RotateSword(swingSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    stage = 0;
                    targetAngle = 45.0f;
                    disAngle = 0.0f;
                }
                break;
        }
    }

    private void RotateSword(float angle)
    {
        transform.Rotate(0, 0, angle);
        currentAngle += angle;
        disAngle += angle;
    }
}
