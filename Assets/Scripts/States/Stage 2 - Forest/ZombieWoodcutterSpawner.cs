using UnityEngine;

public class ZombieWoodcutterSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private GameObject zombieWoodcutterPrefab;
    [SerializeField] private Transform zombieWoodcutterHolder;

    [SerializeField] private float spawnWaitPeriod;
    private float spawnTimer;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        SpawnWoodcutter();
        SpawnWoodcutter();
        SpawnWoodcutter();
        SpawnWoodcutter();
        SpawnWoodcutter();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer >= spawnWaitPeriod)
        {
            SpawnWoodcutter();
            spawnTimer = 0f;
        }
        else
            spawnTimer += Time.deltaTime;
    }

    private void SpawnWoodcutter()
    {
        int randomSpawnPointIndex = Random.Range(0, spawnpoints.Length);
        GameObject newWoodcutter = Instantiate(zombieWoodcutterPrefab, zombieWoodcutterHolder);
        newWoodcutter.transform.position = spawnpoints[randomSpawnPointIndex].transform.position;
        newWoodcutter.transform.rotation = spawnpoints[randomSpawnPointIndex].transform.rotation;

        gameManager.SubscribeToWoodcutter(newWoodcutter.GetComponent<ZombieWoodcutter>());
    }
}
