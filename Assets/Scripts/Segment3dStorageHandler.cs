using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment3dStorageHandler : MonoBehaviour {

    public List<NamedPrefab> SegmentPrefabsList;

    [HideInInspector]
    public Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        foreach (var segment in SegmentPrefabsList)
        {
            SegmentPrefabsDictionary[segment.name] = segment.prefab;
        }
    }

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Serializable]
    public struct NamedPrefab
    {
        public string name;
        public GameObject prefab;
    }
}
