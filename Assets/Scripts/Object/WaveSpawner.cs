using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    private int currentWaveIndex = 0;

    private void Update()
    {
        if (waves == null || waves.Count == 0) return;

        if (currentWaveIndex >= waves.Count)
        {
            ResetWaves(); // Optional: restart from beginning
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        currentWave.UpdateTimer(Time.deltaTime * PlayerController.Instance.boost, SpawnObject);

        if (currentWave.IsFinished)
        {
            currentWaveIndex++;
        }
    }

    private void SpawnObject(GameObject prefab)
    {
        float spawnX = ObjectSpawner.Instance.SpawnX;
        float randomY = Random.Range(ObjectSpawner.Instance.MinY, ObjectSpawner.Instance.MaxY);
        Vector2 spawnPosition = new Vector2(spawnX, randomY);
        Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
    }

    private void ResetWaves()
    {
        currentWaveIndex = 0;
        foreach (var wave in waves)
        {
            wave.Reset();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public GameObject prefab;
        public float spawnInterval = 1f;
        public float spawnDelay = 1f;
        public int objectsPerWave = 5;

        private float spawnTimer;
        private int spawnedObjectsCount;

        public bool IsFinished => spawnedObjectsCount >= objectsPerWave;

        public void UpdateTimer(float deltaTime, System.Action<GameObject> spawnCallback)
        {
            if (IsFinished) return;
            if (spawnedObjectsCount == 0 && (spawnTimer <= spawnDelay))
            {
                spawnTimer += Time.deltaTime;
                return;
            }
            

            spawnTimer += deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                spawnCallback?.Invoke(prefab);
                spawnedObjectsCount++;
            }
        }

        public void Reset()
        {
            spawnTimer = 0f;
            spawnedObjectsCount = 0;
        }
    }
}
