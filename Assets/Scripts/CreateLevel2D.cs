using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateLevel2D : MonoBehaviour {

    public const int SegmentSize2d = 12;
    public GameObject Player;
    public GameObject StarGameButton;
    public GameObject TileIntersection;
    public GameObject TileTurn;
    public GameObject TileTTurn;
    public GameObject TileStraight;
    public GameObject TileNull;
    public GameObject TileDeadend;
    private Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();
    private string jsonString;

    public enum States
    {
        WaitingForPlayer, StartingGame, GameIsPlaying, GameOver
    }

    private States CurrentState = States.WaitingForPlayer;

    void Start()
    {
        SegmentPrefabsDictionary.Add("straight", TileStraight);
        SegmentPrefabsDictionary.Add("turn", TileTurn);
        SegmentPrefabsDictionary.Add("Tturn", TileTTurn);
        SegmentPrefabsDictionary.Add("intersection", TileIntersection);
        SegmentPrefabsDictionary.Add("null", TileNull);
        jsonString = getJSONString();
        //Create2DLevelFromJsonString(jsonString, SegmentSize2d, SegmentPrefabsDictionary);
        StartCoroutine(WaitForPlayer());
        StartCoroutine(testingLoop());
    }

    private string getJSONString()
    {
        return "[[{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":180,\"type\":\"turn\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}]";
    }

    public void Create2DLevelFromJsonString(string jsonString, int segmentSize, Dictionary<string, GameObject> prefabDictionary)
    {
        Vector3 _position = Vector3.zero;
        Vector3 _rotation = Vector3.zero;

        var levelMapping = new JSONObject(jsonString);

        foreach (JSONObject segmentRaw in levelMapping.list)
        {
            _position.x = 0;
            foreach (JSONObject segmentParams in segmentRaw.list)
            {
                if (segmentParams.type == JSONObject.Type.NULL)
                {
                    var nullSegment = Instantiate(prefabDictionary["null"]);
                    _rotation.Set(0f, 0f, 0f);
                    nullSegment.transform.eulerAngles = _rotation;
                    nullSegment.transform.position = _position;
                    _position.x += segmentSize;
                    continue;
                }
                var currentSegment = Instantiate(prefabDictionary[segmentParams["type"].str]);

                //_rotation.Set(0f, segmentParams["angle"].n, 0f);
                _rotation.Set(0f, 0f, segmentParams["angle"].n);
                currentSegment.transform.eulerAngles = _rotation;
                currentSegment.transform.position = _position;

                _position.x += segmentSize;
            }
            _position.y -= segmentSize;
        }
    }

    private void RestartGame()
    {
        SetPlayerStartPosition();
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
        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameOver());
    }

    private IEnumerator StartGame()
    {
        StarGameButton.SetActive(true);
        SetPlayerStartPosition();
        while (CurrentState == States.StartingGame)
        {
            yield return null;
        }
        StarGameButton.SetActive(false);
        Create2DLevelFromJsonString(jsonString, SegmentSize2d, SegmentPrefabsDictionary);
        yield return null;
    }

    private IEnumerator GamePlaying()
    {
        while (CurrentState == States.GameIsPlaying)
        {
            yield return null;
        }
    }

    public void NotifySecondPlayerConnected()
    {
        if (CurrentState == States.WaitingForPlayer)
        {
            CurrentState = States.StartingGame;
        }
    }

    private void SetPlayerStartPosition()
    {
        Player.transform.position = Vector3.zero;
        Player.transform.eulerAngles = Vector3.zero;
    }

    private IEnumerator GameOver()
    {
        yield return null;
    }

    public void NotifyGameOver()
    {
        if (CurrentState == States.GameIsPlaying)
        {
            CurrentState = States.GameOver;
        }
    }

    public void NotifyGameStart()
    {
        if (CurrentState == States.StartingGame)
        {
            CurrentState = States.GameIsPlaying;
        }
    }

    IEnumerator testingLoop()
    {
        while (true)
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                NotifySecondPlayerConnected();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                NotifyGameOver();
            }
            else if (Input.GetKeyDown(KeyCode.R))
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
#endif
            yield return null;
        }
    }

}
