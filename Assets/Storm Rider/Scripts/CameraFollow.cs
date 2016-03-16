using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	//public float followSpead;		// How smoothly the camera catches up with it's target movement in the x axis.
	//public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	//public Vector2 minXAndY;        // The minimum x and y coordinates the camera can have.
    private Vector3 camPos;
	private Transform player;		// Reference to the player's transform.
	
	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.Find("player").transform;
	}
	
	void Update ()
	{
		TrackPlayer();
	}
	
	void TrackPlayer ()
	{
		camPos = Vector3.Lerp(transform.position, player.position, Time.deltaTime*3f);
        //camPos.x = Mathf.Clamp(camPos.x, minXAndY.x, maxXAndY.x);
        //camPos.y = Mathf.Clamp(camPos.y, minXAndY.y, maxXAndY.y);
        camPos.z = transform.position.z;
        transform.position = camPos;
    }
}
