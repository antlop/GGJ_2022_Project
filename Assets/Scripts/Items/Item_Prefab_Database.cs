using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public struct ItemsDatabase
{
    public List<Item_Base> BaseItemsDatabaseList;
}

public class Item_Prefab_Database : MonoBehaviour
{

    #region Singleton
    private static Item_Prefab_Database instance = null;

    // Game Instance Singleton
    public static Item_Prefab_Database Instance
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
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

/*
    public ItemsDatabase DatabaseStruct;
    string DatabaseLocation = "/Items_Database.json";

    // Start is called before the first frame update
    void Start()
    {
        DatabaseStruct = new ItemsDatabase();
        DatabaseStruct.BaseItemsDatabaseList = new List<Item_Base>();

        Load();
    }

    public void Save()
    {
        string json = JsonConvert.SerializeObject(DatabaseStruct);
        string location = Application.persistentDataPath + DatabaseLocation;
        System.IO.File.WriteAllText(location, json);
    }

    void Load()
    {
        string location = Application.persistentDataPath + DatabaseLocation;
        string jsonDB = System.IO.File.ReadAllText(location);
        if (jsonDB.Length > 0)
        {
            DatabaseStruct = JsonConvert.DeserializeObject<ItemsDatabase>(jsonDB);
        }
    }

    public Item_Base GetItemWithID(int id)
    {
        foreach (Item_Base item in DatabaseStruct.BaseItemsDatabaseList)
        {
            if (item.ItemID == id) { return item; }
        }

        return null;
    }
*/
}
