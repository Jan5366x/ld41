using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
	public GameObject Obj;
	public int Quantity;
	public Item Template;

	public InventoryItem Copy()
	{
		InventoryItem ii = new InventoryItem
		{
			Obj = Obj,
			Quantity = Quantity,
			Template = Template
		};
		return ii;
	}
}
