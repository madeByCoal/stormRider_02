using UnityEngine;
using System.Collections;

public class SR_HeightSys : MonoBehaviour 
{
	public int floorCount = 0;
	private float timer;
	public float resetAfterFallTime;	
	private GameObject shadow;

	public SR_LayerController layerContorl;


	void Awake ()
	{
		layerContorl = gameObject.GetComponent<SR_LayerController>();
		shadow = GameObject.Find ("shadow");
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Ground") 
		{
			floorCount += 1;
			Debug.Log ("enter");
		} 
	}
	

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Ground") 
		{
			floorCount -= 1;
			Debug.Log ("exit");
		} 
	}

	void Update ()
	{

		if (floorCount <= 0) 
		{
			falling ();
			LevelReset ();
		}
	}


	void falling ()
	{

		gameObject.GetComponent<Renderer>().sortingLayerName = "Ground";
		Debug.Log ("falling");

		layerContorl.enabled = false;
		GetComponent<SR_Character_Controller_2>().enabled = false;
		shadow.GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Rigidbody2D>().gravityScale = 2;

	}

	void LevelReset ()
	{
		timer += Time.deltaTime * 5;
		if (timer >= resetAfterFallTime)
		{
			Debug.Log ("LevelReset");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}
}
