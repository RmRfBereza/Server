using UnityEngine;
using System.Collections;

public class IntersectionHandler : LevelSectionHandler {

	// Use this for initialization
	void Start ()
	{
	    FindLevel();
        //DrawMapping(3, 1);
        //DrawMapping(1, 1);
        //DrawMapping(-3, 1);
        //DrawMapping(-1, 1);
        //DrawMapping(3, -1);
        //DrawMapping(1, -1);
        //DrawMapping(-3, -1);
        //DrawMapping(-1, -1);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    SetPlayer();
	}

    public override void CreateRandomDoubleObsticle(sbyte pos)
    {

    }

    public override void CreateRandomSingleObsticle(sbyte pos)
    {

    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            //_robot.StartTurning();
        }
    }
}
