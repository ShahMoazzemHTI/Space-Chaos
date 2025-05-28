using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 5;
    List<GameObject> pool;

    void Start()
    {
        createPool();
    }

    void createPool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            createNewObject();
        }
    }

    GameObject createNewObject()
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.SetActive(false);
        pool.Add(newObject);
        return newObject;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }
        return createNewObject();
    }

}
