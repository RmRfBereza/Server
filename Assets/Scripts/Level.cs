using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

using System.CodeDom;
using System.CodeDom.Compiler;

public class Level : MonoBehaviour {

    public const byte TrackWidth = 2;
    public const byte TracksQuantity = 3;

    public const string StraightSegmentName = "straight";
    public const string TurnSegmentName = "turn";
    public const string TturnSegmentName = "Tturn";
    public const string IntersectionSegmentName = "intersection";

    public enum Directions
    {
        Up = 0, Down = 180, Left = 270, Right = 90
    }

    public enum States
    {
        WaitingForPlayer, StartingGame, GameIsPlaying, GameOver
    }

    public States CurrentState = States.WaitingForPlayer;

    [HideInInspector]
    public bool isSecondPlayerConnected = false;
    public bool IsPlayerInstanciated = false;

    public GameObject Segment;
    public List<NamedPrefabs> SegmentPrefabsList;
    public GameObject Player;
    public Camera SceneCamera;

    public GameObject GameOverInterfaceGO;
    public GameObject SecondsTextGO;

    private const sbyte secondsBeforeStart = 3;
    private const string goText = "GO!";
    private const string gameOverText = "Game Over!";

    private Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();
    private RobotMovement _robot;
    private Text _secondsText;
    private List<GameObject> Segments;

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
    }

    public void RestartGame()
    {
        if (CurrentState == States.GameOver)
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

    private IEnumerator WaitForPlayer()
    {
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
        yield return StartCoroutine(GameOver());
    }

    private IEnumerator StartCountDown(sbyte _seconds)
    {
        sbyte seconds = _seconds;

        GameOverInterfaceGO.SetActive(false);

        CreateMainPlayer();

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
        _robot.StartRunning();
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

    private IEnumerator GameOver()
    {
        GameOverInterfaceGO.SetActive(true);
        _secondsText.text = gameOverText;
        _robot.SetIdle();
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
        } else
        {
            Player.transform.position = Vector3.zero;
            Player.transform.eulerAngles = Vector3.zero;
        }
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
        obj[0][lengthBeforeTurn].AddField("type", TurnSegmentName);

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
    public struct NamedPrefabs
    {
        public string name;
        public GameObject prefab;
    }
}
