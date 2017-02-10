using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public static Vector3 StartPosition;
    public static Vector3 StartRotation;

    public const byte TrackWidth = 2;
    public const byte TracksQuantity = 3;
    public const int DeadlyLayer = 8;

    public const string StraightSegmentName = "straight";
    public const string TurnSegmentName = "turn";
    public const string TturnSegmentName = "Tturn";
    public const string IntersectionSegmentName = "intersection";
    public const string DeadendSegmentName = "deadend";
    public const string StartSegmentName = "start";
    public const string FinishSegmentName = "finish";

    public enum Directions
    {
        Up = 0, Down = 180, Left = 270, Right = 90
    }

    public enum States
    {
        WaitingForPlayer, StartingGame, GameIsPlaying, GameOver, GameWon
    }

    public States CurrentState = States.WaitingForPlayer;

    [HideInInspector]
    public bool isSecondPlayerConnected = false;
    public bool IsPlayerInstanciated = false;

    //public GameObject Segment;
    public List<GameObject> SegmentList;
    public GameObject Player;
    public /*Camera*/GameObject SceneCamera;

    public GameObject SegmentStorage;

    public GameObject GameOverInterfaceGO;
    public GameObject SecondsTextGO;

    private const sbyte secondsBeforeStart = 3;
    private const string goText = "GO!";
    private const string gameOverText = "Game Over!";
    private const string gameWonText = "You're free";
    private const string singlePlayerPromptStart = "Locate the path\nLook up to start";
    private const string singlePlayerPromptQuit = "Look up to quit";
    private const string singlePlayerPromptContinue = "Look down to continue";
    private const string singlePlayerPromptRestart = "Look down to restart";

    private RobotMovement _robot;
    private Text _secondsText;
    //private List<GameObject> Segments;
    private Client2 _client2 = null;

    private Segment3dStorageHandler _storage;

    private Vector3 _rotation;
    private Vector3 _position;

    private bool _hasWon = true;

	// Use this for initialization
	void Start ()
	{
	    _storage = Instantiate(SegmentStorage).GetComponent<Segment3dStorageHandler>();
        //Segments = new List<GameObject>();
        if (SceneCamera == null) SceneCamera = GameObject.Find("VrCamera")/*Camera.main*/;
        //SegmentList = CreateLevel.CreateLevelFromJsonString(LevelManager.getInstance().getLevelAndInitIfNecessary(), CreateLevel.SegmentSize3d, SegmentPrefabsDictionary);
        StartCoroutine(WaitForPlayer());
	    StartCoroutine(testingLoop());
        _secondsText = SecondsTextGO.GetComponent<Text>();
        if (Config.isSingle)
            _secondsText.text = singlePlayerPromptStart;
        //var obj = TestTurn();
        //CreateLevel.CreateLevelFromJsonString(obj.ToString(), CreateLevel.SegmentSize3d, SegmentPrefabsDictionary);
    }

    private void PrepareLevel()
    {
        var start = GameObject.FindGameObjectWithTag("Start");
        StartPosition = start.transform.position;
        StartRotation = start.transform.eulerAngles;
    }

    public void RestartGame()
    {
        if (CurrentState == States.GameOver || CurrentState == States.GameWon || CurrentState == States.WaitingForPlayer)
        {
            Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //CreateMainPlayer();
            if (Config.isSingle)
            {
                CurrentState = States.WaitingForPlayer;

                _secondsText.text = singlePlayerPromptStart;
                StartCoroutine(WaitForPlayer());
                _robot.SetIdle();
                return;
            }
            CreateMainPlayerGaze();
            CurrentState = States.StartingGame;
            StartCoroutine(GameLoop());
        }
    }

    public void RunNextLevel()
    {
        if (CurrentState == States.GameOver || CurrentState == States.GameWon ||
            CurrentState == States.WaitingForPlayer)
        {
            if (!_hasWon)
            {
                RestartGame();
            }
            else
            {
                GameObject.Find("LevelCommons").GetComponent<LevelCommonsHandler>().LoadNextLevel();
            }
            /*else if (LevelManager.getInstance().incrementAndCheckLevel())
            {
                foreach (var segment in SegmentList)
                {
                    Destroy(segment);
                }
                SegmentList.Clear();

                SegmentList = CreateLevel.CreateLevelFromJsonString(LevelManager.getInstance().getLevelAndInitIfNecessary(),
                    CreateLevel.SegmentSize3d, SegmentPrefabsDictionary);

                RestartGame();
            }
            else
            {
                SceneManager.LoadScene(0);
            }*/
        }
    }

    public void NotifySecondPlayerConnected()
    {
        if (CurrentState == States.WaitingForPlayer)
        {
            CurrentState = States.StartingGame;
        }
    }

    public void NotifyGameOver()
    {
        if (CurrentState == States.GameIsPlaying)
        {
            CurrentState = States.GameOver;
        }
    }

    public void NotifyGameWon()
    {
        if (CurrentState == States.GameIsPlaying)
        {
            CurrentState = States.GameWon;
        }
    }

    private IEnumerator WaitForPlayer()
    {
        //CreateMainPlayer();
        CreateMainPlayerGaze();

        while (CurrentState == States.WaitingForPlayer)
        {
            yield return null;
        }

        yield return StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartCountDown(secondsBeforeStart));
        yield return StartCoroutine(StartGameplay());
        yield return StartCoroutine(Play());
        if (CurrentState == States.GameOver)
        {
            yield return StartCoroutine(GameOver(gameOverText, false));
        }
        else
        {
            yield return StartCoroutine(GameOver(gameWonText, true));
        }
    }

    private IEnumerator StartCountDown(sbyte _seconds)
    {
        sbyte seconds = _seconds;

        GameOverInterfaceGO.SetActive(false);

        while (seconds > 0)
        {
            _secondsText.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
            --seconds;
        }

        _secondsText.text = goText;
        yield return new WaitForSeconds(1f);
        _secondsText.text = string.Empty;
    }

    private IEnumerator StartGameplay()
    {
        _robot.StartRunningUncontrollably();
        CurrentState = States.GameIsPlaying;
        yield return null;
    }

    private IEnumerator Play()
    {
        while (CurrentState == States.GameIsPlaying)
        {
            yield return null; 
        }
    }

    private IEnumerator GameOver(string message, bool won)
    {
        if (_client2 == null) _client2 = Player.GetComponent<Client2>();
#if UNITY_EDITOR
        GameOverInterfaceGO.SetActive(true);
#endif
        print(message);
        _secondsText.text = message;
        _robot.SetDead();

        _hasWon = won;

        if (Config.isSingle)
        {

            _secondsText.text += '\n' + singlePlayerPromptQuit + '\n';
            _secondsText.text += won ? singlePlayerPromptContinue : singlePlayerPromptRestart;

            yield return null;
        }

        if (won)
        {
            _client2.SendWin();
        }
        else
        {
            _client2.SendGameover();
        }
        
        yield return null;
    }

    private void CreateMainPlayer()
    {
        if (!IsPlayerInstanciated)
        {
            Player = Instantiate(Player);
            SceneCamera.transform.parent = Player.transform;
            _robot = Player.GetComponent<RobotMovement>();
            IsPlayerInstanciated = true;
        }
        
        Player.transform.position = StartPosition;
        Player.transform.eulerAngles = StartRotation;
        //RobotMovement.CurrentTrack = 0;
    }

    private void CreateMainPlayerGaze()
    {
        if (!IsPlayerInstanciated)
        {
            Player = Instantiate(Player);
            SceneCamera.gameObject.AddComponent<GazeCameraHandler>();
            _robot = Player.GetComponent<RobotMovement>();
            IsPlayerInstanciated = true;
        }

        Player.transform.position = StartPosition;
        Player.transform.eulerAngles = StartRotation;
    }

    IEnumerator testingLoop()
    {
        while(true)
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                NotifySecondPlayerConnected();
            } else if (Input.GetKeyDown(KeyCode.G))
            {
                NotifyGameOver();
            } else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene(0);
            }
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                print("tap");
            }

            if (Math.Abs(Input.acceleration.y) > 0)
            {
                //NotifySecondPlayerConnected();
		        //RestartGame();
            }
