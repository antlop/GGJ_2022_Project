using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BaseItemDatabase : MonoBehaviour
{
    #region Singleton
    private static BaseItemDatabase instance = null;

    // Game Instance Singleton
    public static BaseItemDatabase Instance
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

    public List<Item_Base> Database_of_base_items;
    string DatabaseLocation = "/Items_Database.json";

    void Start()
    {
        Load();
    }

    public Item_Base GetItemWithID(int id)
    {
        for(int i = 0; i < Database_of_base_items.Count; i++)
        {
            if( Database_of_base_items[i].ItemID == id)
            {
                return Database_of_base_items[i];
            }
        }

        return null;
    }


    public void Save()
    {
        string json = JsonConvert.SerializeObject(Database_of_base_items);
        string location = Application.persistentDataPath + DatabaseLocation;
        System.IO.File.WriteAllText(location, json);
    }

    void Load()
    {
        string location = Application.persistentDataPath + DatabaseLocation;
        string jsonDB = System.IO.File.ReadAllText(location);
        if (jsonDB.Length > 0)
        {
            Database_of_base_items = JsonConvert.DeserializeObject<List<Item_Base>>(jsonDB);
        }
    }
}
