using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, GameObject> ManagedBaseObjects;
    public Dictionary<string, List<GameObject>> DeadObjects;

   
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (ManagedBaseObjects == null)
        {
            ManagedBaseObjects = new Dictionary<string, GameObject>();
        }
        if (DeadObjects == null)
        {
            DeadObjects = new Dictionary<string, List<GameObject>>();
        }
    }

    public GameObject GetObject(string objectID, bool ForceCreateNew = false)
    {
        if (DeadObjects.ContainsKey(objectID) && DeadObjects[objectID].Count > 0 && !ForceCreateNew)
        {
            GameObject obj = DeadObjects[objectID][0];
            DeadObjects[objectID].RemoveAt(0);
            obj.GetComponent<ResourcePoolID>().ResourceID = objectID;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            if (!ManagedBaseObjects.ContainsKey(objectID))
            {
                ManagedBaseObjects.Add(objectID, new GameObject());
            }

            GameObject obj = Instantiate(ManagedBaseObjects[objectID], Vector3.zero, Quaternion.identity);
            obj.AddComponent<ResourcePoolID>().ResourceID = objectID;
            obj.transform.parent = transform;
            obj.SetActive(true);

            if( ForceCreateNew)
            {
                KillObject(obj);
            }

            return obj;
        }
    }

    public void KillObject(GameObject obj)
    {
        string ID = obj.GetComponent<ResourcePoolID>().ResourceID;
        if (!DeadObjects.ContainsKey(ID))
        {
            DeadObjects.Add(ID, new List<GameObject>());
        }

        obj.transform.parent = transform;
        obj.SetActive(false);
        DeadObjects[ID].Add(obj);
    }
}

