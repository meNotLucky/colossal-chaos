using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

	public UnityEngine.UI.Button myButton;
	public Slider PlaySlider;
	public float timeToChange = 1f;
	public float mouseHeldTimer = 0f;
	public float timer = 0.1f;
	public float colortimer;

void Start()
{
	Cursor.lockState = CursorLockMode.Locked;
	Time.timeScale = 1f;
}
	void Update()
	{

	if(Input.GetAxisRaw("Mouse X")!=0)
	{
		Cursor.lockState = CursorLockMode.None;
	}

		//the red color is a test for the PlayButton.
		Vector3 mousePos = Input.mousePosition;
		if(mousePos.x > (Screen.width / 2) + (Screen.width / 8))
		{
			mouseHeldTimer += Time.deltaTime;
			//myButton.Select();


			if(PlaySlider.value < 1f){
				PlaySlider.value += timer / 60;
			}

			if(PlaySlider.value >= 1){
				GetComponent<LoadGameScript>().LoadGame();
				Destroy(this);
			}
		}
		else
		{

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
