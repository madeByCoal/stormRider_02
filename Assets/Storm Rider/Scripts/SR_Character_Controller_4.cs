using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Move,
	Die
}

public enum MoveDirection
{
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}

public class SR_Character_Controller_4 : MonoBehaviour {
	public PlayerState playerState;
	public MoveDirection moveDirection;
    public Vector3 direcVector;
	public GameObject DestFloor;
    private float isometricAngle;
    //public float moveStep;
    // Use this for initialization
    void Start () {
        moveDirection = MoveDirection.NorthEast;
        isometricAngle = Mathf.Rad2Deg * Mathf.Atan2(0.5f, 1f);
    }

    void DirectionToVector(MoveDirection moveDirection)
    {
        switch (moveDirection)
        {
            case MoveDirection.NorthEast:
                direcVector = Quaternion.AngleAxis(isometricAngle, transform.forward) * transform.right;
                break;
            case MoveDirection.NorthWest:
                direcVector = Quaternion.AngleAxis(180- isometricAngle, transform.forward) * transform.right;
                break;
            case MoveDirection.SouthEast:
                direcVector = Quaternion.AngleAxis(-isometricAngle, transform.forward) * transform.right;
                break;
            case MoveDirection.SouthWest:
                direcVector = Quaternion.AngleAxis(-(180 - isometricAngle), transform.forward) * transform.right;
                break;
            default:
                break;
        }
    }

    void switchDirection()
    {
        //switch (KeyCode.all)
        //{
        //    default:
        //        break;
        //}
    }

	// Update is called once per frame
	void Update () {

          
        if (Input.GetKeyDown("w"))
        {
            //iTween.MoveTo(gameObject, DestFloor.transform.position, 2f);
            moveDirection = MoveDirection.NorthEast;
        }
        else if (Input.GetKeyDown("a"))
        {
            moveDirection = MoveDirection.NorthWest;
        }
        else if (Input.GetKeyDown("s"))
        {
            moveDirection = MoveDirection.SouthWest;
        }
        else if (Input.GetKeyDown("d"))
        {
            moveDirection = MoveDirection.SouthEast;
        }

        DirectionToVector(moveDirection);
        DispalyVelocitys();
	}

	void DispalyVelocitys()
	{
        Debug.DrawRay(transform.position, direcVector, Color.cyan);
    }


}
