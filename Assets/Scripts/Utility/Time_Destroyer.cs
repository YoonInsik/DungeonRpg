using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Time_Destroyer : MonoBehaviour
{
    [SerializeField] float destroy_time;

    private void Start()
    {
        StartCoroutine(Destroy(destroy_time));
    }

    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
