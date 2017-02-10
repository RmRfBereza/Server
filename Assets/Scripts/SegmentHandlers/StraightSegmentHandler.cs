using UnityEngine;
using System.Collections;

public class StraightSegmentHandler : LevelSectionHandler {

    void Awake()
    {
        All = transform.Find("All");
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
        CreateObsticle(All.forward * ObsticleOffsetZ, Vector3.zero, All.right);
        CreateObsticle(All.forward * ObsticleOffsetZ * -1, Vector3.zero, All.right);
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            _robot.TurnOnControlls();
        }
    }
}
