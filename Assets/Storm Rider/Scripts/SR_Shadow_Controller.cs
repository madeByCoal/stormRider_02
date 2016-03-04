using UnityEngine;
using System.Collections;

public class SR_Shadow_Controller : MonoBehaviour 
{	
	private Vector2 moveDirection;
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
	}
}