using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public GameObject[] SingleObsticles;
    public GameObject[] DoubleObsticles;

    private static Storage _instance;
    // Use this for initialization
	void Start ()
	{
	    _instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Storage getInstance()
    {
        return _instance;
    }
}
