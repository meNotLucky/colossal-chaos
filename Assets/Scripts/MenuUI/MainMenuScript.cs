using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.Rendering.PostProcessing;

public class MainMenuScript : MonoBehaviour
{
	public UnityEngine.UI.Button myButton;
	public Slider PlaySlider;
	public Animator animator;
	public PostProcessProfile profile;
	public float timeToChange = 1f;
	private float mouseHeldTimer = 0f;
	private float timer = 0.1f;
	private float colortimer;
	private float originalBloomValue;

void Start()
{
	Cursor.lockState = CursorLockMode.Locked;
	Time.timeScale = 1f;

	if(profile != null){
		originalBloomValue = profile.GetSetting<Bloom>().intensity.value;
	}
}
	void Update()
	{

	if(Input.GetAxisRaw("Mouse X") != 0)
	{
		Cursor.lockState = CursorLockMode.None;
	}

		//the red color is a test for the PlayButton.
		Vector3 mousePos = Input.mousePosition;
		if(mousePos.x > (Screen.width / 2) + (Screen.width / 8))
		{
			mouseHeldTimer += Time.deltaTime;
			//myButton.Select();

			if(animator != null){
				animator.SetBool("Hover", true);
			}

			if(profile != null){
				profile.GetSetting<Bloom>().intensity.value *= 1.012f;
			}

			if(PlaySlider.value < 1f){
				PlaySlider.value += timer / 60;
			}

			if(mouseHeldTimer >= timeToChange){
				if(profile != null){
					profile.GetSetting<Bloom>().intensity.value = originalBloomValue;
				}
				GetComponent<LoadGameScript>().LoadGame();
				Destroy(this);
			}
		}
		else
		{
			if(animator != null){
				animator.SetBool("Hover", false);
			}

			if(profile != null){
				profile.GetSetting<Bloom>().intensity.value = originalBloomValue;
			}

			if(PlaySlider.value >= 0)
				PlaySlider.value -= timer / 60;

			mouseHeldTimer = 0f;
		}

			ColorBlock colors = myButton.colors;
			myButton.colors = colors;
	}

	public void QuitGame()
	{
			Debug.Log("Quit!");
			Application.Quit();
	}
}
