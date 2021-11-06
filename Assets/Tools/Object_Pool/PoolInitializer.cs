using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolObject
{
    public int basePoolCount = 1;
    public GameObject BaseObject;
    public string ObjectTagLine = "";
}


public class PoolInitializer : MonoBehaviour
{
    public List<PoolObject> InitializerObjects;
    ObjectPool Pool;
	
	void Awake()
    {
        if (Pool == null)
        {
            Pool = gameObject.AddComponent<ObjectPool>();
            Pool.Initialize();
        }
        foreach (PoolObject pObj in InitializerObjects)
        {
            for (int i = 0; i < pObj.basePoolCount; i++)
            {
                if (!Pool.ManagedBaseObjects.ContainsKey(pObj.ObjectTagLine))
                {
                    Pool.ManagedBaseObjects.Add(pObj.ObjectTagLine, pObj.BaseObject);
                }
                Pool.GetObject(pObj.ObjectTagLine, true);
            }
        }

        Destroy(this);
    }

}
