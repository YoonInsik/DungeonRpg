using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropTableUI : MonoBehaviour
{
    [SerializeField] private List<Image> dropItemImages;

    public void SetDropItemImages(List<DropData> droptable)
    {
        for (int i = 0; i < droptable.Count; i++)
        {
            dropItemImages[i].sprite = droptable[i].item.icon;
            dropItemImages[i].gameObject.SetActive(true);
        }
        for (int i = droptable.Count; i < dropItemImages.Count; i++)
        {
            dropItemImages[i].gameObject.SetActive(false);
        }
    }
}
