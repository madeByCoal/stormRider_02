using UnityEngine;
using System.Collections;

public class bulletDamage : MonoBehaviour {
    public GameObject bulletSparkPreFab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player"&&col.gameObject.name!="player") {
			col.gameObject.SendMessage("ApplyDamage",1f);
		}
        else if (col.gameObject.tag == "Obstacle")
        {
            //cast ray to get position where those bullets collide
            Vector2 originPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = new Vector2(transform.up.x, transform.up.y);
            RaycastHit2D bulletHit = Physics2D.Raycast(originPos, direction);
            if (bulletHit.collider != null)
            {
                GameObject bulletSpark = Instantiate(bulletSparkPreFab, bulletHit.point, transform.rotation) as GameObject;
                Destroy(gameObject);
            }
        }
    }
}
