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
    private const string gameOverText = "Game Over!";

    private RobotMovement _robot;
    private Text _secondsText;
    private bool ISplayerInstanciated = false;

	// Use this for initialization
	void Start ()
	{
        StartCoroutine(WaitForPlayer());
        StartCoroutine(testingLoop());
        _secondsText = SecondsTextGO.GetComponent<Text>();
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

#if UNITY_EDITOR
    IEnumerator testingLoop()
    {
        while(true)
        {
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
            yield return null;
        }
    }
#endif

    /*private void DeleteMainPlayer()
    {
        Destroy(Player);
    }*/
}
