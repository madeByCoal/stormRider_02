using UnityEngine;
using System.Collections;

public class Timer {

	public delegate void Method();


	public void CountDown(ref float timer, float MaxTime,  Method methodName)
	{
		if (timer <= 0) {
			methodName();
			timer = MaxTime;
		}
        timer -= Time.deltaTime;
    }
}
