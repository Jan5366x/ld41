using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
	public abstract void interact(UnitLogic player);

	public bool CanInteract(UnitLogic obj)
	{
		return (transform.position - obj.transform.position).sqrMagnitude < 0.1;
	}
}
