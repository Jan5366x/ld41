using TemplateSkeleton;
using UnityEngine;

namespace World.Objects
{
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
}
