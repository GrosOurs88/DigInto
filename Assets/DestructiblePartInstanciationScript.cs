using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePartInstanciationScript : MonoBehaviour
{
    public List<GameObject> layers = new List<GameObject>();
    public List<int> numberOfDestructibleParts = new List<int>();

    private int actualListCount = 0;
    private List<int> actualRandomNumbersList = new List<int>();

    private int randomNumberMaxIteration = 20;

    public GameObject destructiblePartPrefabLeft;
    public GameObject destructiblePartPrefabRight;

    public float prefabLeftPercent;

    void Start()
    {
        SetupLayer(0);
    }

    private void TakeRandomNumbers(int _numberOfDestructibleElements, int _listCount)
    {
        for (int i = 0; i < _numberOfDestructibleElements; i++)
        {
            int x = 0;

            //Do X iterations before abandon
            while (x < randomNumberMaxIteration)
            {
                //Take a random number
                int n = Random.Range(0, _listCount);

                if(actualRandomNumbersList.Count == 0)
                {
                    //Add the random number to the list
                    actualRandomNumbersList.Add(n);
                }

                //Check if the random number is already in the list
                else if (!actualRandomNumbersList.Contains(n))
                {
                    //Add the random number to the list
                    actualRandomNumbersList.Add(n);
                    break;
                }

                else
                {
                    //Generate a new random number
                    x++;
                }
            }
        }
    }

    private bool RandomLeftRight()
    {
        //Take a random number
        int n = Random.Range(0, 101);

        if(n > prefabLeftPercent)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Reset()
    {
        actualListCount = 0;
        actualRandomNumbersList.Clear();
    }

    public void SetupLayer(int _index)
    {
        //Count the number of childs in the layer
        foreach (Transform tr in layers[_index].transform)
        {
            actualListCount++;
        }

        //Generate a list of random numbers           
        TakeRandomNumbers(numberOfDestructibleParts[_index] - 1, actualListCount);

        //Replace all the objects at the random numbers indexes by destructible ones
        for (int j = 0; j < actualRandomNumbersList.Count; j++)
        {
            if (RandomLeftRight() == true)
            {
                Instantiate(destructiblePartPrefabLeft, layers[_index].transform.GetChild(actualRandomNumbersList[j]).transform.position, layers[_index].transform.GetChild(actualRandomNumbersList[j]).transform.rotation, layers[_index].transform);
            }
            else
            {
                Instantiate(destructiblePartPrefabRight, layers[_index].transform.GetChild(actualRandomNumbersList[j]).transform.position, layers[_index].transform.GetChild(actualRandomNumbersList[j]).transform.rotation, layers[_index].transform);
            }
        }

        //Replace all the objects at the random numbers indexes by destructible ones
        for (int j = 0; j < actualRandomNumbersList.Count; j++)
        {
            Destroy(layers[_index].transform.GetChild(actualRandomNumbersList[j]).gameObject);
        }

        Reset();
    }

    public void RemoveLayer(int _layerIndex)
    {
        foreach (Transform tr in layers[_layerIndex].transform)
        {
            Destroy(tr.gameObject);
        }
    }

    public void SizeDownLayer(int _layerIndex)
    {
        foreach (Transform tr in layers[_layerIndex].transform)
        {
            tr.gameObject.GetComponent<Animator>().SetTrigger("SizeDown");
        }
    }
}
