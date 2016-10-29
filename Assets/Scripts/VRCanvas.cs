using UnityEngine;
using System.Collections;

public class VRCanvas : MonoBehaviour {

    public Camera cam;

	// Use this for initialization
	void Start () {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam != null)
        {
            // Tie this to the camera, and do not keep the local orientation.
            transform.SetParent(cam.GetComponent<Transform>(), true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
