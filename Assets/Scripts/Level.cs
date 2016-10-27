using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    public const byte TrackWidth = 2;
    public const byte TracksQuantity = 3;
    public enum Directions
    {
        Up = 0, Down = 180, Left = 270, Right = 90
    }

    public enum States
    {
        WaitingForPlayer, StartingGame, GameIsPlaying, GameOver
    }

    public States CurrentState = States.WaitingForPlayer;

    public bool isSecondPlayerConnected = false;

    public GameObject Player;
    public Camera MainCamera;

    public GameObject GameOverInterfaceGO;
    public GameObject SecondsTextGO;

    private const sbyte secondsBeforeStart = 3;
    private const string goText = "GO!";

    private RobotMovement _robot;
    private Text _secondsText;
    private bool ISplayerInstanciated = false;

	// Use this for initialization
	void Start ()
	{
        StartCoroutine(WaitForPlayer());
        _secondsText = SecondsTextGO.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    
	}

    public void RestartGame()
    {
        CurrentState = States.StartingGame;
        StartCoroutine(GameLoop());
    }

    public void NotifySecondPlayerConnected()
    {
        CurrentState = States.StartingGame;
    }

    public void NotifyGameOver()
    {
        CurrentState = States.GameOver;
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
        _robot.SetIdle();
        yield return null;
    }

    private void CreateMainPlayer()
    {
        if (!ISplayerInstanciated)
        {
            Player = Instantiate(Player);
            MainCamera.transform.parent = Player.transform;
            _robot = Player.GetComponent<RobotMovement>();
            ISplayerInstanciated = true;
        } else
        {
            Player.transform.position = Vector3.zero;
            Player.transform.eulerAngles = Vector3.zero;
        }
    }

    /*private void DeleteMainPlayer()
    {
        Destroy(Player);
    }*/
}
