using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : Singleton<MoodManager> 
{
    AudioSource audioSource;
    [Header("Clamping dB values")]
    public float min_dB = -25.0f;
    public float max_dB = 25.0f;

    [Header("The all seeing, all knowing mood value that we must edit")]
    public float MoodValue = 0.5f;

    private int qSamples = 1024;
    private float refRmsVal = 0.1f; // represents the rms value that corresponds to 0 dB
    private float freqMinThreshold = 0.02f;

    private float rmsVal;
    private float dBVal;
    private float pitchVal;

    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        samples = new float[qSamples];
        spectrum = new float[qSamples];
        sampleRate = AudioSettings.outputSampleRate;
    }
    private void AnalyzeSound()
    {
        audioSource.GetOutputData(samples, 0);
        float sumSquares = 0.0f;

        // Sum up the square of all of the samples
        for (int i = 0; i < qSamples; i++)
        {
            sumSquares += Mathf.Pow(samples[i], 2);
        }

        rmsVal = Mathf.Sqrt(sumSquares / qSamples);
        dBVal = 20 * Mathf.Log10(rmsVal / refRmsVal);

        dBVal = Mathf.Clamp(dBVal, min_dB, max_dB);
        Debug.Log("dB val: " + dBVal);
    }

    private void Update()
    {
        AnalyzeSound();
        MoodValue = (dBVal + Mathf.Abs(min_dB)) / (max_dB + Mathf.Abs(min_dB));
    }

}
