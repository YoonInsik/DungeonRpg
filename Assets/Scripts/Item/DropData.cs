using UnityEngine;

[System.Serializable]
public class DropData
{
    public string name;
    public MeatItem item;
    public float dropRate;

    public DropData(string name, MeatItem item, float dropRate)
    {
        this.name = name;
        this.item = item;
        this.dropRate = dropRate;
    }
}
