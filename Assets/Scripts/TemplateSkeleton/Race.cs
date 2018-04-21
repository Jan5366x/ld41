using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Race", menuName = "Template/Race")]
public class Race : ScriptableObject
{
    public GameObject StartPoint;
    public float TimeLimit;
}
