using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullnessUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI fullnessText;
    public Slider fullnessBar;

    // Start is called before the first frame update
    void Start()
    {
        player = UnitManager.Instance.player;
        if (player == null) Debug.Log("�÷��̾ ã���� ����");
        else Debug.Log("�÷��̾� �߰�");
    }

    // Update is called once per frame
    void Update()
    {
        fullnessText.text = player.Fullness.ToString();
        fullnessBar.value = player.Fullness / player.MaxFullness;
    }
}
