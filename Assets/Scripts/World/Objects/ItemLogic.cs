using System.Linq;
using TemplateSkeleton;
using UnityEngine;

namespace World.Objects
{
	public class ItemLogic : MonoBehaviour
	{
		public Item Template;
		
		public bool CanEquip(ItemSlot slot)
		{
			if (Template.PossibleSlots == null)
				return false;
			return Template.PossibleSlots.Any(possibleSlot => possibleSlot == slot);
		}
	}
}
