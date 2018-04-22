using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDamage : MonoBehaviour
{
    
    public void Show(float damage)
    {
        string dmg = ((int)damage).ToString();
        foreach (var chr in dmg)
        {
            Instantiate(Resources.Load("damage"+chr, typeof(GameObject)));
        }
    }
}