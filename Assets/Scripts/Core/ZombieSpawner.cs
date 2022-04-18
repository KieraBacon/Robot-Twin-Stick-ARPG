using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField]
    private int numberOfZombiesToSpawn;
    [SerializeField]
    private GameObject[] zombiePrefabs;
    private SpawnVolume[] spawnVolumes;

    private void Awake()
    {
        spawnVolumes = GetComponentsInChildren<SpawnVolume>();
    }

    private void Start()
    {
        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject randomZombie = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        SpawnVolume randomVolume = spawnVolumes[Random.Range(0, spawnVolumes.Length)];

        // Object pooling can be referenced here
        GameObject spawn = Instantiate(randomZombie, randomVolume.GetPositionInBounds(), randomVolume.transform.rotation);
    }
}
