using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationVector;

    private float time;

    public AnimationCurve moveX;
    public AnimationCurve moveY;
    public AnimationCurve moveZ;

    void Update()
    {
        if (ClickScript.instance.isTimerRunning == true)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Rotate(rotationVector.x * moveX.Evaluate(time) * Time.deltaTime,
                         rotationVector.y * moveY.Evaluate(time) * Time.deltaTime,
                         rotationVector.z * moveZ.Evaluate(time) * Time.deltaTime);

        time += Time.deltaTime;
    }
}
