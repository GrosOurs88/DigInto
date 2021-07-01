using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationScript : MonoBehaviour
{
    public Vector3 vec;
    public float speed;
        
    void Update()
    {
        if (ClickScript.instance.isTimerRunning == true)
        {
            transform.Translate(vec * speed * Time.deltaTime);
        }
    }
}