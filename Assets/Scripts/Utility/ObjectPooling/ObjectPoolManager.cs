using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    private class ObjectInfo
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject perfab;
        // ��� �̸� ���� �س�������
        public int count;
    }

    // ������ƮǮ �Ŵ��� �غ� �Ϸ�ǥ��
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // ������ ������Ʈ�� key�������� ���� ����
    private string objectName;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (goDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} �̹� ��ϵ� ������Ʈ�Դϴ�.", objectInfos[idx].objectName);
                return;
            }

            goDic.Add(objectInfos[idx].objectName, objectInfos[idx].perfab);
            objectPoolDic.Add(objectInfos[idx].objectName, pool);

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                Poolable poolAbleGo = CreatePooledItem().GetComponent<Poolable>();
                poolAbleGo.Pool.Release(poolAbleGo.gameObject);
            }
        }

        Debug.Log("������ƮǮ�� �غ� �Ϸ�");
        IsReady = true;
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(goDic[objectName]);
        poolGo.GetComponent<Poolable>().Pool = objectPoolDic[objectName];
        return poolGo;
    }

    // �뿩
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetGo(string goName)
    {
        objectName = goName;

        if (goDic.ContainsKey(goName) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", goName);
            return null;
        }

        return objectPoolDic[goName].Get();
    }
}