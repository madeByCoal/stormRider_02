using UnityEngine;
using System.Collections;

public class SR_Character_weapon : MonoBehaviour
{
	public GameObject bulletPrefab;
    public float brustForce;
    private float fireTimecur;
    public float fireTimeMax;
    public Timer fireCountDown;
    private Vector3 firePoint;
	private Animator anim;
	private GameObject Vehicle;
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
        fireCountDown = new Timer();
        fireTimecur = 0;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetButton("Fire"))
        {
            fireCountDown.CountDown(ref fireTimecur, fireTimeMax, fire);
        }
        else
        {
            fireTimecur = 0;
        }
	}


	void fire ()
	{
		anim.SetTrigger ("fire");
		firePoint = transform.Find("tube").Find("firePoint").gameObject.transform.position;
		GameObject bullet = Instantiate(bulletPrefab,firePoint,transform.rotation)as GameObject;
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		Rigidbody2D vehicleRB = GetComponentInParent<Rigidbody2D>();
		rb.velocity = vehicleRB.velocity;
		rb.AddForce(bullet.transform.up*brustForce,ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player") {
			Debug.Log("yeah!!");
			col.gameObject.GetComponent<SR_Character_Health>().SendMessage("ApplyDamage",10f);
		}
	}
}
