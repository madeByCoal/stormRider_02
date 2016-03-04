using UnityEngine;
using System.Collections;

public class SR_EnemySight : MonoBehaviour 
{


	public bool playerInSight;

	public Vector3 personalLastSighting;

	private GameObject player;


	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player"); 	

	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") 
		{
			playerInSight = true;
			personalLastSighting = player.transform.position;

		}
	}


	void OnTriggerExit2D (Collider2D other)
	{
		// If the player leaves the trigger zone...
		if (other.tag == "Player")
		{
			// ... the player is not in sight.

			playerInSight = false;
		}
	}
}

