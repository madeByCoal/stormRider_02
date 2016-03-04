using UnityEngine;
using System.Collections;

public enum VehicleType
{
    Moto,
    SpeedRace,
    AWD,
    Tank
}

public enum VehicleState
{
    Idle,
    Drive,
    Slip,
    Die
}

public class SR_Character_Controller_3 : MonoBehaviour
{
    public int dustCount = 0;
    public VehicleState vState = VehicleState.Idle;
    public SR_Wheel2D[] wheel2D;
    public Vector3 inputDir;
    public Vector3 MoveDir;
    public GameObject DustTrail;
    public float moveForce;
    public float steerAngle = 0;
    public float steerAngleMax;
    //public float turnSpeed;
    private Animator anim;
    private float liftFront;
    private float liftRear;
    private float breakForce;
    //public float breakForceMax;
    float weight;
    private float weightFront;
    private float weightRear;
    private int wheelCountFront = 0;
    private int wheelCountRear = 0;
    public float heightOfCG = 10;
    public float wheelBase;                             //轴距
    public float disFront = 0.5f;                   //最前端到重心的重量分布（百分比）
    private float disRear;
    private float coeff_friction = 1.3f;
    private Rigidbody2D rb;
    private float timer = 0.05f;
    public bool gearReverse = false;

    void Awake()
    {
        Countwheels();
        rb = GetComponent<Rigidbody2D>();
        weight = rb.mass * 0.98f;
        disRear = 1 - disFront;
    }

    void Update()
    {
        ChangeRotation();
        GearReverse();
        StateMechine();
    }


    void StateMechine()
    {
        if (GetComponent<SR_Character_Health>().health > 0)
        {
            vState = VehicleState.Idle;
            if (Input.GetButton("Break"))
            {
                vState = VehicleState.Slip;
            }
            else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                vState = VehicleState.Drive;
            }
        }
        else
        {
            vState = VehicleState.Die;
        }

