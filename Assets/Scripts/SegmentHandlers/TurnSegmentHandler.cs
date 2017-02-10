using UnityEngine;
using System.Collections;

public class TurnSegmentHandler : LevelSectionHandler
{

	// Use this for initialization
	void Start () {
	    FindLevel();
	    CreateRandomObsticles();
        //DrawMapping(3, 1);
        //DrawMapping(1, 1);
    }
	
	// Update is called once per frame
	void Update () {
	    SetPlayer();
    }

    public override void CreateRandomObsticles()
    {
        CreateObsticle(transform.forward * ObsticleOffsetZ, Vector3.zero, transform.right);
        CreateObsticle(transform.right * -ObsticleOffsetZ, new Vector3(0,90,0), transform.forward);
    }

    //void OnTriggerEnter()
    //{
    //    if (_player != null)
    //    {
    //        //_robot.StartTurning();
    //    }
    //}

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            _robot.TurnOnControlls();
        }
    }
}
