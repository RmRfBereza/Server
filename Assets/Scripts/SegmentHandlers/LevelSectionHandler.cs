using UnityEngine;
using System.Collections;

public class LevelSectionHandler : MonoBehaviour {

    public GameObject MappingSegment;
    protected GameObject _player = null;
    protected RobotMovement _robot = null;
    protected Level _level;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    protected void FindLevel()
    {
        _level = GameObject.Find("Plane").GetComponent<Level>();
    }

    protected void SetPlayer()
    {
        if (_player == null)
        {
            if (_level.IsPlayerInstanciated)
            {
                _player = _level.Player;
                _robot = _player.GetComponent<RobotMovement>();
            }
        }
    }

    protected void DrawMapping(int offsetX, int offsetY)
    {
        int directionX = offsetX < 0 ? -1 : 1;
        int directionY = offsetY < 0 ? -1 : 1;

        Vector3 mappingCenter = transform.position;
        mappingCenter += -transform.forward * (-CreateLevel.SegmentSize3d / 2) * offsetY;
        mappingCenter += -transform.right * CreateLevel.SegmentSize3d / 2 * directionX;
        int radius = CreateLevel.SegmentSize3d / 2 - offsetX * directionX;

        var testSegment = Instantiate(MappingSegment) as GameObject;
        float segmentLength = testSegment.GetComponent<MeshFilter>().mesh.bounds.size.z;
        float segmentDeg = Mathf.Asin(segmentLength / radius) * Mathf.Rad2Deg;

        float degDrawn = 0;

        Vector3 segmentPostion = mappingCenter;
        Vector3 rotation = Vector3.zero;

        while (degDrawn < GeometryBasic.RightAngleDeg)
        {
            segmentPostion += transform.right * (radius) * Mathf.Cos(degDrawn * Mathf.Deg2Rad) * directionX;
            segmentPostion += transform.forward * (-radius) * Mathf.Sin(degDrawn * Mathf.Deg2Rad) * directionY;
            var curentMappingSegment = Instantiate(MappingSegment, transform, false) as GameObject;
            curentMappingSegment.transform.position = segmentPostion;
            rotation.y = degDrawn * directionX * directionY + transform.eulerAngles.y;
            curentMappingSegment.transform.eulerAngles = rotation;
            segmentPostion = mappingCenter;
            degDrawn += segmentDeg;
        }
    }

    void OnTriggerEnter()
    {
        if (_player != null)
        {
            _robot.TurnOnControlls();
        }
    }
}
