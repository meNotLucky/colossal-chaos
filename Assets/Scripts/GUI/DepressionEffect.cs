using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class DepressionEffect : MonoBehaviour
{
    [Header("PP-Reference")]
    public GameObject postProcessing;
    private PostProcessProfile profile;
    private ColorGrading color;
    private Grain grain;
    private Vignette vignette;

    void Start()
    {
        profile = postProcessing.GetComponent<PostProcessVolume>().profile;

        color = profile.GetSetting<ColorGrading>();
        grain = profile.GetSetting<Grain>();
        vignette = profile.GetSetting<Vignette>();
    }

    void Update()
    {
        color.saturation.value = 0 - GetComponent<LooseCondition>().destroyedHouses;

        if(color.saturation.value > 0)
            color.saturation.value = 0;
    }
}
