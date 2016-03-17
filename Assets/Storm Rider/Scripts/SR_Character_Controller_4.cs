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
    public GameObject currentFloor;

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
        DispalyVelocitys();

        changeMoveDirection();
        getCurentFloor();
        if (currentFloor != null)
        {
            DirectionToVector(moveDirection);
        }
        //DirectionToVector(moveDirection);
        getTargetFloor();
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetFloor.transform.position, Time.deltaTime * moveStep);
    }

    void DispalyVelocitys ()
	{
        Debug.DrawRay(transform.position, direcVector, Color.cyan);
    }

    void getCurentFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
        if (hit.collider != null && hit.collider.tag == "Floor")
        {
            currentFloor = hit.collider.gameObject;
        }
    }

    void getTargetFloor()
    {
        Physics2D.queriesStartInColliders = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcVector);
        if (hit.collider != null && hit.collider.tag == "Floor")
        {
            targetFloor = hit.collider.gameObject;
        }

        //int hitCount;
        //RaycastHit2D [] hits = new RaycastHit2D[2];
        //hitCount = Physics2D.LinecastNonAlloc(transform.position, direcVector,hits);

        //foreach (RaycastHit2D hit in hits)
        //{
        //    Debug.Log("hitCount =" + hitCount);
        //    if ( hit.transform != null && hit.transform.tag == "Floor")
        //    {
        //        Debug.Log("getTargetFloor");
        //        targetFloor = hit.collider.gameObject;
        //    }
        //}
    }
}
