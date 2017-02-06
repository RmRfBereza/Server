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

    public override void CreateRandomDoubleObsticle(sbyte pos)
    {
        var obsticle = Storage.getInstance().DoubleObsticles[Mathf.RoundToInt(Random.Range(0, Storage.getInstance().DoubleObsticles.Length - 1))];

        var obstInst = Instantiate(obsticle, gameObject.transform, false);
        obstInst.transform.position += obstInst.transform.forward * pos * ObsticleOffsetZ;
        obstInst.transform.position += obstInst.transform.right *  (-1 +2*Mathf.RoundToInt(Random.Range(0, 2))) * DoubleObsticleOffset;
    }

    public override void CreateRandomSingleObsticle(sbyte pos)
    {
        var obsticle = Storage.getInstance().SingleObsticles[Mathf.RoundToInt(Random.Range(0, Storage.getInstance().SingleObsticles.Length - 1))];

        var obstInst = Instantiate(obsticle, gameObject.transform, false);
        obstInst.transform.position += obstInst.transform.forward * pos * ObsticleOffsetZ;
        obstInst.transform.position += obstInst.transform.right * SingleObsticleOffset * Mathf.Round(Random.Range(-1, 2));
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            //_robot.StartTurning();
        }
    }
}
