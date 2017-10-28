using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAgent : Agent{

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        return state;
    }

    // to be implemented by the developer
    public override void AgentStep(float[] act)
    {
    }

    // to be implemented by the developer
    public override void AgentReset()
    {
    }
}
