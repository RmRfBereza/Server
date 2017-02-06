using UnityEngine;
using System.Collections;
using System.Linq;

public class GazeCameraHandler : MonoBehaviour
{

    private Level _level;
    private Vector3 height = new Vector3(0, 1.6f, 0);

	// Use this for initialization
	void Start () {
        _level = GameObject.FindGameObjectWithTag("Plane").GetComponent<Level>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.position = _level.Player.transform.position + height + _level.Player.transform.forward * 0.1f;
    }
}
