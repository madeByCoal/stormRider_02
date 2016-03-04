using UnityEngine;
using System.Collections;

public class SR_SFX_Eny_walker_att_01 : MonoBehaviour 
{
	public Transform playerTransform;
	public ParticleEmitter parEmitter;
	//private Particle[] particles;
	public float firePower;
	public bool inAttactZone;

	void Awake ()
	{
		GetComponent<ParticleEmitter>().emit = false;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		parEmitter = gameObject.GetComponent<ParticleEmitter>();
		//particles = particleEmitter.particles;
	}

	void OnTriggerEnter2D (Collider2D other)
	{

		if (other.tag == "Player") 
		{
			inAttactZone = true;
			GetComponent<ParticleEmitter>().emit = true;
			

		}
	}

	void OnTriggerExit2D (Collider2D other)
		
	{
		if (other.tag == "Player") 
		{
			inAttactZone = false;
			GetComponent<ParticleEmitter>().emit = false;
		}
	}

	void Update ()  
	{
		parEmitter.worldVelocity = playerTransform.position - transform.position;
		parEmitter.worldVelocity *= firePower;
	}
}
