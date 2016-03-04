using UnityEngine;
using System.Collections;

public class SR_Cha_EnergySys : MonoBehaviour 
{
	public float energy;
	public float repeatDamagePeriod;
	public float damageAmount;

	private GUITexture energyBar;
	private float energWidth;

	void Awake ()
	{
		energyBar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<GUITexture>();
		energWidth = 300f;
	}
	
	void OnParticleCollision (GameObject other)
	{
		Debug.Log("damage!");
		TakeDamage (); 
	}

	void TakeDamage ()
	{
		energy -= damageAmount;
		UpdateEnergyBar();
		Debug.Log("energy =" + energy);
	}

	void UpdateEnergyBar ()
	{	
		if (energWidth > 0f)
		{
			energWidth -= damageAmount * 3f;
			energyBar.pixelInset = new Rect (-800, 265, energWidth, 50);
		}

		Debug.Log (energWidth);
	}
}


