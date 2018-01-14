using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct FloatRange
    {
        public float min;
        public float max;
        public float RandomInRange
        { get { return Random.Range(min, max); } }
    }

    public FloatRange timeBetweenSpawns;
    public FloatRange scale;
    public FloatRange randomVelocity;
    public FloatRange angularVelocity;
    private float currentSpawnDelay;

    public Stuff[] stuffPrefabs;

    float timeSinceLastSpawn;

    public float velocity;

    public Material stuffMaterial;

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= currentSpawnDelay)
        {
            timeSinceLastSpawn -= currentSpawnDelay;
            currentSpawnDelay = timeBetweenSpawns.RandomInRange;
            SpawnStuff();
        }
    }

    private void SpawnStuff()
    {
        Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
        //Stuff spawn = Instantiate<Stuff>(prefab);
        Stuff spawn = prefab.GetPooledInstance<Stuff>();

        spawn.transform.localPosition = transform.position;
        spawn.transform.localScale = Vector3.one * scale.RandomInRange;
        spawn.transform.localRotation = Random.rotation;

        spawn.Body.velocity = transform.up * velocity + Random.onUnitSphere * randomVelocity.RandomInRange;
        spawn.Body.angularVelocity = Random.onUnitSphere * angularVelocity.RandomInRange;
        spawn.SetMaterial(stuffMaterial);
        //spawn.GetComponent<MeshRenderer>().material = stuffMaterial;
    }

}
