using UnityEngine;
using System.Collections;

public class TturnSegmentHandler : LevelSectionHandler
{

    // Use this for initialization
    void Start()
    {
        All = transform.Find("All");
        FindLevel();
        //DrawMapping(-3, 1);
        //DrawMapping(-1, 1);
        //DrawMapping(-3, -1);
        //DrawMapping(-1, -1);
    }

    // Update is called once per frame
    void Update()
    {
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
            //_robot.StartTurning();
        }
    }
}
