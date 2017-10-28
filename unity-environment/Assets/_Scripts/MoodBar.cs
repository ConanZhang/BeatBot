using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodBar : MonoBehaviour
{
    [SerializeField]
    private float moodScale;
    private RectTransform rectTransform;
    private Vector2 initalPosition;

    [SerializeField]
    private RectTransform LeftEndPos;
    [SerializeField]
    private RectTransform RightEndPos;

	// Use this for initialization
	void Start ()
	{
	    rectTransform = GetComponent<RectTransform>();
        initalPosition = rectTransform.anchoredPosition;
	    moodScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetMoodScale(MoodManager.MoodValue);
        rectTransform.anchoredPosition = Vector2.Lerp(LeftEndPos.anchoredPosition, RightEndPos.anchoredPosition, moodScale);
	}

    public void SetMoodScale(float moodScale)
    {
        this.moodScale = moodScale;
    }
    
}
