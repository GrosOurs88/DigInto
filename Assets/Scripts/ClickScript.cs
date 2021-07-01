using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickScript : MonoBehaviour
{
    public GameObject objectToDestroy;

    private int layersNumber;
    public List<int> destructiblePartsNumber;
    private int actualLayer = 0;
    private int numberOfPartsDestroyedForActualLayer = 0;

    private DestructiblePartInstanciationScript destructiblePartInstanciationScript;

    public ParticleSystem fireworkCubesYellow;
    public ParticleSystem fireworkCubesBlack;

    public GameObject obstacleSphere;

    private bool gameWin = false;

    public Image gaugeLeft;
    public Image gaugeRight;
    public float decreaseAmount;
    public float increaseAmount;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerEndingText;
    private float actualTime;
    [HideInInspector]
    public bool isTimerRunning = false;
    private bool isGameStarted = false;

    public Vector3 targetToMoveLayer = Vector3.forward * 1f;
    public float moveLayerspeed = 10f;

    public static ClickScript instance;

    public bool useSpikyAvatar = false;
    public GameObject avatar;
    public LayerMask layerMaskAvatar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        destructiblePartInstanciationScript = objectToDestroy.GetComponent<DestructiblePartInstanciationScript>();
        destructiblePartsNumber = new List<int>(destructiblePartInstanciationScript.numberOfDestructibleParts);

        layersNumber = destructiblePartInstanciationScript.layers.Count;

        SetTimer();

        //*******ADD
        if (useSpikyAvatar)
        {
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.CompareTag("DestructibleLeft"))
                {
                    BlockDestroyed(hit);
                    IncreaseGauge(gaugeLeft);
                }
                else if (hit.collider.tag == "Obstacle")
                {
                    TakeDamage();
                }
                else if (hit.collider.CompareTag("Core"))
                {
                    Win(hit);
                }
            }

            if (isTimerRunning == false && isGameStarted == false)
            {
                isTimerRunning = true;
                isGameStarted = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("DestructibleRight"))
                {
                    BlockDestroyed(hit);
                    IncreaseGauge(gaugeRight);
                }
                else if (hit.collider.CompareTag("Obstacle"))
                {
                    TakeDamage();
                }
                else if (hit.collider.CompareTag("Core"))
                {
                    Win(hit);
                }
            }

            if (isTimerRunning == false && isGameStarted == false)
            {
                isTimerRunning = true;
                isGameStarted = true;
            }
        }       

        if(isTimerRunning)
        {
            Timer();
        }

        if(gameWin == false)
        {
            DecreaseGauges();
        }

        //*******ADD
        if (useSpikyAvatar)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskAvatar))
            {                
                avatar.transform.position = hit.point;
            }
        }
    }

    public void SetTimer()
    {
        actualTime = 0f;

        float minutes = Mathf.FloorToInt(actualTime / 60);
        float seconds = Mathf.FloorToInt(actualTime % 60);
        float milliseconds = Mathf.FloorToInt(actualTime * 1000);
        milliseconds = milliseconds % 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void Timer()
    {
        actualTime += Time.deltaTime;

        float minutes = Mathf.FloorToInt(actualTime / 60);
        float seconds = Mathf.FloorToInt(actualTime % 60);
        float milliseconds = Mathf.FloorToInt(actualTime * 1000);
        milliseconds = milliseconds % 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void DecreaseGauges()
    {
        gaugeLeft.fillAmount -= decreaseAmount * Time.deltaTime;
        gaugeRight.fillAmount -= decreaseAmount * Time.deltaTime;
    }

    public void IncreaseGauge(Image _image)
    {
        _image.fillAmount += increaseAmount * Time.deltaTime;
    }

    private void Win(RaycastHit _hit)
    {
        print("WIN !");
        gameWin = true;
        Destroy(_hit.transform.gameObject);
        Instantiate(fireworkCubesYellow, _hit.collider.transform.position, Quaternion.identity);

        isTimerRunning = false;

        timerText.gameObject.SetActive(false);
        timerEndingText.gameObject.SetActive(true);
        timerEndingText.text = timerText.text;

        DestroyEveryObstacle();
    }

    private void BlockDestroyed(RaycastHit _hit)
    {
        _hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.5f); //Semi-transparent white color
        _hit.collider.gameObject.tag = "Obstacle"; //Change tag to "Obstacle"

        obstacleSphere.GetComponent<ObstaclesSpawnerScript>().AddObject(_hit.collider.gameObject); //Add the object to obstacleSphere

        numberOfPartsDestroyedForActualLayer++; //count down the number of block to go for next layer
        if (numberOfPartsDestroyedForActualLayer == destructiblePartsNumber[actualLayer]) //if all the block of the layer has been destroyed
        {
            if (actualLayer < layersNumber - 1) //If there is still a next layer 
            {
                destructiblePartInstanciationScript.SetupLayer(actualLayer + 1); //Setup the next layer
            }

            destructiblePartInstanciationScript.SizeDownLayer(actualLayer); //Remove the actual layer
            actualLayer++; //Count the actual layer index
            numberOfPartsDestroyedForActualLayer = 0; //Reset the number of blocks to destroy
            StartCoroutine(MoveLayer()); //Move the destructible block to the good position toward the camera
        }
    }

    private void TakeDamage()
    {
        print("OUILLE !");
    }

    private void DestroyEveryObstacle()
    {
        foreach(Transform tr in obstacleSphere.transform)
        {
            Instantiate(fireworkCubesBlack, tr.transform.position, tr.transform.rotation);
            Destroy(tr.gameObject);
        }
    }

    private IEnumerator MoveLayer()
    {
        Vector3 targetPosition = objectToDestroy.transform.position + targetToMoveLayer;

        while (objectToDestroy.transform.position != targetPosition)
        {
            objectToDestroy.transform.position = Vector3.MoveTowards(objectToDestroy.transform.position, targetPosition, moveLayerspeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }
}