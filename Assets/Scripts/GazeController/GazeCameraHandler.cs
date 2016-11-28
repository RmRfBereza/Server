using UnityEngine;
using System.Collections;

public class GazeCameraHandler : MonoBehaviour {

    private Level _level;

	// Use this for initialization
	void Start () {
        _level = GameObject.Find("Plane").GetComponent<Level>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position = _level.Player.transform.position;
    }
}
