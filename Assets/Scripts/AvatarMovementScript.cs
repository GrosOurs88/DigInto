using System.Collections;
using UnityEngine;
using PathCreation.Examples;

public class AvatarMovementScript : MonoBehaviour
{
    [SerializeField]
    private float digSpeed;
    private float actualDigSpeed;

    private bool isAscending = false;
    public float ascendSpeed;

    private bool isDigging = false;

    private float raycastDistance;
    [SerializeField]
    private float raycastDistanceOffset;

    public LayerMask layersToDig; 

    private PathFollower pathFollower;

    private float rotationSpeed;

    private Vector3 stopPoint;
    private Vector3 dirToAscend;


    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
        rotationSpeed = pathFollower.speed;


        StopDig();
        CalculateDigRaycastDistance();
        StartRotate();
    }


    void Update()
    {
        //If press button
        if(Input.GetMouseButtonDown(0) && isAscending == false)
        {
            StopRotate();
            RegisterStopPoint();
            StartDig();
        }

        //If release button
        if (Input.GetMouseButtonUp(0) && isDigging == true)
        {
            StopDig();
            Ascend();
        }

        if(isAscending)
        {
            if (Vector3.Distance(stopPoint, transform.position) <= 0.5f)
            {
                pathFollower.isLinkedToThePath = true;
                isAscending = false;
                isDigging = false;
                StartRotate();
            }
            else
            {
                transform.position = transform.position + dirToAscend * ascendSpeed * Time.deltaTime;
            }
        }

        //Check what's in front of the drill
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, raycastDistance, layersToDig))
        { 
            StopDig();

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.red, 2f);
        }

        //Translate depending of the actual movement values
        Dig();
    }

    private void StartRotate()
    {
        pathFollower.speed = rotationSpeed;
    }

    private void StopRotate()
    {
        pathFollower.speed = 0;
    }

    private void StartDig()
    {
        isDigging = true;
        actualDigSpeed = digSpeed;
        pathFollower.isLinkedToThePath = false;
    }

    private void StopDig()
    {
        actualDigSpeed = 0;
    }

    private void Dig()
    {
        if(actualDigSpeed > 0)
        {
            transform.Translate((Vector3.up * actualDigSpeed * Time.deltaTime));
        }
    }

    private void Ascend()
    {
        dirToAscend = (stopPoint - transform.position).normalized;
        isAscending = true;
    }

    private void CalculateDigRaycastDistance()
    {
        raycastDistance = GetComponent<Collider>().bounds.size.z / 2 + raycastDistanceOffset;
    }

    private void RegisterStopPoint()
    {
        stopPoint = transform.position;
    }
}
