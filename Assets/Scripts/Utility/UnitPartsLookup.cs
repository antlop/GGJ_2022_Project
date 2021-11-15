using System.Collections.Generic;
using UnityEngine;

public class PartDefinition
{
    public GameObject Part;
    public string Joint;
}


public class UnitPartsLookup : MonoBehaviour
{
    public Transform UnitPartsRoot;

    public Dictionary<string, PartDefinition> _partsLookupDictionary = new Dictionary<string, PartDefinition>();

    // Start is called before the first frame update
    void Awake()
    {
        SearchChildren(UnitPartsRoot);
    }

    void SearchChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child == parent)
                continue;
            if (child.childCount == 0)
            {
                //object found, CANNOT add to the dictionary unless the object has a SkinnedMeshRenderer
                SkinnedMeshRenderer smr = child.GetComponent<SkinnedMeshRenderer>();
                if (smr != null)
                {
                    if (_partsLookupDictionary.ContainsKey(child.name))
                    {
                        Debug.Log("ERROR - parts dictionary found object with a duplicate name: " + child.name + ", ignoring any duplicate names!");
                    }
                    else
                    {
                        PartDefinition newPart = new PartDefinition();
                        newPart.Part = child.gameObject;
                        newPart.Joint = smr.rootBone.name;
                        _partsLookupDictionary.Add(child.name, newPart);
                    }
                }
            }
            else
            {
                SearchChildren(child);
            }
        }
    }

    //Returns a reference to each of the parts.
    public List<PartDefinition> LookupParts(List<string> parts)
    {
        List<PartDefinition> returnObjects = new List<PartDefinition>();

        foreach (var part in parts)
        {
            PartDefinition objectPrefab;
            if (_partsLookupDictionary.TryGetValue(part, out objectPrefab))
            {
                returnObjects.Add(objectPrefab);
            }
        }

        return returnObjects;
    }
}