#endif
            yield return null;
        }
    }

    /*private void DeleteMainPlayer()
    {
        Destroy(Player);
    }*/

    private JSONObject TestLongHall(int numberOfSegmentsInHall)
    {
        //test
        JSONObject obj = new JSONObject(JSONObject.Type.ARRAY);
        obj.Add(new JSONObject(JSONObject.Type.ARRAY));
        for (int i = 0; i < numberOfSegmentsInHall; ++i)
        {
            obj[0].Add(new JSONObject());
            obj[0][i].AddField("angle", 0f);
            obj[0][i].AddField("type", StraightSegmentName);
        }
        return obj;
    }

    private JSONObject TestTurn()
    {
        int lengthBeforeTurn = 3;
        var obj = TestLongHall(lengthBeforeTurn);

        obj[0].Add(new JSONObject());
        obj[0][lengthBeforeTurn].AddField("angle", 180f);
        obj[0][lengthBeforeTurn].AddField("type", IntersectionSegmentName);

        obj[0].Add(new JSONObject());
        obj[0][lengthBeforeTurn + 1].AddField("angle", 0f);
        obj[0][lengthBeforeTurn + 1].AddField("type", StraightSegmentName);

        int newTonnelLength = 5;

        for (int j = 0; j < newTonnelLength; ++j)
        {
            obj.Add(new JSONObject(JSONObject.Type.ARRAY));

            for (int i = 0; i < lengthBeforeTurn; ++i)
            {
                obj[j + 1].Add(new JSONObject(JSONObject.Type.NULL));
            }

            obj[j + 1].Add(new JSONObject());
            obj[j + 1][lengthBeforeTurn].AddField("angle", 90f);
            obj[j + 1][lengthBeforeTurn].AddField("type", StraightSegmentName);
        }
        return obj;
    }

    void Update()
    {
        //print("Camera angles " + SceneCamera.transform.eulerAngles);
    }
}
