#pragma warning disable 0414

using UnityEngine;

public class uVisualizer : MonoBehaviour
{
    //private int _NumSamples = 1024;
    public uVizCore myCore;



    private float[] _SamplesLeft, _SamplesRight;
    private float[] _SpectrumLeft, _SpectrumRight;
    //public UnityEngine.AudioSource theAudioSource;
    public UnityEngine.UI.Slider[] SliderList;
    public UnityEngine.UI.Slider[] SliderList2;
    public UnityEngine.UI.Slider aSliderLeft;
    public UnityEngine.UI.Slider aSliderRight;

    //public FFTWindow myFFTWindow = FFTWindow.Rectangular;

    public float Viz_Scale = 1;

    // public System.Collections.Generic.List<Slider> GameObjects;
    // Use this for initialization
    void Start()
    {
           _SamplesLeft = new float[myCore._NumSamples];
        _SamplesRight = new float[myCore._NumSamples];
        _SpectrumLeft = new float[myCore._NumSamples];
        _SpectrumRight = new float[myCore._NumSamples];
        var r = new UnityEngine.UI.Slider[100];
        //int index = 0;
        //int offset = 0;

        //foreach (var t in r)
        //{
        //    var g = Instantiate<UnityEngine.UI.Slider>(aSliderLeft);
        //    var p = aSliderLeft.transform.parent.localPosition;
        //    p.x += offset + 200;
        //    offset += 5;
        //    g.transform.localPosition = p;
        //    g.transform.parent = aSliderLeft.transform.parent;
        //    r[index++] = g;
        //}
        //SliderList = r;

        //StartCoroutine(updateViz());
        myCore.send_data += updateSectrumViz;

    }

    //void LoadGuiControls()
    //{
    //  foreach(GameObject g in  GameObjects)
    //    {
    //        SliderList
    //    }
    //}


    public float[] freqData;
    public float[] band;

    void updateSectrumViz(float[] _Spectrum)
    {
        //theAudioSource.GetOutputData(_SamplesLeft, 0);
        //theAudioSource.GetOutputData(_SamplesRight, 0);
        //theAudioSource.GetSpectrumData(_SpectrumLeft, 0, myFFTWindow);
        //theAudioSource.GetSpectrumData(_SpectrumRight, 1, myFFTWindow);
        //uAudio.SpectrumViz.self.DataReciver(_SpectrumLeft);
        int count = SliderList.Length;
        //int samplesPerItem = Mathf.FloorToInt(myCore._NumSamples / count);
        //int index = 0;
        for (int i = 0; i < count; i++)
        {
            ///   AForge.Math.FourierTransform.FFT(_SamplesLeft, AForge.Math.FourierTransform.Direction.Forward;

            //uViz.convert_FFT c = new uViz.convert_FFT();
            //c.RealFFT(_SamplesLeft, true);
            ////    float d= splitSamples(i, _SpectrumLeft, samplesPerItem);
            float f = Mathf.Clamp(_Spectrum[i] * (50 + i * i)/200, 0, 50);

            float d = f * Viz_Scale;
            SliderList[i].value = d;
            SliderList2[i].value = d;
            //Debug.DrawLine(new Vector3(i11, d * Viz_Scale, 0), new Vector3(i11, -1*d * Viz_Scale, 0), Color.red);

            //index++;
        }
        ////int n = _SpectrumLeft.Length;
        ////    int k = 0;
        ////    for (int j = 0; j < _SpectrumLeft.Length; j++)
        ////    {
        ////        n = n / 2;
        ////        if (n <= 0)
        ////            break;
        ////        k++;
        ////    }

        ////    band = new float[k + 1];


        ////    k = 0;
        ////    int crossover = 2;
        ////    float speed = 5.0f;

        ////    for (int i = 0; i < _SpectrumLeft.Length; i++)
        ////    {
        ////        float d2 = _SpectrumLeft[i];
        ////        float b = band[k];

        ////        // find the max as the peak value in that frequency band.
        ////        band[k] = (d2 > b) ? d2 : b;

        ////        if (i > (crossover - 3))
        ////        {
        ////            k++;
        ////            crossover *= 2;   // frequency crossover point for each band.
        ////            SliderList[k].value = band[k] * 32;
        ////            //Vector3 tmp = new Vector3(g[k].transform.position.x, band[k] * 32, g[k].transform.position.z);
        ////            //g[k].transform.position = Vector3.Lerp(g[k].transform.position, tmp, speed);
        ////            band[k] = 0;
        ////        }
        ////    }



    }


    float splitSamples(int index, float[] samples, int samplesPerItem)
    {
        int offset = index * samplesPerItem;
        float output = 0;
        for (int i = 0; i < samplesPerItem; i++)
        {
            output += samples[offset + i];
        }
        output = GetYPosLog(output / samplesPerItem);
        //output = Mathf.Log(output / samplesPerItem*1000,100f)*1000;
        return 1 - output;
    }

    private float GetYPosLog(float c)
    {
        // not entirely sure whether the multiplier should be 10 or 20 in this case.
        // going with 10 from here http://stackoverflow.com/a/10636698/7532
        float intensityDB = 10 * Mathf.Log10(c);
        float minDB = -90;
        if (intensityDB < minDB) intensityDB = minDB;
        float percent = intensityDB / minDB;
        // we want 0dB to be at the top (i.e. yPos = 0)
        float yPos = percent;
        return yPos;
    }
}