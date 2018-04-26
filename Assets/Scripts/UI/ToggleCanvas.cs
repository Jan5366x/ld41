using UnityEngine;

namespace UI
{
	public class ToggleCanvas : MonoBehaviour
	{

		public void Toggle()
		{
			Canvas can = GetComponent<Canvas>();

			if (can == null)
			{
				Debug.LogWarning("no cavans found for toggle!");
				return;
			}

			can.enabled = !can.enabled;
		}
	}
}
