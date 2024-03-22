using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable_", menuName = "SO/DropTable")]
public class DropTable : ScriptableObject
{
    public List<Drop> droptable;
}

[System.Serializable]
public struct Drop
{
    public string name;
    public GameObject item;
    public float dropRate;
}
