using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : Singleton<StatUI>
{
    [SerializeField] private GameObject StatCellPrefab;
    [SerializeField] private Player player;
    private Dictionary<string, int> dict;
    private List<GameObject> cells;

    //한국어 스탯명
    private List<string> stat_kor = new List<string>
    {
        "공격력",
        "방어력",
        "이동속도",
        "투사체 속도",
        "쿨타임 감소",
        "지속시간",
        "탐욕",
        "미식",
        "지혜",
        "유혹"
    };

    // Start is called before the first frame update
    void Start()
    {
        cells = new List<GameObject>();
        player = UnitManager.Instance.player;
        dict = player.StatLevels;

        InstantiateStatCell();
        gameObject.SetActive(false);
    }

    public void InstantiateStatCell()
    {
        int i = 0;
        foreach (var value in dict)
        {
            string dictKey = value.Key;
            int dictValue = value.Value;

            //최대레벨 표시할 text
            TMP_Text maxStatNum = transform.GetChild(1).GetComponent<TMP_Text>();

            if (dictKey == "StatMaxLevel")
            {
                maxStatNum.text = "최대 LV : " + dictValue.ToString();
                return;
            }

            //프리팹과 그 구성요소
            GameObject cell = Instantiate(StatCellPrefab, transform);
            TMP_Text statName = cell.transform.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text statNum = cell.transform.GetChild(1).GetComponent<TMP_Text>();
            Image image = cell.transform.GetChild(1).GetComponent<Image>();
            cells.Add(cell);

            statName.text = stat_kor[i++].ToString();
            statNum.text = ": LV " + dictValue.ToString();
        }
    }

    // 스탯 수치 다시 불러와 그림
    public void UpdateCell()
    {
        int i = 0;
        foreach (var value in dict)
        {
            string dictKey = value.Key;
            int dictValue = value.Value;

            TMP_Text maxStatNum = transform.GetChild(1).GetComponent<TMP_Text>();

            if (dictKey == "StatMaxLevel")
            {
                maxStatNum.text = "최대 LV : " + dictValue.ToString();
                return;
            }

            GameObject cell = cells[i];
            TMP_Text statNum = cell.transform.GetChild(1).GetComponent<TMP_Text>();
            //Image image = cell.transform.GetChild(1).GetComponent<Image>();

            statNum.text = ": LV " + dictValue.ToString();
            i++;
        }
    }
}
