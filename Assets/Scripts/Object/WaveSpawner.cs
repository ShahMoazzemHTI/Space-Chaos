using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves; // List of waves to spawn
    private int currentWaveIndex = 0; // Keeps track of which wave is currently active

    private void Update()
    {
        // If there are no waves, do nothing
        if (waves == null || waves.Count == 0) return;

        // If all waves have been processed, optionally reset (loop back to start)
        if (currentWaveIndex >= waves.Count)
        {
            ResetWaves(); // Optional: restart from beginning
            return;
        }

        // Get the current wave
        Wave currentWave = waves[currentWaveIndex];

        // Update its internal timer to control spawning based on time and player speed boost
        currentWave.UpdateTimer(Time.deltaTime * PlayerController.Instance.boost, SpawnObject);

        // If this wave is finished spawning all its objects, move to the next wave
        if (currentWave.IsFinished)
        {
            currentWaveIndex++;
        }
    }

    // Function to spawn a single object from a wave
    private void SpawnObject(GameObject prefab)
    {
        // Get horizontal (X) spawn position from ObjectSpawner
        float spawnX = ObjectSpawner.Instance.SpawnX;

        // Get a random vertical (Y) position within the allowed camera bounds
        float randomY = Random.Range(ObjectSpawner.Instance.MinY, ObjectSpawner.Instance.MaxY);

        // Final spawn position
        Vector2 spawnPosition = new Vector2(spawnX, randomY);

        // Instantiate the object at the calculated position
        Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
    }

    // Resets all waves (used if looping is enabled)
    private void ResetWaves()
    {
        currentWaveIndex = 0;
        foreach (var wave in waves)
        {
            wave.Reset();
        }
    }

    // Wave class defines how one group of enemies is spawned
    [System.Serializable]
    public class Wave
    {
        public GameObject prefab;        // The object to spawn
        public float spawnInterval = 1f; // Time between each spawn (after delay is done)
        public float spawnDelay = 1f;    // Wait time before spawning starts (first object only)
        public int objectsPerWave = 5;   // Total number of objects to spawn in this wave

        private float spawnTimer;        // Timer used to track spawn time
        private int spawnedObjectsCount; // How many objects have been spawned so far

        // Returns true if the wave is done spawning
        public bool IsFinished => spawnedObjectsCount >= objectsPerWave;

        // Called every frame to handle timing and spawning
        public void UpdateTimer(float deltaTime, System.Action<GameObject> spawnCallback)
        {
            if (IsFinished) return;

            // Wait for initial delay before spawning first object
            if (spawnedObjectsCount == 0 && spawnTimer <= spawnDelay)
            {
                spawnTimer += Time.deltaTime; // Delay increases in real time
                return;
            }

            // After delay, spawn objects at regular intervals
            spawnTimer += deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f; // Reset timer
                spawnCallback?.Invoke(prefab); // Spawn the object
                spawnedObjectsCount++; // Track how many have been spawned
            }
        }

        // Resets this wave so it can run again (if looping)
        public void Reset()
        {
            spawnTimer = 0f;
            spawnedObjectsCount = 0;
        }
    }
}
