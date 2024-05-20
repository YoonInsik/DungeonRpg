using UnityEngine;
using UnityEngine.UI;

public class DropInfo : MonoBehaviour
{
    [SerializeField] private Image img;

    private void Update()
    {
        var dropTable = MapManager.Instance.CurChunk.data.dropTable;
        if (dropTable.Count <= 0)
        {
            img.color = new Color(1, 1, 1, 0);
            return;
        }

        var icon = dropTable[0].item.icon;   
        if (icon is null)
        {
            img.color = new Color(1, 1, 1, 0);
        }
        else
        {
            img.sprite = icon;   
            img.color = new Color(1, 1, 1, 1);
        }
    }
}
