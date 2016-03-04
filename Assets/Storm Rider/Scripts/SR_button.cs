using UnityEngine;
using System.Collections;

public class SR_button : MonoBehaviour 
{
	public bool buttonPress = false;
	private Touch touch;
	private GUITexture speedButton;

	void Awake ()
	{
		speedButton = gameObject.GetComponent<GUITexture> ();
	}

	void Update ()
	{
		if ( speedButton.HitTest(touch.position))
		{
			testButton ();
		}
	}

	void testButton ()
	{
		if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				buttonPress = true;
				Debug.Log (gameObject + "is pressed");
			}
			else
				buttonPress = false;
	}
}
