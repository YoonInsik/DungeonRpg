using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using System;
using UnityEngine.Rendering;

public class Rotation : WeaponBase
{
    public GameObject BoxPrefab;
    private IObjectPool<Box> pool;

    float rotateSpeed = 180.0f;
    float count = 3;

    private void Awake()
    {
        pool = new ObjectPool<Box>(CreateBox, OnGetBox, OnReleaseBox, OnDestroyBox, maxSize:10);
    }

    private void Start()
    {
        Activation();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > data.interval)
        {
            elapsedTime = 0.0f;
            ReAct();

        }
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    void Activation()
    {
        for(int i = 0; i < count; i++)
        {
            var box = pool.Get();
            box.transform.parent = transform;
            Vector3 angle = Vector3.forward * 360 * i / count;
            box.transform.Rotate(angle);
            box.transform.Translate(box.transform.up * 1.5f);
        }
    }

    void ReAct()
    {
        for(int j = 0; j < count; j++)
        {
            var reBox = pool.Get();
        }
    }

    private Box CreateBox()
    {
        Box box = Instantiate(BoxPrefab).GetComponent<Box>();
        box.SetManagedPool(pool);
        return box;
    }

    private void OnGetBox(Box box)
    {
        box.gameObject.SetActive(true);
    }

    private void OnReleaseBox(Box box)
    {
        box.gameObject.SetActive(false);
    }

    private void OnDestroyBox(Box box)
    {
        Destroy(box.gameObject);
    }
}
