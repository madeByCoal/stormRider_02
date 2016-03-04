using UnityEngine;
using System.Collections;

public enum WheelType
{
    steeringWheel,
    drivingWheel
}

public class SR_Wheel2D : MonoBehaviour
{
    public WheelType wheelType = WheelType.drivingWheel;
    public Vector3 LateralVelocity { get; set; }
    private Animator anim;
    private float LVMax = 0;
    private float breakForce;
    public float maxFriction { get; set; }
    private Rigidbody2D vehicleRigidbody;

    public float moveForce { get; set; }
    private float turnAmount;
    public Vector3 LinearVelocity { get; set; }

    public float brakeForce { get; set; }

    public float steerAngle { get; set; }
    public bool gearRevers = false;
    void Start()
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            if (parent.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                vehicleRigidbody = parent.gameObject.GetComponent<Rigidbody2D>();
                turnAmount = Mathf.Clamp(vehicleRigidbody.angularDrag, 1, Mathf.Infinity);

                break;
            }
            parent = parent.parent;
        }
        if (vehicleRigidbody == null)
        {
            Debug.LogError(this.ToString() + ": Unable to find associated Rigidbody2D.");
        }
    }

    void Update()
    {
        wheelTypeSwitcher();
    }

    void wheelTypeSwitcher()
    {
        switch (wheelType)
        {
            case WheelType.steeringWheel:
                ApplySteering();
                GetLinearVelocity();
                AddSideFriction(1 + Mathf.Abs(steerAngle));
                break;
            case WheelType.drivingWheel:
                GetLinearVelocity();
                AddSideFriction();
                break;
            default:
                break;
        }
        DispalyVelocitys();
    }

    void ApplySteering()
    {
        transform.localEulerAngles = new Vector3(0, 0, steerAngle);
    }

    void GetLinearVelocity()
    {
        LinearVelocity = vehicleRigidbody.velocity;
    }

    void GetLateralVelocity(Vector3 m_LinearVelocity)
    {
        if (gearRevers == true)
        {
            LateralVelocity = Vector3.Dot(-transform.right, m_LinearVelocity.normalized) * -transform.right;
        }
        else
        {
            LateralVelocity = Vector3.Dot(transform.right, m_LinearVelocity.normalized) * transform.right;// * Mathf.Clamp(LinearVelocity.magnitude, -maxFriction, maxFriction);
        }
    }

    void AddSideFriction()
    {
        GetLateralVelocity(LinearVelocity);
        LateralVelocity *= Mathf.Clamp(LinearVelocity.magnitude, -maxFriction, maxFriction);
        vehicleRigidbody.AddForceAtPosition(-LateralVelocity, transform.position);
    }

    void AddSideFriction(float slipAmout)
    {
        GetLateralVelocity(LinearVelocity);
        //LateralVelocity *= slipAmout;
        vehicleRigidbody.AddForceAtPosition(-LateralVelocity, transform.position);
    }

    public void AddSideFriction(Vector3 velocity)
    {
        GetLateralVelocity(velocity);
        vehicleRigidbody.AddForceAtPosition(-velocity * turnAmount, transform.position);
    }

    void DispalyVelocitys()
    {
        Debug.DrawRay(transform.position, LinearVelocity, Color.cyan);
        Debug.DrawRay(transform.position, -LateralVelocity, Color.red);
    }

    void CheckMaxLaterVelocity()
    {
        if (LVMax < Mathf.Abs(LateralVelocity.magnitude))
        {
            LVMax = Mathf.Abs(LateralVelocity.magnitude);
        }
        Debug.Log(gameObject.name + ": " + LVMax);
    }
}
