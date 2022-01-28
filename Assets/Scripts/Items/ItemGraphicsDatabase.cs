using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item_Graphics
{
    public Sprite Icon;
    public GameObject Object;
    public int ID;
}

public class ItemGraphicsDatabase : MonoBehaviour
{
    #region Singleton
    private static ItemGraphicsDatabase instance = null;

    // Game Instance Singleton
    public static ItemGraphicsDatabase Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        if (this.transform.parent == null)
            DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public List<Item_Graphics> ItemGraphicsList;
    private Dictionary<int, Item_Graphics> Database;

    // Start is called before the first frame update
    void Start()
    {
        Database = new Dictionary<int, Item_Graphics>();
        foreach(Item_Graphics graphics in ItemGraphicsList)
        {
            if (Database.ContainsKey(graphics.ID))
            {
                Debug.Log("Clang! Duplicate graphics item IDs: " + graphics.ID);
            }
            else
            {
                Database.Add(graphics.ID, graphics);
            }
        }
    }

    public Item_Graphics GetGraphicsForItemWithID(int _id)
    {
        return Database[_id];
    }
}
