using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
	public GameObject obj;
	public int quantity;

    // Use this for initialization
    void Start () {
		
    }
	
    // Update is called once per frame
    void Update () {
		
    }

	public InventoryItem Copy()
	{
		InventoryItem ii = gameObject.AddComponent<InventoryItem>();
		ii.obj = obj;
		ii.quantity = quantity;
		return ii;
	}
}
