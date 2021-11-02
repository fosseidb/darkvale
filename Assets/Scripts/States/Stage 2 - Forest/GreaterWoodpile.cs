using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterWoodpile : MonoBehaviour
{
    private GameManager gameManager;

    private Transform[] _woodpiles;
    private int _totalWood = 0;
    private int _activePile = 1;
    [SerializeField] private int _woodPerPile = 10;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        _woodpiles = GetComponentsInChildren<Transform>();
        foreach(Transform woodPile in _woodpiles)
        {
            if (woodPile.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    woodPile.gameObject.SetActive(false);
                }
        }
        
    }

    public void IncrementPile()
    {
        _totalWood++;
        if((_totalWood / _activePile) > _woodPerPile)
        {
            _woodpiles[_activePile].gameObject.SetActive(true);
            _activePile++;
        }

        gameManager.UpdateWoodPile(_totalWood);
    }

}
