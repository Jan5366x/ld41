using System.Linq;
using UnityEngine;

public class EquipSlot : MonoBehaviour
{
    public int[] PossibleSlots;

    public bool CanEquip(int slot)
    {
        if (PossibleSlots == null)
            return false;
        return PossibleSlots.Any(possibleSlot => possibleSlot == slot);
    }
}