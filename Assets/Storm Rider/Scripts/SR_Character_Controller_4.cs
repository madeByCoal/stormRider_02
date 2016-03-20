using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Move,
	Die
}

public enum Direction
{
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}

public class SR_Character_Controller_4 : MonoBehaviour
{
	public PlayerState playerState;
	public Direction moveDirection;
    public Vector3 direcVector;
    private Vector3 sliderVector_R;
    private Vector3 sliderVector_L;
    public GameObject GameManager;
    public GameObject targetFloor;
    public GameObject currentFloor;

    private float isometricAngle;
    private Animator anim;
    public float moveStep;
    
 
    void Start () {
        //moveDirection = MoveDirection.NorthEast;
        isometricAngle = Mathf.Rad2Deg * Mathf.Atan2(0.5f, 1f);   //根据x，y坐标比值算出视角
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        anim = GetComponent<Animator>();
    }

    Vector3 DirectionToVector(Direction moveDirection)
    {
        Vector3 moveVector=  Vector3.one;
        switch (moveDirection)
        {
            case Direction.NorthEast:
                moveVector = Quaternion.AngleAxis(isometricAngle, transform.forward) * transform.right;
                break;
            case Direction.NorthWest:
                moveVector = Quaternion.AngleAxis(180- isometricAngle, transform.forward) * transform.right;
                break;
            case Direction.SouthEast:
                moveVector = Quaternion.AngleAxis(-isometricAngle, transform.forward) * transform.right;
                break;
            case Direction.SouthWest:
                moveVector = Quaternion.AngleAxis(-(180 - isometricAngle), transform.forward) * transform.right;
                break;
            default:
                break;
        }
        return moveVector;
    }

    void changeMoveDirection()
    {
        if (Input.GetKeyDown("w"))
        {
            moveDirection = Direction.NorthEast;
        }
        else if (Input.GetKeyDown("a"))
        {
            moveDirection = Direction.NorthWest;
        }
        else if (Input.GetKeyDown("s"))
        {
            moveDirection = Direction.SouthWest;
        }
        else if (Input.GetKeyDown("d"))
        {
            moveDirection = Direction.SouthEast;
        }
        GameManager.SendMessage("getPlayerDir", moveDirection);
    }

	void Update ()
    {
        DispalyVelocitys();
        playAnimation();
        changeMoveDirection();
        getCurentFloor();
        if (currentFloor != null)
        {
            direcVector = DirectionToVector(moveDirection);
        }
        getTargetFloor();
        Move();
        Slide();
    }

    void playAnimation()
    {
        switch (moveDirection)
        {
            case Direction.NorthEast:
                break;
            case Direction.NorthWest:
                break;
            case Direction.SouthEast:
                if (Input.GetKey("s"))
                {
                    Debug.Log("s");
                    anim.SetTrigger("SE-SW");
                }
                break;
            case Direction.SouthWest:
                if (Input.GetKey("d"))
                {
                    Debug.Log("d");
                    anim.SetTrigger("SW-SE");
                }
                break;
            default:
                break;
        }
        
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetFloor.transform.position, Time.deltaTime * moveStep);
    }

    void Slide()
    {
        switch (moveDirection)
        {
            case Direction.NorthEast:
                sliderVector_R = DirectionToVector(Direction.SouthEast);
                sliderVector_L = DirectionToVector(Direction.NorthWest);
                break;
            case Direction.NorthWest:
                sliderVector_R = DirectionToVector(Direction.NorthEast);
                sliderVector_L = DirectionToVector(Direction.SouthWest);
                break;
            case Direction.SouthEast:
                sliderVector_R = DirectionToVector(Direction.SouthWest);
                sliderVector_L = DirectionToVector(Direction.NorthEast);
                break;
            case Direction.SouthWest:
                sliderVector_R = DirectionToVector(Direction.NorthWest);
                sliderVector_L = DirectionToVector(Direction.SouthEast);
                break;
            default:
                break;
        }

        if (Input.GetKeyDown("u"))
        {
            transform.position += sliderVector_L;
        }
        else if (Input.GetKeyDown("i"))
        {
            transform.position += sliderVector_R;
        }
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
        int hitCount;
        RaycastHit2D[] hits = new RaycastHit2D[3];
        hitCount = Physics2D.RaycastNonAlloc(transform.position, direcVector, hits);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Floor")
            {
                targetFloor = hit.collider.gameObject;
            }
        }
    }
}
