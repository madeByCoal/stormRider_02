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

public class SR_Character_Controller_4 : MonoBehaviour
{
	public PlayerState playerState;
	public MoveDirection moveDirection;
    public Vector3 direcVector;
    public GameObject GameManager;
    public GameObject targetFloor;
    private GameObject currentFloor;

    private float isometricAngle;
    public float moveStep;
    
 
    void Start () {
        moveDirection = MoveDirection.NorthEast;
        isometricAngle = Mathf.Rad2Deg * Mathf.Atan2(0.5f, 1f);   //根据x，y坐标比值算出视角
        GameManager = GameObject.FindGameObjectWithTag("GameController");
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

    void changeMoveDirection()
    {
        if (Input.GetKeyDown("w"))
        {
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
        GameManager.SendMessage("getPlayerDir", moveDirection);
    }

	void Update ()
    {
        changeMoveDirection();
        DirectionToVector(moveDirection);
        getTargetFloor();
        DispalyVelocitys();
        Move();
    }

    void Move()
    {
        transform.position += direcVector * Time.deltaTime*moveStep;
    }

    void DispalyVelocitys ()
	{
        Debug.DrawRay(transform.position, direcVector, Color.cyan);
    }


    void getTargetFloor()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcVector);
        if (hit.collider.tag == "Floor")
        {
            targetFloor = hit.collider.gameObject;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Floor")
        {
            currentFloor = other.gameObject;
        }
    }

    

}
