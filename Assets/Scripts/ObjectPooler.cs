using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlastDash
{
    public class ObjectPooler : MonoBehaviour
    { 
        [Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }
        [SerializeField]
        private List<Pool> pools;
        private Dictionary<string,Queue<GameObject>> poolDictionary;

        #region Singleton

        public static ObjectPooler Instance;
        private void Awake()
        {
            Instance = this;
            poolDictionary = new Dictionary<string,Queue<GameObject>>();

            foreach(Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, this.transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }
        #endregion
        
        public GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(key))
            {
                Debug.LogError("Object pool with key" + key + " doesn't exists");
                return null;
            }
            
            GameObject objectToSpawn = poolDictionary[key].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            
            poolDictionary[key].Enqueue(objectToSpawn);
            
            return objectToSpawn; 
        }
        
        public GameObject SpawnFromPool(string key, Transform parentTransform)
        {
            if (!poolDictionary.ContainsKey(key))
            {
                Debug.LogError("Object pool with key" + key + " doesn't exists");
                return null;
            }
            
            GameObject objectToSpawn = poolDictionary[key].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.parent = parentTransform;
            objectToSpawn.transform.localScale = Vector3.one;
            
            poolDictionary[key].Enqueue(objectToSpawn);
            
            return objectToSpawn; 
        }
        
        public void BackToPool(string objectPooTag , GameObject gameObject) 
        {
            poolDictionary[objectPooTag].Enqueue(gameObject);
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}

