using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public void SetMinMaxValue(float min, float max){
        GetComponent<Slider>().minValue = min;
        GetComponent<Slider>().maxValue = max;
    }

    public void SetValue(float pValue) { GetComponent<Slider>().value = pValue; }
    
}
