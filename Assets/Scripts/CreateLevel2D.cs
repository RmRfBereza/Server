using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreateLevel2D : MonoBehaviour {

    public const int SegmentSize2d = 12;
    public GameObject Player;
    public GameObject StarGameButton;
    public GameObject RestarGameButton;
    public GameObject TileIntersection;
    public GameObject TileTurn;
    public GameObject TileTTurn;
    public GameObject TileStraight;
    public GameObject TileNull;
    public GameObject TileDeadend;
    public GameObject TileStart;
    public GameObject TileFinish;

    public Text MessageText;

    private Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();
    private string jsonString;

    private const string StartGameMessage = "Second player connected!\nStart the game whenever you both are ready!";
    private const string WaitForPlayerMessage = "Tell your friend your IP and wait for him to connect. Your IP is\n";
    private const string GameOverMessage = "Game Over!";
    private const string YouWonMesssage = "You Won!";

    public enum States
    {
        WaitingForPlayer, StartingGame, GameIsPlaying, GameOver, GameWon
    }

    private States CurrentState = States.WaitingForPlayer;

    void Start()
    {
        SegmentPrefabsDictionary.Add("straight", TileStraight);
        SegmentPrefabsDictionary.Add("turn", TileTurn);
        SegmentPrefabsDictionary.Add("Tturn", TileTTurn);
        SegmentPrefabsDictionary.Add("intersection", TileIntersection);
        SegmentPrefabsDictionary.Add("null", TileNull);
        SegmentPrefabsDictionary.Add("start", TileStart);
        SegmentPrefabsDictionary.Add("finish", TileFinish);
        SegmentPrefabsDictionary.Add("deadend", TileDeadend);
        jsonString = getJSONString();
        //Create2DLevelFromJsonString(jsonString, SegmentSize2d, SegmentPrefabsDictionary);
        StartCoroutine(WaitForPlayer());
        StartCoroutine(testingLoop());
    }

    private string getJSONString()
    {
        //return "[[{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":180,\"type\":\"turn\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}]";
        //return "[[{\"angle\":270,\"type\":\"start\"},{\"angle\":180,\"type\":\"turn\"}],[{\"angle\":270,\"type\":\"finish\"},{\"angle\":90,\"type\":\"turn\"}]]";
        return "[[{\"angle\":270,\"type\":\"start\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":180,\"type\":\"turn\"}],[{\"angle\":0,\"type\":\"null\"},{\"angle\":0,\"type\":\"null\"},{\"angle\":180,\"type\":\"deadend\"},{\"angle\":90,\"type\":\"straight\"}],[{\"angle\":270,\"type\":\"finish\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"intersection\"},{\"angle\":90,\"type\":\"Tturn\"}],[{\"angle\":0,\"type\":\"null\"},{\"angle\":0,\"type\":\"null\"},{\"angle\":0,\"type\":\"deadend\"},{\"angle\":0,\"type\":\"deadend\"}]]";
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

    public void RestartGame()
    {
        if (CurrentState == States.GameOver || CurrentState == States.GameWon)
        {
            print("restart");
            RestarGameButton.SetActive(false);
            ActivateMessageText(false);
            SetPlayerStartPosition();
            CurrentState = States.GameIsPlaying;
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator WaitForPlayer()
    {
        MessageText.text = WaitForPlayerMessage + Server2.myIp;
        while (CurrentState == States.WaitingForPlayer)
        {
            yield return null;
        }
        Create2DLevelFromJsonString(jsonString, SegmentSize2d, SegmentPrefabsDictionary);

        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GamePlaying());
        if (CurrentState == States.GameOver)
        {
            yield return StartCoroutine(GameOver());
        }
        else
        {
            yield return StartCoroutine(GameWon());
        }
    }

    private IEnumerator StartGame()
    {
        MessageText.text = StartGameMessage;
        StarGameButton.SetActive(true);
        SetPlayerStartPosition();
        while (CurrentState == States.StartingGame)
        {
            yield return null;
        }
        ActivateMessageText(false);
        StarGameButton.SetActive(false);
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
        ActivateMessageText(true);
        MessageText.text = GameOverMessage;
        RestarGameButton.SetActive(true);
        yield return null;
    }

    private IEnumerator GameWon()
    {
        ActivateMessageText(true);
        MessageText.text = YouWonMesssage;
        RestarGameButton.SetActive(true);
        yield return null;
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

    public void NotifyGameStart()
    {
        if (CurrentState == States.StartingGame)
        {
            CurrentState = States.GameIsPlaying;
        }
    }

    private void ActivateMessageText(bool value)
    {
        MessageText.gameObject.transform.parent.gameObject.SetActive(value);
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
            else if (Input.GetKeyDown(KeyCode.W))
            {
                NotifyGameWon();
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
