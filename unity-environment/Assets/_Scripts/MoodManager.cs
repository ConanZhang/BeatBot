using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : Singleton<MoodManager> 
{
    [Header("The all seeing, all knowing mood value that we must edit")]
    public float MoodValue = 0.5f;
}