        switch (vState)
        {
            case VehicleState.Idle:
                break;
            case VehicleState.Drive:
                breakForce = 0f;
                ChangeMoveForce(weight);
                WeightTransfer();
                AddForce();
                break;
            case VehicleState.Slip:
                //ChangeMoveForce(weight*0.8f);
                WeightTransfer();
                HandBreak();
                ChangeMoveForce(moveForce - breakForce);
                AddForce();
                break;
            default:
                break;
        }
    }

    void HandBreak()
    {
        breakForce = Mathf.Lerp(breakForce, weight / 10f, 0.5f);
        Timer t = new Timer();
        t.CountDown(ref timer, 0.05f, CreateDustTrail);
        //moveForce = Mathf.Lerp (moveForce, moveForce*0.8f, Time.deltaTime);
        //rb.AddForce (-transform.up * breakForce);
        for (int i = 0; i < wheel2D.Length; i++)
        {
            switch (wheel2D[i].wheelType)
            {
                case WheelType.steeringWheel:
                    wheel2D[i].AddSideFriction(rb.velocity);
                    break;
                case WheelType.drivingWheel:
                    wheel2D[i].AddSideFriction(-rb.velocity * 2);
                    break;
                default:
                    break;
            }
        }
    }

    void Countwheels()
    {
        for (int i = 0; i < wheel2D.Length; i++)
        {
            switch (wheel2D[i].wheelType)
            {
                case WheelType.steeringWheel:
                    wheelCountFront++;
                    break;
                case WheelType.drivingWheel:
                    wheelCountRear++;
                    break;
                default:
                    break;
            }
        }
    }

    void WeightTransfer()
    {
        liftFront = (disFront * weight + breakForce * heightOfCG / wheelBase) / wheelCountFront;
        liftRear = (disRear * weight - breakForce * heightOfCG / wheelBase) / wheelCountRear;
        weightFront = liftFront;
        weightRear = liftRear;

        for (int i = 0; i < wheel2D.Length; i++)
        {
            switch (wheel2D[i].wheelType)
            {
                case WheelType.steeringWheel:
                    wheel2D[i].maxFriction = coeff_friction * weightFront;
                    break;
                case WheelType.drivingWheel:
                    wheel2D[i].maxFriction = coeff_friction * weightRear;
                    break;
                default:
                    break;
            }
        }
        //print ("LF = " + liftFront.ToString () + ",LR = " + liftRear.ToString ());
    }

    void ChangeRotation()
    {
        inputDir.x = Input.GetAxis("Horizontal");
        inputDir.y = Input.GetAxis("Vertical");
        float targetAngle = 0;
        if (gearReverse)
        {
            MoveDir = transform.InverseTransformDirection(-inputDir);//将世界坐标转化为local坐标
            targetAngle = Mathf.Atan2(MoveDir.x, MoveDir.y) * Mathf.Rad2Deg;
        }
        else
        {
            MoveDir = transform.InverseTransformDirection(inputDir);//将世界坐标转化为local坐标
            targetAngle = Mathf.Atan2(-MoveDir.x, MoveDir.y) * Mathf.Rad2Deg;
        }

        Debug.DrawRay(transform.position, MoveDir, Color.green);

        for (int i = 0; i < wheel2D.Length; i++)
        {
            if (wheel2D[i].wheelType == WheelType.steeringWheel)
            {
               
                steerAngle = Mathf.Lerp(steerAngle, targetAngle,Time.deltaTime);
                steerAngle = Mathf.Clamp(steerAngle, -steerAngleMax, steerAngleMax);
                wheel2D[i].steerAngle = steerAngle;
            }
        }
    }

    void ChangeMoveForce(float executeForce)
    {
        moveForce = Mathf.Lerp(moveForce, executeForce, Time.deltaTime);
    }

    void AddForce()
    {
        for (int i = 0; i < wheel2D.Length; i++)
        {
            switch (wheel2D[i].wheelType)
            {
                case WheelType.steeringWheel:
                    AddForceToWheel(wheel2D[i],weightFront);
                    break;
                case WheelType.drivingWheel:
                    AddForceToWheel(wheel2D[i], weightRear);
                    break;
                default:
                    break;
            }
        }

        //rb.drag = rb.velocity.magnitude/5;
        //rb.angularDrag = Mathf.Abs(rb.angularVelocity)/20;
        //rb.angularVelocity = Mathf.Lerp(rb.angularVelocity,0,1f);
        //Debug.DrawRay (transform.position, rb.velocity, Color.blue);
    }

    void GearReverse()
    {
        if (Input.GetButtonDown("Reverse"))
        {
            gearReverse = !gearReverse;
            for (int i = 0; i < wheel2D.Length; i++)
            {
                //wheel2D[i].gearRevers = !wheel2D[i].gearRevers;
            }
        }
    }

    void AddForceToWheel(SR_Wheel2D wheel2D, float forceClampMax)
    {
        Vector3 forceDirection;
        moveForce = Mathf.Clamp(moveForce, 0, forceClampMax);
        if (gearReverse == true)
        {
            forceDirection = -wheel2D.transform.up;
            //moveForce /= 2;
        }
        else
        {
            forceDirection = wheel2D.transform.up;
        }
        rb.AddForceAtPosition(forceDirection * moveForce, wheel2D.transform.position, ForceMode2D.Impulse);
    }

    void CreateDustTrail()
    {
        GameObject newDustTrail = Instantiate(DustTrail, gameObject.transform.FindChild("trailPoint").position, Quaternion.identity) as GameObject;
        Rigidbody2D dustRB = newDustTrail.GetComponent<Rigidbody2D>();
        dustRB.AddForce(-transform.up * Mathf.Clamp(rb.velocity.magnitude, 3, Mathf.Infinity));
        dustRB.AddTorque(-rb.angularVelocity);
        dustCount++;
        Destroy(newDustTrail, 5);
    }
}





