using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Move,
    Slide,
	Die
}

public enum Direction
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public class SR_Character_Controller_4 : MonoBehaviour
{
	public PlayerState playerState;
	public Direction moveDirection;
    public Direction tempDirection;
    public Vector3 direcVector;
    public GameObject GameManager;
    public GameObject targetFloor;
    public GameObject currentFloor;
    private float isometricAngle;
    
    private Animator anim;
    public float moveStep;


    void Awake()
    {
        playerState = PlayerState.Move;
    }
 
    void Start () {
        isometricAngle = Mathf.Rad2Deg * Mathf.Atan2(0.5f, 1f);   //根据x，y坐标比值算出视角
        direcVector = DirectionToVector(moveDirection);
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        DispalyVelocitys();
        playAnimation();
        changeMoveDirection();
        Move();

        DisplayTargetFloor();

    }

    Vector3 DirectionToVector(Direction moveDirection)
    {
        Vector3 moveVector = Vector3.one;
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
            case Direction.North:
                moveVector = transform.up;
                break;
            case Direction.South:
                moveVector = -transform.up;
                break;
            case Direction.East:
                moveVector = transform.right;
                break;
            case Direction.West:
                moveVector = -transform.right;
                break;
            default:
                break;
        }
        return moveVector;
    }

    void changeMoveDirection()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            moveDirection = GetNewDirection(moveDirection,Mathf.CeilToInt(Input.GetAxis("Horizontal")),2);
            direcVector = DirectionToVector(moveDirection);
        }

        if (Input.GetButtonDown ("Slide"))
        {
            playerState = PlayerState.Slide;
            tempDirection = GetNewDirection(moveDirection, Mathf.CeilToInt(Input.GetAxis("Slide")), 1);
            direcVector = DirectionToVector(tempDirection);
        }
     
        GameManager.SendMessage("getPlayerDir", moveDirection);
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
                if (Input.GetKeyDown("s"))
                {
                    Debug.Log("s");
                    anim.SetTrigger("SE-SW");
                }
                break;
            case Direction.SouthWest:
                if (Input.GetKeyDown("d"))
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
        currentFloor = getCurentFloor();
        if (targetFloor != null)
        {
            targetFloor.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        switch (playerState)
        {
            case PlayerState.Move:
                if (targetFloor == null || currentFloor == targetFloor)
                {
                    targetFloor = getNewTargetFloor();
                }
                break;
            case PlayerState.Slide:
                if (targetFloor == null || currentFloor == targetFloor)
                {
                    targetFloor = getNewTargetFloor();
                    direcVector = DirectionToVector(moveDirection);
                    playerState = PlayerState.Move;
                }
                break;
            case PlayerState.Die:
                break;
            default:
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetFloor.transform.position, Time.deltaTime * moveStep);
    }


    Direction GetNewDirection(Direction curDir,int indexShift,int indexStep)
    {
        Direction newDir = curDir;
        if (curDir == Direction.NorthWest && indexShift > 0)
        {
            if (indexStep == 1)
            {
                newDir = Direction.North;
            }
            else if (indexStep == 2)
            {
                newDir = Direction.NorthEast;
            }
        }
        else  if (curDir == Direction.NorthEast && indexShift < 0)
        {
            if (indexStep == 2)
            {
                newDir = Direction.NorthWest;
            }
        }

        else
        {
            newDir = curDir + indexShift * indexStep;
        }

        return newDir;
    }

    void DispalyVelocitys ()
	{
        Debug.DrawRay(transform.position, direcVector, Color.cyan);
    }

    void DisplayTargetFloor()
    {
        if (targetFloor !=null)
        {
            targetFloor.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    GameObject getCurentFloor()
    {
        GameObject curFloor = null;
        int hitCount;
        RaycastHit2D[] hits = new RaycastHit2D[3];
        hitCount = Physics2D.RaycastNonAlloc(transform.position, transform.forward, hits);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Floor")
            {
                curFloor = hit.collider.gameObject;
            }
        }
        return curFloor;
    }

    GameObject getNewTargetFloor()
    {
        GameObject newTargetFloor = null;
        int hitCount;
        RaycastHit2D[] hits = new RaycastHit2D[3];
        hitCount = Physics2D.RaycastNonAlloc(transform.position, direcVector, hits);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Floor" && hit.collider.gameObject != currentFloor)
            {
                newTargetFloor = hit.collider.gameObject;
                //newTargetFloor.SendMessage("showMe");
            }
        }
        return newTargetFloor;
    }

   
}
