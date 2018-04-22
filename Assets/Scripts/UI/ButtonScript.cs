using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

	public Sprite buttonNormal;
	public Sprite buttonHover;
	public Color textColorNormal = Color.white;
	public Color textColorHover = Color.yellow;

	public void SetStateHover()
	{
		SetButtonImage(buttonHover);
		SetButtonTextColor(textColorHover);
	}

	public void SetStateNormal()
	{
		SetButtonImage(buttonNormal);
		SetButtonTextColor(textColorNormal);
	}

	private void SetButtonImage(Sprite sprite)
	{
		Image img = GetComponent<Image>();
		img.sprite = sprite;
	}

	private void SetButtonTextColor(Color color)
	{
		Transform textTransform = transform.Find("Text");
		if (textTransform == null)
		{
			Debug.LogWarning("not text game object found!");
			return;	
		}

		Text text =  textTransform.GetComponent<Text>();
		text.color = color;
	}
}
