using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSkybox : MonoBehaviour
{
    private float moodScale;
    private float previousMoodScale;
    public Color[] Color1;
    public Color[] Color2;
    public Color[] Color3;

	// Use this for initialization
	void Start ()
	{
	    previousMoodScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	    if (moodScale > previousMoodScale)
	    {
            RenderSettings.skybox.SetColor("_Color1", Color.Lerp(Color1[0], Color1[1], moodScale));
            RenderSettings.skybox.SetColor("_Color2", Color.Lerp(Color2[0], Color2[1], moodScale));
            RenderSettings.skybox.SetColor("_Color3", Color.Lerp(Color3[0], Color3[1], moodScale));
	    }
	    else
	    {
            RenderSettings.skybox.SetColor("_Color1", Color.Lerp(Color1[1], Color1[0], moodScale));
            RenderSettings.skybox.SetColor("_Color2", Color.Lerp(Color2[1], Color2[0], moodScale));
            RenderSettings.skybox.SetColor("_Color3", Color.Lerp(Color3[1], Color3[0], moodScale));
        }
	}

    public void SetMoodScale(float moodScale)
    {
        previousMoodScale = this.moodScale;
        this.moodScale = moodScale;
    }
    
    void OnApplicationQuit()
    {
        RenderSettings.skybox.SetColor("_Color1", Color1[0]);
        RenderSettings.skybox.SetColor("_Color2", Color2[0]);
        RenderSettings.skybox.SetColor("_Color3", Color3[0]);
    }

}
