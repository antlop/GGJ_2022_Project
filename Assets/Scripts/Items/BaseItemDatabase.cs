using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using System.IO;

[System.Serializable]
public class ItemDBSerializer
{
    public List<Item_Base> Items;
    public int Count;
    public int ScannedCount;

    public ItemDBSerializer()
    {
        Items = new List<Item_Base>();
    }
}


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
    public ItemDBSerializer Items_Database;
    string DatabaseLocation = "/Items_Database.json";

    void Start()
    {
        Items_Database = new ItemDBSerializer();
        Load();
    }

    public Item_Base GetItemWithID(int id)
    {
        for(int i = 0; i < Items_Database.Items.Count; i++)
        {
            if( Items_Database.Items[i].Item_ID == id)
            {
                return Items_Database.Items[i];
            }
        }

        return null;
    }


    public void Save()
    {
        string json = JsonConvert.SerializeObject(Items_Database.Items);
        string location = Application.persistentDataPath + DatabaseLocation;
        System.IO.File.WriteAllText(location, json);
    }

    void Load()
    {
        // WE ONLINE, MUTHA TRUCKA!

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ow42fhkx0j.execute-api.us-east-1.amazonaws.com/Items");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();

        Items_Database = JsonConvert.DeserializeObject<ItemDBSerializer>(jsonResponse);
    }
}
