using UnityEngine;
using System.Collections;

public class SR_Character_rotateDummy : MonoBehaviour 
{
	public Vector2 inputDirection;
	public float turnAngle = 0;
	public float turnSpeed = 1;
	
	void Update ()
	{
		inputDirection.x = Input.GetAxis ("Horizontal");
		inputDirection.y = Input.GetAxis ("Vertical");
		inputDirection = inputDirection.normalized; 
		Vector3 inputDir = new Vector3 (inputDirection.x, inputDirection.y, 0f);
		

		// convert the world relative moveInput vector into a local-relative
		inputDir = transform.InverseTransformDirection(inputDir);
		turnAngle = Mathf.Atan2(inputDir.y , inputDir.x) * Mathf.Rad2Deg;
		transform.Rotate(0,0,turnAngle * turnSpeed * Time.deltaTime);

	}
}
