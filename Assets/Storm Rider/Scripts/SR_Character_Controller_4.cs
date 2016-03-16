using UnityEngine;
using System.Collections;

public enum MoveState
{
	Foward,
	Left,
	Right,
	Die
}

public class SR_Character_Controller_4 : MonoBehaviour {
	public MoveState moveState;
	private Vector3 moveDirection;

	public GameObject DestFloor;
	//public float moveStep;
	// Use this for initialization
	void Start () {
		moveDirection = Quaternion.AngleAxis (30, transform.forward) * transform.right;

	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKey("a")) {
            iTween.MoveTo(gameObject, DestFloor.transform.position, 2f);
		}


		DispalyVelocitys ();
	}

	void DispalyVelocitys()
	{
		Debug.DrawRay(transform.position, moveDirection, Color.cyan);
	}


}
