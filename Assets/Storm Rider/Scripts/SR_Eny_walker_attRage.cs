using UnityEngine;
using System.Collections;

public class SR_Eny_walker_attRage : MonoBehaviour 
{
	public bool inAttactZone;
	public GameObject player;

	void Awake ()
	{
		Debug.Log("awake");
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") 
		{
			inAttactZone = true;
			player.GetComponent<ParticleEmitter>().emit = true;
			
			Debug.Log("fire!");
		}
	}
	
	void OnTriggerExit2D (Collider2D other)
		
	{
		if (other.tag == "Player") 
		{
			inAttactZone = false;
			
			player.GetComponent<ParticleEmitter>().emit = false;
			
			
		}
	}
}