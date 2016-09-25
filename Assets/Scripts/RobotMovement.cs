using UnityEngine;
using System.Collections;

public class RobotMovement : MonoBehaviour {

    private const float Speed = 2.5f;
    private const sbyte Left = -1;
    private const sbyte Right = 1;
    private const sbyte DefaultTrackNumber = 1;
    
    //TODO Do we need new Vector3()?
    private Vector3 _movement = new Vector3();
    private Vector3 _rotation = new Vector3();

    private Vector3 _middlePosition;
    private Vector3 _newPosition;
    public static sbyte _currentTrack;

    // Use this for initialization
    void Start () {
        _currentTrack = DefaultTrackNumber;
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveSidewaysKeyboard();
        TurnKeyboard();
        RunForward();
	}

    private void MoveSidewaysKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeTrack(Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeTrack(Right);
        }
    }

    private void TurnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Turn(-GeometryBasic.RightAngleDeg);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Turn(GeometryBasic.RightAngleDeg);
        }
    }

    private void Turn(float rotationY)
    {
        _rotation.Set(0f, rotationY, 0f);
        transform.Rotate(_rotation);
    }

    private void RunForward()
    {
        _movement.Set(0f, 0f, Speed * Time.deltaTime);
        MoveInLocalCs(_movement);
    }

    private void ChangeTrack(sbyte direction)
    {
        if (direction == Left && _currentTrack != 0 || 
            direction == Right && _currentTrack + direction < Level.tracksQuantity)
        {
            _movement.Set(direction * Level.trackWidth, 0f, 0f);
            MoveInLocalCs(_movement);
            _currentTrack += direction;
        }
    }

    private void MoveInLocalCs(Vector3 movement)
    {
        GeometryBasic.RotateCs(transform.position, out _middlePosition, -gameObject.transform.eulerAngles.y);
        _middlePosition += movement;
        GeometryBasic.RotateCs(_middlePosition, out _newPosition, gameObject.transform.eulerAngles.y);
        
        //TODO Why doesn't it work?
        //_playerBody.MovePosition(_newPosition);

        transform.position = _newPosition;
    }
}
