using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

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

    public ScrollRect MapListScrollRect;

    public string SaveFileName;

    private bool hasDelta = false;

    public UnityEngine.UI.Toggle listItem;
    List<Toggle> _listOfToggles;
    int selectedIndex = -1;

    public WFC_Tiles WFC_Tiles_Array;

    // Start is called before the first frame update
    void Start()
    {
        _savedMaps = new List<WFC_SaveObject>();
        _stagedMaps = new List<WFC_SaveObject>();
        _listOfToggles = new List<Toggle>();

        string location = Application.persistentDataPath + "/" + SaveFileName;
        string jsonDB = System.IO.File.ReadAllText(location);
        if (jsonDB.Length > 0)
        {
            _savedMaps = JsonConvert.DeserializeObject<List<WFC_SaveObject>>(jsonDB);
            hasDelta = true;
        }
    }

    public void SaveMap(WFC_SaveObject saveObj)
    {
        _stagedMaps.Add(saveObj);
        hasDelta = true;
    }

    public void LoadMap()
    {
        int index = selectedIndex;
        if (index >= 0)
        {
            if(_savedMaps.Count > index)
            {
                WFC_Tiles_Array.GenerateAMap(_savedMaps[index].Seed, new Vector2Int((int)_savedMaps[index].MapSize.x, (int)_savedMaps[index].MapSize.y));
                return;
            }

            index -= _savedMaps.Count;
            
            if (_stagedMaps.Count > index)
            {
                WFC_Tiles_Array.GenerateAMap(_stagedMaps[index].Seed, new Vector2Int((int)_stagedMaps[index].MapSize.x, (int)_stagedMaps[index].MapSize.y));
            }
        }
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

    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (hasDelta)
        {
            hasDelta = false;

            foreach(Transform child in MapListScrollRect.content.transform)
            {
                Destroy(child.gameObject);
            }

            float yPos = 0;
            _listOfToggles.Clear();

            foreach (WFC_SaveObject saveObj in _savedMaps)
            {
                Toggle item = Instantiate(listItem, MapListScrollRect.content.transform);
                item.GetComponentInChildren<Text>().text = string.Format("Seed: {0}, Size: {1}", saveObj.Seed, saveObj.MapSize);
                item.transform.Translate(new Vector3(0, -yPos, 0));
                item.onValueChanged.AddListener(delegate { ToggleValueChanged(item); });
                yPos += 25;

                _listOfToggles.Add(item);
            }

            foreach (WFC_SaveObject saveObj in _stagedMaps)
            {
                Toggle item = Instantiate(listItem, MapListScrollRect.content.transform);
                item.GetComponentInChildren<Text>().text = string.Format("Seed: {0}, Size: {1}", saveObj.Seed, saveObj.MapSize);
                item.transform.Translate(new Vector3(0, -yPos, 0));
                yPos += 25;

                _listOfToggles.Add(item);
            }
        }
    }
#endif

    void ToggleValueChanged(Toggle change)
    {
        int index = -1;
        foreach (Toggle saveObj in _listOfToggles)
        {
            index++;
            if(saveObj == change)
            {
                selectedIndex = index;
            } else if (saveObj.isOn)
            {
                saveObj.isOn = false;
            }
        }
    }
}
