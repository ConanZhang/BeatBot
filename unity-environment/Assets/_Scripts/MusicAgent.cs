using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAgent : Agent
{
    float dBVal;
    float freqVal;

    float lowdBval;
    float lowFreqVal;
    float highdBVal;
    float highFreqVal;

    private void Start()
    {
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
        float moodVal = act[0];


        // brain is guessing the value. Clamp its input
        //if(actionVal > 1.0f)
        //{
        //    actionVal = 1.0f;
        //}
        //if(actionVal < 0.0f)
        //{
        //    actionVal = 0.0f;
        //}

        if(moodVal < 0.0f || moodVal > 1.0f)
        {
            // give it a bad reward if it guesses outside the bounds
            reward = -1f;
        }
        else
        {
            float difference = Mathf.Abs(moodVal - GetCorrectMoodVal());

            reward = 1.0f - difference;
        }







    }

    public override void AgentReset()
    {

    }

    public override void AgentOnDone()
    {

    }

    private float GetCorrectMoodVal()
    {
        float correctdBVal = (dBVal - lowdBval) / (highdBVal - lowdBval);
        float correctFreqVal = (freqVal - lowFreqVal) / (highFreqVal - lowFreqVal);

        float correctMoodVal = (correctdBVal + correctFreqVal) / 2.0f;
        return correctMoodVal;
    }

}
