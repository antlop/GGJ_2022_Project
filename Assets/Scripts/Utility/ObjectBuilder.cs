using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectBuilder : MonoBehaviour
{
    public Transform PartsRoot;
    public Transform JointsRoot;
    //This could be improved to create a custom interface where a developer can drop in a gameobject and it adds the name of the object
    public List<string> PartsNames = new List<string>();

    Dictionary<string, Transform> JointLookup = new Dictionary<string, Transform>();

    private void Awake()
    {
        GetJoints(JointsRoot);
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitPartsLookup lookup = GameObject.FindObjectOfType<UnitPartsLookup>();
        if (lookup != null)
        {
            List<PartDefinition> partsPrefab = lookup.LookupParts(PartsNames);

            this.gameObject.SetActive(false);
            Vector3 cachedPos = this.transform.position;
            this.transform.position = Vector3.zero;
            foreach (var partPrefab in partsPrefab)
            {
                
                GameObject part = Instantiate<GameObject>(partPrefab.Part, PartsRoot, false);
                //the Parts lookup object cannot add a part UNLESS it has the SkinnedMeshRenderer, so this should always be safe.
                SkinnedMeshRenderer smr = part.GetComponent<SkinnedMeshRenderer>();
                smr.enabled = false;
                part.transform.position = Vector3.zero;
                Transform joint;
                if (JointLookup.TryGetValue(partPrefab.Joint, out joint))
                {
                    smr.rootBone = joint;
                }
            }

            this.transform.position = cachedPos;
            this.gameObject.SetActive(true);
        }
    }

    private void GetJoints(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (JointLookup.ContainsKey(child.name))
            {
                Debug.LogError(this.gameObject.name + " trying to add a root bone with duplicate name " + child.name);
            }
            else
            {
                JointLookup.Add(child.name, child);
            }
        }
    }
}
