using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript instance;

    public AudioSource audioSource;

    public float bpm;
    private float beatInterval, beatTimer, beatIntervalDivBy8, beatTimerDivBy8;
    public static bool beatFull, beatDivBy8;
    public static int beatCountFull, beatCountDivBy8;

    public GameObject colorCubesParent;
    public List<Color> colorList = new List<Color>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        audioSource.Play();
    }

    void Update()
    {
        BeatDetection();
    }

    void BeatDetection()
    {
        //full beat count
        beatFull = false;
        beatInterval = 60 / bpm;
        beatTimer += Time.deltaTime;

        if(beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatFull = true;
            beatCountFull++;
            //Debug.Log("Full");

            ChangeCubesColor();
        }

        //divided by 8 beat count
        beatDivBy8 = false;
        beatIntervalDivBy8 = beatInterval / 8;
        beatTimerDivBy8 += Time.deltaTime;

        if(beatTimerDivBy8 >= beatIntervalDivBy8)
        {
            beatTimerDivBy8 -= beatIntervalDivBy8;
            beatDivBy8 = true;
            beatCountDivBy8++;
            //Debug.Log("Divided by 8");
        }
    }


    public void ChangeCubesColor()
    {
        int boby = Random.Range(0, colorList.Count);

        foreach (Transform tr in colorCubesParent.transform)
        {
            tr.GetComponent<Renderer>().material.color = colorList[boby];
            tr.GetComponent<Animator>().SetTrigger("Bump");
        }
    }
}
