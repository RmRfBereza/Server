using System;
using UnityEngine;
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

    public GameObject Segment;
    public List<NamedPrefab> SegmentPrefabsList;
    public GameObject Player;
    public Camera SceneCamera;

    public GameObject GameOverInterfaceGO;
    public GameObject SecondsTextGO;

    private const sbyte secondsBeforeStart = 3;
    private const string goText = "GO!";
    private const string gameOverText = "Game Over!";
    private const string gameWonText = "You're free";

    private Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();
    private RobotMovement _robot;
    private Text _secondsText;
    private List<GameObject> Segments;
    private Client2 _client2 = null;

    private Vector3 _rotation;
    private Vector3 _position;

	// Use this for initialization
	void Start ()
    {
        foreach (var segment in SegmentPrefabsList)
        {
            SegmentPrefabsDictionary[segment.name] = segment.prefab;
        }

        Segments = new List<GameObject>();
        if (SceneCamera == null) SceneCamera = Camera.main;
        StartCoroutine(WaitForPlayer());
	    StartCoroutine(testingLoop());
        _secondsText = SecondsTextGO.GetComponent<Text>();

        //var obj = TestTurn();
        //CreateLevel.CreateLevelFromJsonString(obj.ToString(), CreateLevel.SegmentSize3d, SegmentPrefabsDictionary);
        CreateLevel.CreateLevelFromJsonString("[[null,{\"angle\":270,\"type\":\"start\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":180,\"type\":\"turn\"}],[null,{\"angle\":180,\"type\":\"deadend\"},null,{\"angle\":90,\"type\":\"straight\"}],[{\"angle\":270,\"type\":\"finish\"},{\"angle\":0,\"type\":\"intersection\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":90,\"type\":\"tturn\"}],[null,{\"angle\":0,\"type\":\"deadend\"},null,{\"angle\":0,\"type\":\"deadend\"}]]", CreateLevel.SegmentSize3d, SegmentPrefabsDictionary);
    }

    public void RestartGame()
    {
        CreateMainPlayer();
        if (CurrentState == States.GameOver || CurrentState == States.WaitingForPlayer)
        {
            CurrentState = States.StartingGame;
            StartCoroutine(GameLoop());
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
        CreateMainPlayer();

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
        GameOverInterfaceGO.SetActive(true);
        print(message);
        _secondsText.text = message;
        _robot.SetIdle();
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
                Application.LoadLevel(0);
            }
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                print("tap");
            }

            if (Math.Abs(Input.acceleration.y) > 0)
            {
                NotifySecondPlayerConnected();
		RestartGame();
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

    [Serializable]
    public struct NamedPrefab
    {
        public string name;
        public GameObject prefab;
    }
}
