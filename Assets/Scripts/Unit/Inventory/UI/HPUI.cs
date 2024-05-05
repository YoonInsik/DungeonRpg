using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Player player;
    public Text HPText;
    public RectTransform HPBar;
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
        HPBar.localScale = new Vector3(player.HP / player.GetBaseHP(), 1, 1);
        //Debug.Log(player.HP);
    }
}
