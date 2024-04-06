using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cooking_", menuName = "item/cooking")]
public class CookingItem : ScriptableObject
{
    [Serializable] public struct Ingredient
    {
        public MeatItem ingredient;
        public int amount;
    }

    [SerializeField] public List<Ingredient> recipe;

    public string itemName;
    public Sprite icon = null;
    public float fullness;

    public int ATK;
    public int SPEED;
    public int HP;
    public int DEF;
}