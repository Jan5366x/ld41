using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDamage : MonoBehaviour
{
    public float spreadX = 0.1f;
    public float spreadY = 0.05f;
    public float charWidth = 0.1f;

    public void Show(float damage)
    {
        string dmg = ((int) damage).ToString();
        float dx = Random.value * spreadX - spreadX / 2;
        float dy = Random.value * spreadY - spreadY / 2;
        dx -= 0.1f;
        dy += 0.14f;

        foreach (var chr in dmg)
        {
            var a = Resources.Load("Damage\\damage" + chr);
            var obj = Instantiate(a, transform) as GameObject;
            if (obj)
            {
                obj.transform.Translate(dx, dy, 0);
                dx += charWidth;
            }
        }
    }
}