using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImplement : MonoBehaviour
{
    public SpriteRenderer image;
    public MeatItem item;
    // Start is called before the first frame update
    void Start()
    {
        if (item != null && image != null)
        {
            image.sprite = item.icon;
        }
    }
}
