using System;
using UnityEngine;

public class Distance2D
{
    public static float getDistance(GameObject a, GameObject b)
    {
        float dx = a.transform.position.x - b.transform.position.x;
        float dy = a.transform.position.y - b.transform.position.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
    public static float getDistance(GameObject a, Vector3 b)
    {
        float dx = a.transform.position.x - b.x;
        float dy = a.transform.position.y - b.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
    public static float getDistance(Vector3 a, GameObject b)
    {
        float dx = a.x - b.transform.position.x;
        float dy = a.y - b.transform.position.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}