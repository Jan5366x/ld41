using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    private GameObject obj;
    public void Fire(GameObject obj, float duration)
    {
        this.obj = obj;
        StartCoroutine("Nuke", duration);
    }

    IEnumerator Nuke(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }

    private void Update()
    {
        
    }
}
