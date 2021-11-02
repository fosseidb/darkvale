using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledNecros;
    public GameObject necromancerPrefab;
    public int amountOfNecros;
    public Transform necromancerHolder;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledNecros = new List<GameObject>();
        GameObject tmp;
        for(int i= 0; i < amountOfNecros; i++)
        {
            tmp = Instantiate(necromancerPrefab,necromancerHolder);
            tmp.SetActive(false);
            pooledNecros.Add(tmp);
        }
    }

    public GameObject GetPooledNecromancer()
    {
        for(int i =0; i < amountOfNecros; i++)
        {
            if (!pooledNecros[i].activeInHierarchy)
            {
                return pooledNecros[i];
            }
        }
        return null;
    }
}
