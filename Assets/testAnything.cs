using UnityEngine;
using System.Collections;

public class testAnything : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        print(Mathf.Tan(Mathf.Atan2(1,2)));
    }
}
