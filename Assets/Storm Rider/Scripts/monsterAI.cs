using UnityEngine;
using System.Collections;

public class monsterAI : MonoBehaviour 
{

	public Transform trans;
	public float chaseSpeed;

	private SR_EnemySight enemySight;
	//private Transform playerTrans;


	void Awake ()
	{
		trans = transform;
		enemySight = GetComponent<SR_EnemySight> ();
		//playerTrans = GameObject.FindGameObjectWithTag ("player").transform;
	}

	// Update is called once per frame
	void Update ()
	{
		if (enemySight.playerInSight == true) 
		{
			Chasing ();
		}
		if (enemySight.playerInSight == false) 
		{
			Stop ();
		}
	}


	void Chasing ()
	{
		trans.position = Vector3.Lerp (transform.position, enemySight.personalLastSighting, chaseSpeed * Time.deltaTime);
	}

	void Stop ()
	{
		trans.position = transform.position;

	}
}




