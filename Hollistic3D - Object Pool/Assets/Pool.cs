using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int amount;
    public bool isExpandable;
}
public class Pool : MonoBehaviour
{
    public static Pool instance;
    public List<PoolItem> items;

    private List<GameObject> pooledItems;

    public GameObject Get(string tag)
    {

        for (int i = 0; i < pooledItems.Count; i++)
        {
            if (!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)
            {
                return pooledItems[i];
            }
        }
        foreach (PoolItem poolItem in items)
        {
            if (poolItem.prefab.tag == tag && poolItem.isExpandable)
            {
                GameObject gameObject = Instantiate(poolItem.prefab);
                gameObject.SetActive(false);
                pooledItems.Add(gameObject);
                return gameObject;
            }
        }


        return null;
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pooledItems = new List<GameObject>();
        foreach (PoolItem item in items)
        {
            for (int i = 0; i < item.amount; i++)
            {
                GameObject gameObject = Instantiate(item.prefab);
                gameObject.SetActive(false);
                pooledItems.Add(gameObject);
            }
        }
    }
}
