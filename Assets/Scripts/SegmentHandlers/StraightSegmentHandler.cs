using UnityEngine;
using System.Collections;

public class StraightSegmentHandler : LevelSectionHandler {

    public GameObject[] SingleObsticles;
    public GameObject[] DoubleObsticles;

    private const int ObsticleOffsetZ = 4;
    private const float DoubleObsticleOffset = 2.0f;
    private const float SingleObsticleOffset = 2.0f;

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

    public void CreateRandomObsticles()
    {
        for (sbyte i = -1; i <= 1; i += 2)
        {
            var rand = Random.Range(-1, 2);
            if (rand > 0)
            {
                //CreateRandomDoubleObsticle(i);
                CreateRandomSingeObsticle(i);
            }
            else
            {
                //CreateRandomSingeObsticle(i);
                CreateRandomDoubleObsticle(i);
            }
        }
    }

    private void CreateRandomDoubleObsticle(sbyte pos)
    {
        var obsticle = DoubleObsticles[Mathf.RoundToInt(Random.Range(0, DoubleObsticles.Length - 1))];

        var obstInst = Instantiate(obsticle, gameObject.transform, false) as GameObject;
        obstInst.transform.position += obstInst.transform.forward * pos * ObsticleOffsetZ;
        obstInst.transform.position += obstInst.transform.right *  Mathf.RoundToInt(Random.Range(-1, 2)) * DoubleObsticleOffset;
    }

    private void CreateRandomSingeObsticle(sbyte pos)
    {
        var obsticle = SingleObsticles[Mathf.RoundToInt(Random.Range(0, DoubleObsticles.Length - 1))];

        var obstInst = Instantiate(obsticle, gameObject.transform, false) as GameObject;
        obstInst.transform.position += obstInst.transform.forward * pos * ObsticleOffsetZ;
        obstInst.transform.position += obstInst.transform.right * SingleObsticleOffset * Mathf.Round(Random.Range(-1, 2));
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            _robot.TurnOnControlls();
        }
    }
}
