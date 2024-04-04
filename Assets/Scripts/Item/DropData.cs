using UnityEngine;

[System.Serializable]
public class DropData
{
    public string name;
    public GameObject item;
    public float dropRate;

    public DropData(string name, GameObject item, float dropRate)
    {
        this.name = name;
        this.item = item;
        this.dropRate = dropRate;
    }
}
