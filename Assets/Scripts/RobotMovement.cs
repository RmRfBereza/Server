using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RobotMovement : MonoBehaviour
{
    //public static sbyte CurrentTrack;

    public float Thrust;
    public float Speed;
    public float JumpLength;

    private float _jumpHeight;
    private float _jumpPosition;
    private Vector3 _jumpPreviousPosition;

    private int _turnStartRotation;
    private float _turnRotation = 0;
    private float _turningSpeedDeg = 36f;
    private byte _ticks = 0;
    private Vector3 _turnigCenterPosition;
    private Vector3 _turningRight;
    private Vector3 _turningForward;

    private Animator _animator;
    private Level _level;
    private Text _secondsText;

    private const sbyte JumpCoefficient = 3;
    private const sbyte Left = -1;
    private const sbyte Right = 1;
    //private const sbyte DefaultTrackNumber = 0;
    private const float Tolerance = 0.0001f;
    private const string State = "State";
    private const string NoControl = "<color=#800000ff>Warning:\nNO Control!</color>";
    private const byte ticksBeforeChangeTrack = 15;

    //order can not be changed because of the animator
    private enum States
    {
        Running=10, RunningUnconrollably, Jumping, Idle, Turning, Dead
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
        _level = GameObject.Find("Plane").GetComponent<Level>();
        _secondsText = GameObject.Find("SecondsText").GetComponent<Text>();

        subscribeXAction();
        _animator = GetComponent<Animator>();
        //CurrentTrack = DefaultTrackNumber;
        SetIdle();
        SetJumpParameters();
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleCurrentState();
    }

    void FixedUpdate()
    {
        if (_ticks > 0) --_ticks;
    }

    public void TurnOnControlls()
    {
        if (CurrentState == States.RunningUnconrollably)
        {
            CurrentState = States.Running;
            _secondsText.text = "";
        }
    }

    public void StartRunning()
    {
        CurrentState = States.Running;
        _secondsText.text = "";
    }

    public void StartRunningUncontrollably()
    {
        CurrentState = States.RunningUnconrollably;
        _secondsText.text = NoControl;
    }

    public void SetIdle()
    {
        CurrentState = States.Idle;
        //_secondsText.text = "";
    }

    public void SetDead()
    {
        CurrentState = States.Dead;
        //_secondsText.text = "";
    }

    public void SetRotation(float rotationY)
    {
        _rotation.Set(0f, rotationY, 0f);
        transform.eulerAngles = _rotation;
    }

    private void HandleCurrentState()
    {
        switch (CurrentState)
        {
            case States.Jumping:
                RunForward();
                HandleJumping();
                break;
            case States.Running:
                TurnWithCamera();
                RunForward();
#if UNITY_EDITOR
                HandleKeyboard();
#endif
                break;
            case States.Turning:
                TurnWithCamera();
                RunForward();
#if UNITY_EDITOR
                HandleKeyboard();
#endif
                //HandleTurning();
                break;
            case States.RunningUnconrollably:
                TurnWithCamera();
                RunForward();
#if UNITY_EDITOR
                HandleKeyboard();
#endif
                break;
            case States.Idle:
                CheckStart();
                break;
            case States.Dead:
                CheckRestart();
                CheckQuit();
                break;
        }
    }

    private void CheckQuit()
    {
        if (Config.isSingle)
        {
            var angle = Camera.main.gameObject.transform.eulerAngles.x;
            if (angle > 40 && angle < 50) SceneManager.LoadScene(0);
        }
    }

    private void CheckRestart()
    {
        if (Config.isSingle)
        {
            var angle = Camera.main.gameObject.transform.eulerAngles.x;
            if (angle < 320 && angle > 310) _level.RunNextLevel();
        }
    }

    private void CheckStart()
    {
        if (Config.isSingle)
        {
            var angle = Camera.main.gameObject.transform.eulerAngles.x;
            _secondsText.text = angle.ToString();
            if (angle < 320 && angle > 310) _level.CurrentState = Level.States.StartingGame;
        }
    }

    private void TurnWithCamera()
    {
#if UNITY_EDITOR
        return;
#endif
        _rotation.Set(0, Camera.main.gameObject.transform.eulerAngles.y, 0);
        transform.eulerAngles = _rotation;
    }

#if UNITY_EDITOR
    /*private void MoveSidewaysKeyboard()
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
            //Turn(-GeometryBasic.RightAngleDeg);
            StartTurning();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Turn(GeometryBasic.RightAngleDeg);
            StartTurning();
        }
    }*/

    private void JumpKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void TurnKeyboardGaze()
    {
        if (Input.GetKey(KeyCode.A))
        {
            print("Turning right");
            SetRotation(transform.eulerAngles.y - 1);
            Camera.main.transform.eulerAngles = transform.eulerAngles;
        }

        if (Input.GetKey(KeyCode.D))
        {
            print("Turning left");
            SetRotation(transform.eulerAngles.y + 1);
            Camera.main.transform.eulerAngles = transform.eulerAngles;
        }
    }

    private void HandleKeyboard()
    {
        //MoveSidewaysKeyboard();
        //TurnKeyboard();
        JumpKeyboard();
        TurnKeyboardGaze();
    }
#endif

    private void Turn(float rotationY)
    {
        _rotation.Set(0f, rotationY, 0f);
        transform.Rotate(_rotation);
    }

    private void RunForward()
    {
        transform.position += transform.forward*Speed*Time.deltaTime;
    }

    /*private void ChangeTrack(sbyte direction)
    {
        if (_ticks > 0) return;
        if (CurrentState != States.Running) return;
        _ticks = ticksBeforeChangeTrack;
        if (direction == Left && CurrentTrack > -1 || 
            direction == Right && CurrentTrack < 1)
        {
            transform.position += transform.right*direction*Level.TrackWidth;
            CurrentTrack += direction;
        }
    }*/
	void subscribeXAction()
    {
        XController.OnXLeft  += OnXLeft;
        XController.OnXRight += OnXRight;
    }
	
	void OnXLeft()
	{
		//ChangeTrack(Left);
	}
	
	void OnXRight()
	{
		//ChangeTrack(Right);
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
        _jumpPreviousPosition = transform.position;
    }

    private void HandleJumping()
    {
        //not using physics engine because jumping should be deterministic, avoyding physics can increase performance 
        _jumpPosition += Math.Abs(_jumpPreviousPosition.x - transform.position.x) > Tolerance
            ? transform.position.x - _jumpPreviousPosition.x
            : transform.position.z - _jumpPreviousPosition.z;

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

        _jumpPreviousPosition = transform.position;
    }

    /*public void StartTurning()
    {
        if (_currentState == States.Turning || _currentState == States.Dead) return;
        _secondsText.text = NoControl;
        if (CurrentTrack == 0)
        {
            StartRunningUncontrollably();
            return;
        }
        _turningForward = transform.forward;
        _turningRight = transform.right;
        _turnStartRotation = Mathf.FloorToInt(transform.eulerAngles.y);
        _turnigCenterPosition = transform.position;
        _turnigCenterPosition += transform.right * CurrentTrack * 7;
        _currentState = States.Turning;
    }

    private void HandleTurning()
    {
        _turnRotation += _turningSpeedDeg * Time.deltaTime;

        transform.position = _turnigCenterPosition +
                             _turningRight*(-1) * CurrentTrack * 7*Mathf.Cos(CurrentTrack * _turnRotation*Mathf.Deg2Rad)
                             + _turningForward*7*Mathf.Sin(_turnRotation * Mathf.Deg2Rad);

        SetRotation(_turnStartRotation + _turnRotation * CurrentTrack);

        if (_turnRotation >= GeometryBasic.RightAngleDeg)
        {
            SetRotation(_turnStartRotation + CurrentTrack * GeometryBasic.RightAngleDeg);
            _turnRotation = 0;
            StartRunning();
        }
    }*/

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == Level.DeadlyLayer)
        {
            CurrentState = States.Dead;
            _level.NotifyGameOver();
        }
        
    }
}
