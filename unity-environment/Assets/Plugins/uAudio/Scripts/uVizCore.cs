using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uVizCore : MonoBehaviour {
     private float[] _Spectrum;
   public UnityEngine.AudioSource theAudioSource;
    public FFTWindow myFFTWindow = FFTWindow.Rectangular;

    public readonly int _NumSamples = 256;

    public System.Action<float[]> send_data;
    // Use this for initialization
    void Start ()
    {
        _Spectrum = new float[_NumSamples];

        StartCoroutine(updateViz());
        //Timing.RunCoroutine(updateViz());
    }
    System.Collections.IEnumerator updateViz()
    {
        while (true)
        {
            if (send_data != null&& theAudioSource.isPlaying)
                BuildSend();
            yield return null;// new WaitForEndOfFrame();
        }
    }
    // Update is called once per frame
    void BuildSend () {

        theAudioSource.GetSpectrumData(_Spectrum, 0, myFFTWindow);
        send_data(_Spectrum);

    }








#if UNITY_5_3_OR_NEWER
    private float _BaseLeft;
    private float _BaseRight;
#endif


    void updateSpatializerFloat()
    {
#if UNITY_5_3_OR_NEWER
        theAudioSource.GetSpatializerFloat(0, out _BaseLeft);
        theAudioSource.GetSpatializerFloat(1, out _BaseRight);
        //aSliderLeft.value = (_BaseLeft) * 10;
#endif
        //aSliderRight.value = _BaseRight;
    }
}
