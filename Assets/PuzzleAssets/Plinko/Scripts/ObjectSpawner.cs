using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnee;
    public Transform spawnPosition;
    public bool stopSpawn = false;
    public float spawnTime;
    public float spawnDelay;
    private GameObject lastSpawned = null;
    private AudioSource activateSound;

    void Start()
    {
        activateSound = GetComponent<AudioSource>();
        SpawnObject();
        activateSound.Play();
    }
    public void SpawnObject()
    {
        DestroyObject();
        lastSpawned = Instantiate(spawnee, spawnPosition.position, spawnPosition.rotation);
    }
    public void DestroyObject()
    {
        if(lastSpawned != null)
        {
            Destroy(lastSpawned);
            lastSpawned = null;
        }
    }
}
