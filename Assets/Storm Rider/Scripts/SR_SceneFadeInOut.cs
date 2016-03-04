using UnityEngine;
using System.Collections;

public class SR_SceneFadeInOut : MonoBehaviour
{
	public float fadeSpeed = 5f;			// Speed that the screen fades to and from black.
	
	private bool sceneStarting = true;		// Whether or not the scene is still fading in.
	

	void Update ()
	{
		// If the scene is starting...
		if(sceneStarting)
			// ... call the StartScene function.
			StartScene();
	}
	
	
	/*void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}*/
	
	
	void StartScene ()
	{

		sceneStarting = false;
	}
	
	
	public void EndScene ()
	{
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
