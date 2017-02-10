using UnityEngine;
using System.Collections;

public class StraightSegmentHandler : LevelSectionHandler {

    void Awake()
    {
        FindLevel();
    }
    // Use this for initialization
    void Start () {
        CreateRandomObsticles();
	}
	
	// Update is called once per frame
	void Update () {
        SetPlayer();
    }

    public override void CreateRandomObsticles()
    {
        CreateObsticle(transform.forward * ObsticleOffsetZ, Vector3.zero, transform.right);
        CreateObsticle(transform.forward * ObsticleOffsetZ * -1, Vector3.zero, transform.right);
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            _robot.TurnOnControlls();
        }
    }
}
