using UnityEngine;
using System.Collections;

public class FinishHandler : MonoBehaviour
{
    private Level _level;

	// Use this for initialization
	void Start ()
	{
	    _level = GameObject.Find("Plane").GetComponent<Level>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        if (_level != null)
        {
            _level.NotifyGameWon();
        }
    }
}
