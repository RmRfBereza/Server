using UnityEngine;
using System.Collections;

public class TturnSegmentHandler : LevelSectionHandler
{

    // Use this for initialization
    void Start()
    {
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
        CreateObsticle(transform.forward * ObsticleOffsetZ, Vector3.zero, transform.right);
        CreateObsticle(transform.forward * ObsticleOffsetZ * -1, Vector3.zero, transform.right);
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            //_robot.StartTurning();
        }
    }
}
