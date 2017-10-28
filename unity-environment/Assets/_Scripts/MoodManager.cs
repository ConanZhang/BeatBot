using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MoodManager : Agent
{
    public enum MoodManagerState
    {
        SamplingInput,
        Learning
    }

    AudioSource audioSource;
    [Header("State this object is in")]
    public MoodManagerState State = MoodManagerState.SamplingInput;

    [Header("The all seeing, all knowing mood value that we must edit")]
    public static float MoodValue = 0.5f;

    [Header("File paths for output data files")]
    public string lowValueFilePath = "Assets/_Data/lowVals.txt";
    public string highValueFilePath = "Assets/_Data/highVals.txt";

    private int qSamples = 1024;
    private float refRmsVal = 0.1f; // represents the rms value that corresponds to 0 dB
    private float minSpectrumVal = 0.02f;

    private float rmsVal;

    private float dBVal;
    private float freqVal;

    private float[] samples;
    private float[] spectrum;
    private float sampleRate;

    private float lowdBVal;
    private float lowFreqVal;
    private float highdBVal;
    private float highFreqVal;

    // Lists of high and low values stored in format dB, Hz 
    private List<Tuple<float, float>> highValues;
    private List<Tuple<float, float>> lowValues;

    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();

        highValues = new List<Tuple<float, float>>();
        lowValues = new List<Tuple<float, float>>();

        samples = new float[qSamples];
        spectrum = new float[qSamples];
        sampleRate = AudioSettings.outputSampleRate;

        if(State == MoodManagerState.Learning)
        {
            StreamReader lowValueReader = new StreamReader(lowValueFilePath);
            StreamReader highValueReader = new StreamReader(highValueFilePath);

            lowdBVal = float.Parse(lowValueReader.ReadLine());
            lowFreqVal = float.Parse(lowValueReader.ReadLine());

            lowValueReader.Close();

            highdBVal = float.Parse(highValueReader.ReadLine());
            highFreqVal = float.Parse(highValueReader.ReadLine());

            highValueReader.Close();


            
        }
    }
    private void AnalyzeSound()
    {
        // CALCULATING dB

        audioSource.GetOutputData(samples, 0);
        float sumSquares = 0.0f;

        // Sum up the square of all of the samples
        for (int i = 0; i < qSamples; i++)
        {
            sumSquares += Mathf.Pow(samples[i], 2);
        }

        rmsVal = Mathf.Sqrt(sumSquares / qSamples);
        dBVal = 20 * Mathf.Log10(rmsVal / refRmsVal);

        // calculates a dB val clamped between low and high defined points
        //dBVal = Mathf.Clamp(dBVal, min_dB, max_dB);
        Debug.Log("dB val: " + dBVal);

        // CALCULATING Hz

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxSpectrumVal = 0.0f;
        int maxSpectrumIndex = 0;
        for (int i = 0; i < qSamples; i++)
        {
            // If you've found a new max value, then assign the max index and spectrum 
            if(spectrum[i] > maxSpectrumVal && spectrum[i] > minSpectrumVal)
            {
                maxSpectrumVal = spectrum[i];
                maxSpectrumIndex = i;
            }
        }

        // interpolating between neighboring values
        float frequencyAtIndex = maxSpectrumIndex;
        if(maxSpectrumIndex > 0 && maxSpectrumIndex < qSamples - 1)
        {
            float dL = spectrum[maxSpectrumIndex - 1] / spectrum[maxSpectrumIndex];
            float dR = spectrum[maxSpectrumIndex + 1] / spectrum[maxSpectrumIndex];
            frequencyAtIndex += 0.5f * (dR * dR - dL * dL);
        }

        // calculate the dominant frequency at that frame
        freqVal = frequencyAtIndex * (sampleRate / 2) / qSamples;
        Debug.Log("Hz val: " + freqVal);
    }

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();

        state.Add(dBVal);
        state.Add(freqVal);

        return state;
    }
    public override void AgentStep(float[] act)
    {
        if(State == MoodManagerState.Learning)
        {
            float moodVal = act[0];

            MoodValue = moodVal;

            if (moodVal < 0.0f)
            {
                // give it a bad reward if it guesses outside the bounds
                reward = moodVal;
            }
            else if (moodVal > 1.0f)
            {
                reward = -moodVal;
            }
            else
            {
                float difference = Mathf.Abs(moodVal - GetCorrectMoodVal());

                reward = 1.0f - difference;
            }

        }
    }

    private void CaptureHighSound()
    {
        highValues.Add(new Tuple<float, float>(dBVal, freqVal));
    }
    private void CaptureLowSound()
    {
        lowValues.Add(new Tuple<float, float>(dBVal, freqVal));
    }

    private float GetCorrectMoodVal()
    {
        float correctdBVal = (dBVal - lowdBVal) / (highdBVal - lowdBVal);
        float correctFreqVal = (freqVal - lowFreqVal) / (highFreqVal - lowFreqVal);

        float correctMoodVal = (correctdBVal + correctFreqVal) / 2.0f;
        return correctMoodVal;
    }

    private void Update()
    {
        AnalyzeSound();
        //MoodValue = (dBVal - min_dB) / (max_dB - min_dB);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            CaptureHighSound();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            CaptureLowSound();
        }
    }

    private void OnApplicationQuit()
    {
        if (State == MoodManagerState.SamplingInput)
        {

            Debug.Log("Low size: " + lowValues.Count);
            Debug.Log("High size: " + highValues.Count);

            StreamWriter lowValueWriter = new StreamWriter(lowValueFilePath, false);
            StreamWriter highValueWriter = new StreamWriter(highValueFilePath, false);

            float avgLowFreq, avgHighFreq;
            float avgLowdB, avgHighdB;
            float freqSum = 0.0f, dBSum = 0.0f;

            // Calculate the average for low values
            foreach (Tuple<float, float> pair in lowValues)
            {
                dBSum += pair.Item1;
                freqSum += pair.Item2;
            }
            avgLowdB = dBSum / lowValues.Count;
            avgLowFreq = freqSum / lowValues.Count;

            Debug.Log("avg low: " + avgLowdB + " " + avgLowFreq);

            // Writing values into files
            lowValueWriter.WriteLine(avgLowdB);
            lowValueWriter.WriteLine(avgLowFreq);

            lowValueWriter.Close();

            AssetDatabase.ImportAsset(lowValueFilePath);

            dBSum = 0.0f;
            freqSum = 0.0f;

            // Calculate the average for high values
            foreach (Tuple<float, float> pair in highValues)
            {
                dBSum += pair.Item1;
                freqSum += pair.Item2;
            }
            avgHighdB = dBSum / highValues.Count;
            avgHighFreq = freqSum / highValues.Count;

            Debug.Log("avg high: " + avgHighdB + " " + avgHighFreq);

            highValueWriter.WriteLine(avgHighdB);
            highValueWriter.WriteLine(avgHighFreq);

            highValueWriter.Close();

            AssetDatabase.ImportAsset(highValueFilePath);

        }
    }

}
