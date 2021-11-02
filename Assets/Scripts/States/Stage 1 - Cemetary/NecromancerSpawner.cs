using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerSpawner : MonoBehaviour
{
    [SerializeField] private float necroWaitingSpawnTime = 1f; // time between spawns
    [SerializeField] private Transform[] necromancerSummonPoints; // list of summoningpoints that are to be assigned to spawned necromancers
    [SerializeField] private SummoningCrystal summoningCrystal; // list of summoningpoints that are to be assigned to spawned necromancers
    [SerializeField] private GameManager gameManager;

    private float timer = 0f;
    private int[] summonIDs= new int[5];

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > necroWaitingSpawnTime)
        {
            SpawnNecromancer();
            timer = 0;
        }
        
    }

    private void SpawnNecromancer()
    {
        GameObject necromancer = ObjectPool.SharedInstance.GetPooledNecromancer();
        if(necromancer != null)
        {
            bool placed = false;
            for(int i =0; i< summonIDs.Length; i++)
            {
                if (summonIDs[i] == 0 && !placed)
                {
                    placed = true;
                    summonIDs[i] = 1;
                    necromancer.GetComponent<NPC_Necromancer>().NecroID = i;
                }
            }

            necromancer.transform.position = this.transform.position;
            necromancer.transform.rotation = this.transform.rotation;

            if (!necromancer.GetComponent<NPC_Necromancer>().summoningCrystal) 
                necromancer.GetComponent<NPC_Necromancer>().summoningCrystal = summoningCrystal.transform;

            gameManager.SubscribeToNecromancer(necromancer.GetComponent<NPC_Necromancer>());
            necromancer.GetComponent<NPC_Necromancer>().NecromancerDeathEvent += OnNecroDeath;
            necromancer.GetComponent<NPC_Necromancer>().SetUpStats();
            necromancer.SetActive(true);
            necromancer.GetComponent<NPC_Necromancer>().SetDestinationSummonPoint(necromancerSummonPoints[necromancer.GetComponent<NPC_Necromancer>().NecroID]);
        }

    }

    private void OnNecroDeath(NPC_Necromancer necro)
    {
        int id = necro.GetComponent<NPC_Necromancer>().NecroID;
        summonIDs[id] = 0;
    }
}
