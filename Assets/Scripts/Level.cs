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
    public Camera MainCamera;

    private RobotMovement _robot;

	// Use this for initialization
	void Start ()
	{
        CreateMainPlayer();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	}
    
    public void CreateMainPlayer()
    {
        Player = Instantiate(Player);
        MainCamera.transform.parent = Player.transform;
        _robot = Player.GetComponent<RobotMovement>();
    }
}
