using UnityEngine;
using System.Collections;

public class SR_LayerController : MonoBehaviour 
{
	public float enlarge = 10f;
	//too small for making the y to sprite layer.so 10 time larger...

	// Update is called once per frame
	void Update () 
	{
		gameObject.GetComponent<Renderer>().sortingOrder = (int)(gameObject.transform.position.y * enlarge);
	}
}
