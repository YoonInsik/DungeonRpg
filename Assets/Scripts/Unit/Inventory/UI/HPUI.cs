using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI HPText;
    public Slider HPBar;
    // Start is called before the first frame update
    void Start()
    {
        player = UnitManager.Instance.player.GetComponent<Player>();
        //Debug.Log(player.HP);
    }

    // Update is called once per frame
    void Update()
    {
        HPText.text = player.HP.ToString();
        HPBar.value = (float)player.HP / player.MaxHP;
        //Debug.Log(player.HP);
    }
}
