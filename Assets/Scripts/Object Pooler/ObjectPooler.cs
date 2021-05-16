using System;
using System.Collections.Generic;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectPooler : SingletonClass<ObjectPooler>
{
    #region Pool struct

    [Serializable]
    public struct Pool
    {
        public Tag tag;
        public GameObject prefab;
        public int size;
        [CanBeNull] public Transform parent;
    }

    #endregion

    [SerializeField] private List<Pool> pools;
    private Dictionary<Tag, Queue<GameObject>> _poolDictionary;

    private void OnEnable()
    {
        _poolDictionary = new Dictionary<Tag, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject instantiatedObject = Instantiate(pool.prefab, pool.parent);
                instantiatedObject.SetActive(false);
                objectPool.Enqueue(instantiatedObject);
            }

            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(Tag objectTag, Vector3 position, Quaternion rotation)
    {
        GameObject objectToSpawn = _poolDictionary[objectTag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;


        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        pooledObject?.OnObjectSpawn();
        _poolDictionary[objectTag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

 
}