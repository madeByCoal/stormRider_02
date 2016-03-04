using UnityEngine;
using System.Collections;

public class SR_Sound_Controller : MonoBehaviour 
{
	public AudioClip idleSound;
	public AudioClip moveSound;
	public AudioClip driftSound;
	public SR_Character_Controller_3 playerControler;


	private AudioSource idleAudio;
	private AudioSource driveAudio;
	private AudioSource slipAudio;

	private float maxRolloffDistance = 500f;

	void Awake ()
	{
		playerControler = gameObject.GetComponent<SR_Character_Controller_3> ();

		idleAudio = gameObject.AddComponent<AudioSource> ();
		idleAudio.loop = true;
		idleAudio.clip = idleSound;
		idleAudio.Play ();


		driveAudio = gameObject.AddComponent<AudioSource> ();
		driveAudio.loop = true;
		driveAudio.clip = moveSound;
		driveAudio.Play ();

		slipAudio = gameObject.AddComponent<AudioSource> ();
		slipAudio.loop = true;
		slipAudio.clip = driftSound;
		slipAudio.Play ();
	}

	void Update ()
	{
		StateMachine();
	}

	void StateMachine()
	{
		switch (playerControler.vState) 
		{
		case VehicleState.Idle:
			ChangeAudio(idleAudio);
			break;
		case VehicleState.Drive:
			ChangeAudio(driveAudio);
			break;
		case VehicleState.Slip:
			ChangeAudio(slipAudio);
			break;
        case VehicleState.Die:
            AudioSource[] audioList = { idleAudio, driveAudio, slipAudio };
            foreach (AudioSource audio in audioList)
            {
                audio.volume = 0;
            }
            break;
		default:
			break;
		}
	}

	void ChangeAudio(AudioSource toAudio)
	{
		AudioSource[] audioList = {idleAudio,driveAudio,slipAudio};
		foreach (AudioSource audio in audioList) {
			if (audio == toAudio) {
				audio.volume = 1;
			}
			else {
				audio.volume = 0;
			}
		}
	}
	
	private AudioSource SetUpEngineAudioSource(AudioClip clip)
	{
		// create the new audio source component on the game object and set up its properties
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = 0;
		source.loop = true;
		
		// start the clip from a random point
		source.time = Random.Range(0f, clip.length);
		source.Play();
		source.minDistance = 5;
		source.maxDistance = maxRolloffDistance;
		source.dopplerLevel = 0;
		return source;
	}
}


