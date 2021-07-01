using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawnerScript : MonoBehaviour
{
    public int numObjects;
    public GameObject prefab;
    private float sphereRadius;

    void Start()
    {
        sphereRadius = GetComponent<SphereCollider>().radius;

        for (int i = 0; i < numObjects; i++)
        {
            InstanciateObject();
        }
    }       
       
    public void AddObject(GameObject _object)
    {
        GameObject spawned = Instantiate(_object, transform.position, Quaternion.identity, transform);

        spawned.GetComponent<Renderer>().material.color = Color.black;

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 vec = new Vector3(x, y, z).normalized * sphereRadius;
        spawned.transform.position = vec;

        spawned.transform.LookAt(Vector3.zero);
    }

    public void InstanciateObject()
    {
        GameObject spawned = Instantiate(prefab, transform.position, Quaternion.identity, transform);

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 vec = new Vector3(x, y, z).normalized * sphereRadius;
        spawned.transform.position = vec;

        spawned.transform.LookAt(Vector3.zero);
    }
}
