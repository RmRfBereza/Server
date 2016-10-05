using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public const byte TrackWidth = 2;
    public const byte TracksQuantity = 3;
    public enum Directions
    {
        Up = 0, Down = 180, Left = 270, Right = 90
    }

    public GameObject Player;
    public Camera MCamera;

    private RobotMovement _robot;
    private Vector3 _position;
    private Vector3 _rotation;

	// Use this for initialization
	void Start ()
	{
	    _robot = Player.GetComponent<RobotMovement>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _position = Player.transform.position;
	    _position.y = 5;

        MCamera.transform.position = _position;

	    float switcher = Random.value;

	    if (switcher > 0.99)
	    {
	        TurnPlayer(Directions.Down);
            print("down");
	    }
	    else if (switcher > 0.95)
	    {
	        TurnPlayer(Directions.Left);
            print("left");
	    } else if (switcher > 0.9)
	    {
	        TurnPlayer(Directions.Right);
            print("Right");
	    }
	    else if (switcher > 0.86)
	    {
	        TurnPlayer(Directions.Up);
            print("Up");
	    }
	}

    public void TurnPlayer(Directions direction)
    {
        _robot.SetRotation((int)direction);
        _rotation.Set(90, (float)direction, 0f);
        MCamera.transform.eulerAngles = _rotation;
    }
}
