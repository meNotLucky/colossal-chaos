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
	public ParticleSystem particle;
	public PostProcessProfile profile;
	public float timeToChange = 1f;
	public float bloomStartValue;
	public float bloomIntensityIncrease;
	private float mouseHeldTimer = 0f;
	private float timer = 0.1f;
	private float colortimer;

void Start()
{
	Cursor.lockState = CursorLockMode.Locked;

	Time.timeScale = 1f;

	if(profile != null){
		profile.GetSetting<Bloom>().intensity.value = bloomStartValue;
	}
}
	void FixedUpdate()
	{

		if(Input.GetAxisRaw("Mouse X") != 0)
			Cursor.lockState = CursorLockMode.None;

		//the red color is a test for the PlayButton.
		Vector3 mousePos = Input.mousePosition;
		if(mousePos.x < (Screen.width / 2) - 20f)
		{
			mouseHeldTimer += Time.deltaTime;
			//myButton.Select();

			if(animator != null)
				animator.SetBool("Hover", true);
			if(profile != null)
				profile.GetSetting<Bloom>().intensity.value *= bloomIntensityIncrease;
			if(particle != null)
				particle.gameObject.SetActive(true);

			if(PlaySlider.value < 1f){
				PlaySlider.value += timer / 60;
			}

			if(mouseHeldTimer >= timeToChange){
				if(profile != null){
					profile.GetSetting<Bloom>().intensity.value = bloomStartValue;
				}
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
		else
		{
			if(animator != null)
				animator.SetBool("Hover", false);
			if(profile != null)
				profile.GetSetting<Bloom>().intensity.value = bloomStartValue;
			if(particle != null)
				particle.gameObject.SetActive(false);

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
