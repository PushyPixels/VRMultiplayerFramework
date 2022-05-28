using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsciiLipSync : MonoBehaviour
{
    public float speakingThreshold = 0.1f;
    public float minBlinkDelay = 1.0f;
    public float maxBlinkDelay = 2.0f;
    public float minBlinkTime = 0.2f;
    public float maxBlinkTime = 0.4f;
    public float minSpeakTime = 0.2f;
    public float maxSpeakTime = 0.4f;

    [Multiline]
    public string neutralFace;

    [Multiline]
    public string blinkingFace;

    [Multiline]
    public List<string> speakingFaces1 = new List<string>();
    public List<string> speakingFaces2 = new List<string>();
    public List<string> whistlingFaces = new List<string>();

    public AudioSource audioSource;
    public TextMesh textMesh;
    private float[] outputData = new float[700];
    private float average;
    private float speakDelay;
    private bool blinking;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Blink",Random.Range(minBlinkDelay,maxBlinkDelay));
    }

    void Blink()
    {
        if(average < speakingThreshold)
        {
            textMesh.text = blinkingFace;
            blinking = true;
            Invoke("Unblink",Random.Range(minBlinkTime,maxBlinkTime));
        }
        else
        {
            Invoke("Blink",Random.Range(minBlinkDelay,maxBlinkDelay));
        }
    }
    void Unblink()
    {
        if(average < speakingThreshold)
        {
            textMesh.text = neutralFace;
        }
        blinking = false;
        Invoke("Blink",Random.Range(minBlinkDelay,maxBlinkDelay));
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetOutputData(outputData,0);
        float sum = 0;
        for(int i = 0; i < outputData.Length; i++)
        {
            sum += Mathf.Abs(outputData[i]);
        }
        average = sum/(outputData.Length + 1);

        if(average > speakingThreshold && speakDelay <= 0.0f)
        {
            textMesh.text = speakingFaces1[Random.Range(0,speakingFaces1.Count)];
            speakDelay = Random.Range(minSpeakTime,maxSpeakTime);
        }
        else if(speakDelay <= 0.0f && !blinking)
        {
            textMesh.text = neutralFace;
        }
        speakDelay -= Time.deltaTime;
    }

    void OnValidate()
    {
        if(!audioSource)
        {
            audioSource = GetComponentInChildren<AudioSource>();
            textMesh = GetComponentInChildren<TextMesh>();
        }
    }
}
