using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public struct WFC_SaveObject
{
    public string Seed;
    public Vector2 MapSize;

    public WFC_SaveObject(string seed, Vector2 mapSize)
    {
        Seed = seed;
        MapSize = mapSize;
    }
}

public class WFC_Managment : MonoBehaviour
{
    [SerializeField]
    List<WFC_SaveObject> _savedMaps;

    [SerializeField]
    List<WFC_SaveObject> _stagedMaps;

    public string SaveFileName;

    // Start is called before the first frame update
    void Start()
    {
        _savedMaps = new List<WFC_SaveObject>();
        _stagedMaps = new List<WFC_SaveObject>();

        string location = Application.persistentDataPath + "/" + SaveFileName;
        string jsonDB = System.IO.File.ReadAllText(location);
        if (jsonDB.Length > 0)
        {
            _savedMaps = JsonConvert.DeserializeObject<List<WFC_SaveObject>>(jsonDB);
        }
    }

    public void SaveMap(WFC_SaveObject saveObj)
    {
        _stagedMaps.Add(saveObj);
    }

    private void OnDestroy()
    {
        if(_stagedMaps.Count > 0)
        {
            foreach (WFC_SaveObject saveObj in _stagedMaps) {
                _savedMaps.Add(saveObj);
            }
        }

        string json = JsonConvert.SerializeObject(_savedMaps);
        string location = Application.persistentDataPath + "/" + SaveFileName;
        System.IO.File.WriteAllText(location, json);
    }
}
