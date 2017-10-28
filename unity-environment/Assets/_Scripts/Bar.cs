using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bar : MonoBehaviour {

    public float testValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        setScaleRate(testValue);
	}

    public void setScaleRate(float value)
    {
        float yScale = value * 6;
        yScale = Mathf.Clamp(yScale, 0.40f, 6f);
        float yRotate = value;
        yRotate = Mathf.Clamp(yRotate, 0.01f, 4f);
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
        transform.Rotate(0, yScale, 0);
    }
}
