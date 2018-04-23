using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Template/Item")]
public class Item : ScriptableObject {
    public string ItemName;
    public Effect ItemEffect;
    public int BasePrice;
    public Sprite DisplaySprite;
    public float ArmorResistence;
}
