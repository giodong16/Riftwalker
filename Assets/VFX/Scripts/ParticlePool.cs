using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// DÙNG LIÊN TIẾP SẼ INIT NHIỀU
public class PoolParticle : MonoBehaviour
{
    public static PoolParticle Instance;
    
    [System.Serializable]
    public class ParticlePrefab
    {
        public ParticleName particleName;
        public ParticleSystem particlePrefab;
    }

    public List<ParticlePrefab> particlePrefabs;
    private Transform rootPostionPlayer;
    private Dictionary<ParticleName, Queue<ParticleSystem>> poolDictionary;
    private bool isPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        poolDictionary = new Dictionary<ParticleName, Queue<ParticleSystem>>();
        foreach (var prefab in particlePrefabs)
        {
            Queue<ParticleSystem> particleQueue = new Queue<ParticleSystem>();
            poolDictionary[prefab.particleName] = particleQueue;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public ParticleSystem GetParticle(ParticleName particleName, Vector3 particlePosition,bool isPlayer = true)
    {
        if(!poolDictionary.ContainsKey(particleName)) return null;
        ParticleSystem particle;
        if (poolDictionary[particleName].Count > 0)
        {
            particle = poolDictionary[particleName].Dequeue();
            particle.transform.position = particlePosition;
            particle.gameObject.SetActive(true);
        }
        else
        {
            ParticlePrefab particlePrefab = particlePrefabs.Find(p => p.particleName == particleName);
            if (particlePrefab == null) return null;

            rootPostionPlayer = FindObjectOfType<PlayerController>().transform;
            if (rootPostionPlayer != null) {
                particle = Instantiate(particlePrefab.particlePrefab, particlePosition, Quaternion.identity,isPlayer ? rootPostionPlayer:null );
            }
            else
            {
                particle = Instantiate(particlePrefab.particlePrefab, particlePosition, Quaternion.identity);
            }

        }
        particle.Play();
        StartCoroutine(ReturnParticleToPool(particle,particleName,particle.main.duration));
            

        return particle;
    }

    private IEnumerator ReturnParticleToPool(ParticleSystem particle, ParticleName particleName, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (particle == null || particle.Equals(null)) yield break;
        particle.Stop();
        particle.gameObject.SetActive(false); 
        poolDictionary[particleName].Enqueue(particle);
    }


    public float GetParticleDuration(ParticleName particleName)
    {
        ParticlePrefab particle = particlePrefabs.FirstOrDefault(p => p.particleName == particleName);

        if (particle != null && particle.particlePrefab != null)
        {
            return particle.particlePrefab.main.duration;
        }

        return 1f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearDestroyedParticles();
    }

    private void ClearDestroyedParticles()
    {
        poolDictionary = new Dictionary<ParticleName, Queue<ParticleSystem>>();
        foreach (var prefab in particlePrefabs)
        {
            Queue<ParticleSystem> particleQueue = new Queue<ParticleSystem>();
            poolDictionary[prefab.particleName] = particleQueue;
        }
    }


}
