using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public void Fire(float duration)
    {
        StartCoroutine("Nuke", duration);
    }

    IEnumerator Nuke(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
