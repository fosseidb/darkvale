using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatehouse : MonoBehaviour
{
    [SerializeField] GameObject GateSecureHolder;
    [SerializeField] GameObject GateBreachedHolder;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        GateSecureHolder.SetActive(true);
        GateBreachedHolder.SetActive(false);

        gameManager = FindObjectOfType<GameManager>();
        if(gameManager != null)
            gameManager.StageChangeEvent += Breached;
    }

    // Update is called once per frame
    void Breached(int i)
    {
        if(i == 4)
        {
            GateSecureHolder.SetActive(false);
            GateBreachedHolder.SetActive(true);
        }
    }
}
