using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDecision : MonoBehaviour, Decision
{

    public float[] Decide(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[4]{ 0f, 0f, 0f, 0.0f };
    }

    public float[] MakeMemory(List<float> state, List<Camera> observation, float reward, bool done, float[] memory)
    {
        return new float[0];
    }
}