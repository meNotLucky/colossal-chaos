using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [System.Serializable]
    public class PopUpClass
    {
        public string name;
        public GameObject popUpObject;
    }

    [Header("Properties")]
    [SerializeField] PopUpClass[] popUps;

    public void ActivatePopUp(string name, float timeToHide){
        if(timeToHide > 0)
            StartCoroutine(HideAfterTime(name, timeToHide));

        foreach (var popUp in popUps)
        {
            if(popUp.name == name){
                popUp.popUpObject.SetActive(true);
                return;
            }
        }

        Debug.LogError("No PopUp with that name seems to exist! Check Menus/PopUps -> PopUps");
    }

    public void DeactivatePopUp(string name){
        foreach (var popUp in popUps)
        {
            if(popUp.name == name){
                popUp.popUpObject.SetActive(false);
                return;
            }
        }

        Debug.LogError("No PopUp with that name seems to exist! Check Menus/PopUps -> PopUps");
    }

    private IEnumerator HideAfterTime(string name, float time) {
        yield return new WaitForSeconds(time);
        DeactivatePopUp(name);
    }
}
