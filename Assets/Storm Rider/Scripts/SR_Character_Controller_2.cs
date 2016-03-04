using UnityEngine;
using System.Collections;

public class SR_Character_Controller_2 : MonoBehaviour 
{	
	public Vector2 moveDirection;
	public float moveForce;
	public float moveForceMax;
	public GameObject rotateDummy;
	private Animator anim;

	void Awake ()
	{
		anim = GetComponent<Animator>();
	}

	void Update ()
	{
		moveDirection.x = rotateDummy.transform.right.x;
		moveDirection.y = rotateDummy.transform.right.y;

		//animation
		anim.SetFloat ("x",moveDirection.x);
		anim.SetFloat ("y",moveDirection.y);

		if (Input.GetButton ("Accelerator"))
		{
			moveForce = Mathf.Lerp (moveForce,moveForceMax,Time.time);
		}

		if (!Input.GetButton ("Accelerator"))
		{
			moveForce = Mathf.Lerp (moveForce,100000,Time.time);
		}
	

		GetComponent<Rigidbody2D>().AddForce (moveDirection * moveForce * Time.deltaTime);
	}
}