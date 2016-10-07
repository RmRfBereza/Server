using System;
using UnityEngine;
using System.Collections;

public class RobotMovement : MonoBehaviour
{
    public static sbyte CurrentTrack;

    public float Thrust;
    public float Speed;
    public float JumpLength;

    private float _jumpHeight;
    private float _jumpPosition;
    private Vector3 jumpPreviousPosition;
    private Animator _animator;

    private const sbyte JumpCoefficient = 3;
    private const sbyte Left = -1;
    private const sbyte Right = 1;
    private const sbyte DefaultTrackNumber = 1;
    private const float Tolerance = 0.0001f;
    private const string State = "State";

    private enum States
    {
        Running, Jumping
    }

    private States _currentState;
    private States CurrentState {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
            _animator.SetInteger(State, (int)value);
        }
    }

    private Vector3 _movement;
    private Vector3 _rotation;

    private Vector3 _middlePosition;
    private Vector3 _newPosition;

    // Use this for initialization
    void Start ()
    {
        _animator = GetComponent<Animator>();
        CurrentTrack = DefaultTrackNumber;
        StartRunning();
        SetJumpParameters();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RunForward();
        HandleCurrentState();
	}

    public void SetRotation(int rotationY)
    {
        _rotation.Set(0f, rotationY, 0f);
        transform.eulerAngles = _rotation;
    }

    private void HandleCurrentState()
    {
        switch (CurrentState)
        {
            case States.Jumping:
                HandleJumping();
                break;
            case States.Running:
#if UNITY_EDITOR
                HandleKeyboard();
#endif
                break;
        }
    }

#if UNITY_EDITOR
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

    private void JumpKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleKeyboard()
    {
        MoveSidewaysKeyboard();
        TurnKeyboard();
        JumpKeyboard();
    }
#endif

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
        if (direction == Left && CurrentTrack != 0 || 
            direction == Right && CurrentTrack + direction < Level.TracksQuantity)
        {
            _movement.Set(direction * Level.TrackWidth, 0f, 0f);
            MoveInLocalCs(_movement);
            CurrentTrack += direction;
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

    private void SetJumpParameters()
    {
        _jumpHeight = Mathf.Pow(JumpLength, 2)/JumpCoefficient;
    }

    private void Jump()
    {
        transform.position = _newPosition;
        CurrentState = States.Jumping;
        _jumpPosition = 0;
        jumpPreviousPosition = transform.position;
    }

    private void HandleJumping()
    {
        //not using physics engine because jumping should be deterministic, avoyding physics can increase performance 
        _jumpPosition += Math.Abs(jumpPreviousPosition.x - transform.position.x) > Tolerance
            ? transform.position.x - jumpPreviousPosition.x
            : transform.position.z - jumpPreviousPosition.z;

        float currentJumpHeight = -Mathf.Pow((_jumpPosition - JumpLength)/2, 2) + _jumpHeight;

        if (currentJumpHeight > 0)
        {
            _newPosition.Set(transform.position.x, currentJumpHeight + 0.9f, transform.position.z);
            transform.position = _newPosition;
        }
        else
        {
            StartRunning();
        }

        jumpPreviousPosition = transform.position;
    }

    private void StartRunning()
    {
        CurrentState = States.Running;
    }
}
