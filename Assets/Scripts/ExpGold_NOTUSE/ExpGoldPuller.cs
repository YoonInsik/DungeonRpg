using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGoldPuller : MonoBehaviour
{
    public static ExpGoldPuller Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        Initialize(30);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 풀링

    #region 골드

    [Header("풀링")]

    [SerializeField] GameObject Gold_Prefab;

    Queue<Gold> GoldQueue = new Queue<Gold>();

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            GoldQueue.Enqueue(CreateNewGold());
        }
    }

    Gold CreateNewGold()
    {
        var newObj = Instantiate(Gold_Prefab).GetComponent<Gold>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Gold GetGold()
    {
        if (Instance.GoldQueue.Count > 0)
        {
            var obj = Instance.GoldQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewGold();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(Gold _gold)
    {
        _gold.gameObject.SetActive(false);
        _gold.transform.SetParent(Instance.transform);
        Instance.GoldQueue.Enqueue(_gold);
    }

    #endregion

    #endregion

}
