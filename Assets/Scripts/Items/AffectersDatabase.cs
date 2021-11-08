using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AffectersDatabase : MonoBehaviour
{
    #region Singleton
    private static AffectersDatabase instance = null;

    // Game Instance Singleton
    public static AffectersDatabase Instance
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


    public List<Affecter> Database;

    [SerializeField] public Dictionary<int,Affecter> Database_of_base_items;

    // Start is called before the first frame update
    void Start()
    {
        Database_of_base_items = new Dictionary<int, Affecter>();

        foreach(Affecter affecter in Database)
        {
            Database_of_base_items.Add(affecter.ID, affecter);
        }
#if !UNITY_EDITOR
        Database = null;
#endif
    }

    public Affecter GetAffecterWithID(int id)
    {
        if( !Database_of_base_items.ContainsKey(id) )
        {
            Database_of_base_items.Add(id, new Affecter());
        }

        return Database_of_base_items[id];
    }
}
