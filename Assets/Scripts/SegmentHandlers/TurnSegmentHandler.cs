using UnityEngine;
using System.Collections;

public class TurnSegmentHandler : LevelSectionHandler
{

	// Use this for initialization
	void Start () {
	    FindLevel();
        DrawMapping(3, 1);
        DrawMapping(1, 1);
    }
	
	// Update is called once per frame
	void Update () {
	    SetPlayer();
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            //_robot.StartTurning();
        }
    }
}
