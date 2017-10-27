using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* ------- Heart of the Game -------
 * The GameManager gets loaded by the Loader-script, it instantiates
 * an instance of the GameManager and after that the GameManger takes over.
 *
 * It has four Controllers attached, which all prepare a part of the game.
 * GridController: instantiates (almost) all GameObjects including Grid, Player, Enemies...
 * TimerController: the computing of the passed time
 * CameraController: set cameras position or field of view
 * PowerController: the collectables management, what powerup spawns etc.
 *
 * further possible implementation
 *
 * EnemyController
 * which manages all the enemies at the moment they are being spawned in the
 * GridController, which is alright, but most of the Enemies have their own
 * scripts and it is a little 'confusing'/all-over-the-place
 *
 * DatabaseController
 * which exists kind of. I wanted to implement a Database online, but didn't have much
 * success, due to other time consuming projects and thought its alright if its not
 * implemented.
 * what I did manage to implement is fetching data from a local database and post them
 * to the highscore panel (see more in Database/README.txt)
 *
 * other than that the GM gets as already said the time by the TimerController and then
 * plots it to the canvas or:
 * - inbetween level images (random)
 * - delays
 * - sets difficulty
 * - game pausing
 * - game over management
 *
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GridController gridcontroller;
    public TimerController timercontroller;
    public CameraController cameracontroller;
    public PowerController powercontroller;

    public static int level = 1;
    public static int difficulty;
    public const int DELAY_LEVELRESTART = 1;
    public const int DELAY_LEVELSTART = 2;
    public const int DELAY_TIMER = 3;

    public static int numOfJumps;
    public static int numOfShields;
    public int numOfComets;
    public float enemyPushPower = 2000;
    public const int MAXPOWER = 7;

    [System.NonSerialized]
    public bool gamepaused; // cant be set in inspector

    private Text textLevel;
    private Text textTimeValue;
    private Text textLevelHUDValue;
    private Text textTotalTimeValue;

    private GameObject imageLevel;
    private RawImage textureImageLevel;
    private GameObject buttonHome;
    private GameObject canvasHUD;
    private GameObject canvasESC;

    public Texture[] img; // image array for levelimage

    private void Awake()
    {
		if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

		/* ***************** Get Controllers ***************** */
		gridcontroller = GetComponent<GridController>();
        timercontroller = GetComponent<TimerController>();
        cameracontroller = GetComponent<CameraController>();
        powercontroller = GetComponent<PowerController>();
    }

    /* Entry point - when user starts game by clicking 'Start' in gamemenu */
    private void OnLevelWasLoaded(int index)
    {
        /* reset the level time*/
        timercontroller.ResetTimer();
        InitializeGame();
    }

    private void InitializeGame()
    {
        /* ***************** Getting references ***************** */
        buttonHome = GameObject.Find("ButtonHome");
        imageLevel = GameObject.Find("ImageLevel");
        textureImageLevel = imageLevel.GetComponent<RawImage>();
        textLevel = GameObject.Find("TextLevel").GetComponent<Text>();
        textTimeValue = GameObject.Find("TextTimeValue").GetComponent<Text>();
        textLevelHUDValue = GameObject.Find("TextLevelHUDValue").GetComponent<Text>();
        textTotalTimeValue = GameObject.Find("TextTotaltimeValue").GetComponent<Text>();
        canvasESC = GameObject.Find("CanvasESC");
        canvasHUD = GameObject.Find("CanvasHUD");

        powercontroller.GetReferences();

        /* ***************** Setting things up ***************** */
        powercontroller.SetPowers(PowerController.Powers.jump);
        powercontroller.SetPowers(PowerController.Powers.shield);

        difficulty = (int)Mathf.Log(level, 2f);
        numOfComets = difficulty + 4;

        textTotalTimeValue.text = timercontroller.GetTotalTime().ToString("##.##");
        textLevel.text = "Level: " + level;
        textLevelHUDValue.text = level.ToString();
        buttonHome.SetActive(false);
        SetRandomLevelImage();
        imageLevel.SetActive(true);
        canvasESC.SetActive(false);

		/* ***************** start level image ***************** */
		Invoke("HideLevelImage", DELAY_LEVELSTART);

		/* ***************** building level ***************** */
		gridcontroller.BuildLevel();

        /* needs to be after BuildLevel() - so the player can reference to 2nd camera (which gets instantiated in BuildLevel() */
        cameracontroller.GetReferences();
        cameracontroller.SetCameraPosition();

        Invoke("StartTimer", DELAY_TIMER);
    }

    private void Update()
    {
        /* set time to view */
        textTimeValue.text = timercontroller.GetTime().ToString("##.##");

        /*  Escape Menu (Pause) */
        if (Input.GetKeyDown(KeyCode.Escape))
            SetGameStatus();

        /*  Set camera */
        if (Input.GetKeyDown("1"))
            cameracontroller.SetCamera(1);
        if (Input.GetKeyDown("2"))
            cameracontroller.SetCamera(2);
    }

	/* ***************** when pressing 'ESC' ***************** */
	private void SetGameStatus()
    {
        if(gamepaused)
        {
            canvasESC.SetActive(false);
            canvasHUD.SetActive(true);
            gamepaused = false;
            Time.timeScale = 1;
        }
        else
        {
            canvasESC.SetActive(true);
            canvasHUD.SetActive(false);
            gamepaused = true;
            Time.timeScale = 0;
        }
    }

	/* ***************** Called when reached end zone ***************** */
	public void LevelFinish()
    {
        timercontroller.StopTimer();
        level++;
    }

	/* ***************** Called when dead ***************** */
	public void GameOver()
    {
        timercontroller.StopTimer();
        textLevel.text = "Game Over\nYour time was: " + timercontroller.GetTotalTime().ToString("##.##") + "\n You reached level: " + level;
        imageLevel.SetActive(true);
        buttonHome.SetActive(true);
        ResetValues();
        enabled = false;
        Destroy(instance);
    }

	/* ***************** just to have a random image in between ***************** */
	private void SetRandomLevelImage()
    {
        int random = Random.Range(0, img.Length);

        textureImageLevel.texture = img[random];
    }

	/* ***************** delay for level inbetween ***************** */
	private void HideLevelImage()
    {
        imageLevel.SetActive(false);
    }

	/* ***************** start timer after delay ***************** */
	private void StartTimer()
    {
        timercontroller.SetTimer();
    }

    private void ResetValues()
    {
        level = 1;
        numOfJumps = 0;
        numOfShields = 0;
        timercontroller.ResetTotalTimer();
    }
}
