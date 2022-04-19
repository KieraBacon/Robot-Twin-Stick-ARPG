using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField]
    private int numberOfZombiesToSpawn;
    [SerializeField]
    private GameObject[] zombiePrefabs;
    private SpawnVolume[] spawnVolumes;
    private LinkedList<GameObject> zombiePool = new LinkedList<GameObject>();
    private HashSet<GameObject> spawnedZombies = new HashSet<GameObject>();
    [SerializeField, Min(0)]
    private float despawnTime = 4;
    [SerializeField, Min(0)]
    private float respawnTime = 16;

    private void Awake()
    {
        spawnVolumes = GetComponentsInChildren<SpawnVolume>();
    }

    private void Start()
    {
        foreach (GameObject zombie in zombiePrefabs)
        {
            GameObject spawn = Instantiate(zombie);
            spawn.gameObject.SetActive(false);
            zombiePool.AddLast(spawn);
        }

        for (int i = 0; i < numberOfZombiesToSpawn; i++)
        {
            Fetch();
        }
    }

    private GameObject Fetch()
    {
        if (zombiePool.Count < 1)
            return null;

        LinkedListNode<GameObject> node = zombiePool.First;
        int targetIndex = Random.Range(0, zombiePool.Count - 1);

        for (int i = 1; i < targetIndex; i++)
            node = node.Next;

        GameObject spawn = node.Value;
        zombiePool.Remove(node);
        spawnedZombies.Add(spawn);

        SpawnVolume randomVolume = spawnVolumes[Random.Range(0, spawnVolumes.Length)];
        spawn.transform.position = randomVolume.GetPositionInBounds();

        spawn.gameObject.SetActive(true);
        HealthComponent healthComponent = spawn.GetComponent<HealthComponent>();
        healthComponent.RestoreHealth();
        healthComponent.onDeath += OnDeath;

        return spawn;
    }

    private void OnDeath(GameObject caller)
    {
        HealthComponent healthComponent = caller.GetComponent<HealthComponent>();
        healthComponent.onDeath -= OnDeath;

        StartCoroutine(Despawn(caller));
    }

    private void Return(GameObject gameObject)
    {
        spawnedZombies.Remove(gameObject);
        zombiePool.AddLast(gameObject);
        gameObject.SetActive(false);
    }

    private IEnumerator Despawn(GameObject gameObject)
    {
        Debug.Log("Despawning Zombie!");

        yield return new WaitForSeconds(despawnTime);

        Return(gameObject);

        yield return new WaitForSeconds(respawnTime);

        Fetch();
    }
}