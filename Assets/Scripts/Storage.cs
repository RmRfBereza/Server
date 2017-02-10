using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public GameObject[] SingleObsticles;
    public GameObject[] DoubleObsticles;

    private const float DoubleObsticleOffset = 1.0f;
    private const float SingleObsticleOffset = 2.0f;

    private List<ObsticleMeta> _obsticleTypes = new List<ObsticleMeta>();

    private static Storage _instance;
    // Use this for initialization

    void Awake()
    {
        _instance = this;

        float[] doublePositions = {-1 * DoubleObsticleOffset, DoubleObsticleOffset};
        _obsticleTypes.Add(new ObsticleMeta(doublePositions, DoubleObsticles));

        float[] singlePositions = { -1 * SingleObsticleOffset, 0, SingleObsticleOffset };
        _obsticleTypes.Add(new ObsticleMeta(singlePositions, SingleObsticles));
    }

    void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Storage getInstance()
    {
        return _instance;
    }

    public ObsticleMeta getRandomObsticleMeta()
    {
        return _obsticleTypes[Random.Range(0, _obsticleTypes.Count)];
    }

    public class ObsticleMeta
    {
        public ObsticleMeta(float[] positionsInLine, GameObject[] prefabsStorage)
        {
            PositionsInLine = positionsInLine;
            PrefabsStorage = prefabsStorage;
        }

        public float[] PositionsInLine { get; private set; }
        public GameObject[] PrefabsStorage { get; private set; }

        public GameObject getPrefab()
        {
            return PrefabsStorage[Random.Range(0, PrefabsStorage.Length)];
        }

        public float getPositionInLine()
        {
            return PositionsInLine[Random.Range(0, PositionsInLine.Length)];
        }
    }
}
