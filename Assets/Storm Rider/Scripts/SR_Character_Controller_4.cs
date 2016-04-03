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
    public float originMoveStep;
    public float stepMutiply;
    private float polarRadius;


    void Awake()
    {
        playerState = PlayerState.Move;
    }
 
    void Start () {
        isometricAngle = Mathf.Rad2Deg * Mathf.Atan2(0.5f, 1f);   //根据x，y坐标比值算出视角
        direcVector = DirectionToVector(moveDirection);
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        GameManager.SendMessage("getPlayerDir", moveDirection);
        anim = GetComponent<Animator>();
        originMoveStep = moveStep;
        polarRadius = calPolarRadius();
    }

    void Update()
    {
        DispalyVelocitys();
        playAnimation();
        changeMoveDirection();
        StepModify();
        //print (Mathf.Pow(3,2));
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
            playerState = PlayerState.Move;
            moveDirection = GetNewDirection(moveDirection,Mathf.CeilToInt(Input.GetAxis("Horizontal")),2);
            direcVector = DirectionToVector(moveDirection);
            GameManager.SendMessage("getPlayerDir", moveDirection);
        }

        if (Input.GetButtonDown ("Slide"))
        {
            playerState = PlayerState.Slide;
            tempDirection = GetNewDirection(moveDirection, Mathf.CeilToInt(Input.GetAxis("Slide")), 1);
            direcVector = DirectionToVector(tempDirection);
            GameManager.SendMessage("getPlayerDir", tempDirection);
            targetFloor = getNewTargetFloor();
        }

        switch (playerState)
        {
            case PlayerState.Move:
                GameManager.SendMessage("getPlayerDir", moveDirection);
                break;
            case PlayerState.Slide:

                break;
            case PlayerState.Die:
                break;
            default:
                break;
        }
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
                if (Input.GetKeyDown("d"))
                {
                    Debug.Log("d");
                    anim.SetTrigger("SE-SW");
                }
               
                break;
            case Direction.SouthWest:
                if (Input.GetKeyDown("a"))
                {
                    Debug.Log("a");
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
        if (targetFloor == null || transform.position == targetFloor.transform.position)
        {
            //if (currentFloor == targetFloor)
            //{

            //}
            switch (playerState)
            {
                case PlayerState.Move:
                    
                    targetFloor = getNewTargetFloor();
                    
                    break;
                case PlayerState.Slide:
                    direcVector = DirectionToVector(moveDirection);             //set back to move data
                    playerState = PlayerState.Move;
                    GameManager.SendMessage("getPlayerDir", moveDirection);
                    targetFloor = getNewTargetFloor();
                    break;
                case PlayerState.Die:
                    break;
                default:
                    break;
            }
        }

       
        transform.position = Vector3.MoveTowards(transform.position, targetFloor.transform.position, Time.deltaTime * moveStep);
    }

    void StepModify()
    {
        if (playerState == PlayerState.Slide)
        {
            if (tempDirection == Direction.East || tempDirection == Direction.West)
            {
                moveStep = originMoveStep;
            }
            else
            {
                moveStep = originMoveStep / 2;
            }
        }
        else if (playerState == PlayerState.Move)
        {
            moveStep = originMoveStep * polarRadius;
        }
    }

    float calPolarRadius()
    {
        float e = Mathf.Sqrt(1 - (Mathf.Pow(0.5f,2) / Mathf.Pow(1,2)));
        float r = 0.5f / Mathf.Sqrt(1 - Mathf.Pow(e,2) * (1 - Mathf.Cos(isometricAngle * 2 * Mathf.Deg2Rad)));
        return r;
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
        if (targetFloor != null)
        {
            targetFloor.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        GameObject newTargetFloor = GameManager.GetComponent<BoardManager>().getTargetFloor(currentFloor);
        return newTargetFloor;
    }
}
