using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using System.IO;

public class AudioInput : MonoBehaviour
{
    public string filename;
    private AudioSource audSource;

	// Use this for initialization
	void Start ()
    {
        audSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        int FFTSize = 256;
        int FFTLen = (int)Mathf.Sqrt(FFTSize);

        float[] spectrum = new float[FFTSize];



        audSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);


        for (int i = 0; i < FFTSize; i += FFTLen)
        {
            Debug.Log("spec: " + string.Join("", new List<float>(spectrum).GetRange(i, i + FFTLen).ConvertAll(s => s.ToString() ).ToArray() ) );
        }
    
    }
}
