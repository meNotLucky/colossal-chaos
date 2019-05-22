using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeKeybinds : MonoBehaviour
{
    public KeyCode restart;
    public KeyCode centerMouse;

    void Update()
    {
        if (Input.GetKeyDown(restart))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if(Input.GetKeyDown(centerMouse)){
            Cursor.lockState = CursorLockMode.Locked;
        } else if(Input.GetAxisRaw("Mouse X") != 0) {
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
