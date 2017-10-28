using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodBar : MonoBehaviour {
    [SerializeField]
    private float moodScale;
    private RectTransform rectTransform;

	// Use this for initialization
	void Start ()
	{
	    rectTransform = GetComponent<RectTransform>();
	    moodScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if (moodScale < 0.5)
	    {
	        rectTransform.anchoredPosition = new Vector2(-220 + (moodScale * 220), rectTransform.anchoredPosition.y);
	    }
	    else
	    {
	        rectTransform.anchoredPosition = new Vector2(220 * moodScale, rectTransform.anchoredPosition.y);
	    }
	}

    public void SetMoodScale(float moodScale)
    {
        this.moodScale = moodScale;
    }
    
}
