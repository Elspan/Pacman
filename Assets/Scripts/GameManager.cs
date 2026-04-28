using System.Collections;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pacman;

    public GameObject leftWarpNode;
    public GameObject rightWarpNode;
    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public AudioSource powerPelletAudio;
    public AudioSource startGameAudio;
    public AudioSource deathAudio;

    public int currentMunch;

    public int score;
    public Text scoreText;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject pinkGhost;
    public GameObject blueGhost;
    public GameObject orangeGhost;

    public EnemyController redGhostController;
    public EnemyController pinkGhostController;
    public EnemyController blueGhostController;
    public EnemyController orangeGhostController;

    public int totalPellets;
    public int pelletsLeft;
    public int pelletsCollectedOnThisLife;

    public bool hadDeathOnThisLevel = false;

    public bool gameIsRunning;

    public List<NodeController> nodeControllers = new List<NodeController>();

    public bool newGame;
    public bool clearedLevel;
    
    public int lives;
    public int currentLevel;

    public Image blackBackground;

    public Text gameOverText;
    
    public bool isPowerPelletRunning = false;
    public float currentPowerPelletTime = 0;
    public float powerPelletTimer = 8f;
    public int powerPelletMultiplier = 1;

    public enum GhostMode
    {
        chase,
        scatter,
    }

    public GhostMode currentGhostMode;

    public int[] ghostModeTimers = new int[] { 7, 20, 7, 20, 5, 20, 5 };
    public int ghostModeTimerIndex;
    public float ghostModeTimer = 0;
    public bool runningTimer;
    public bool completedTimer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        newGame = true;
        clearedLevel = false;
        blackBackground.enabled = false;

        redGhostController = redGhost.GetComponent<EnemyController>();
        pinkGhostController = pinkGhost.GetComponent<EnemyController>();
        blueGhostController = blueGhost.GetComponent<EnemyController>();
        orangeGhostController = orangeGhost.GetComponent<EnemyController>();


        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
    }

    void Start()
    {
        StartCoroutine(Setup());
    }

    public IEnumerator Setup()
    {
        ghostModeTimerIndex = 0;
        ghostModeTimer = 0;
        completedTimer = false;
        runningTimer = true;
        gameOverText.enabled = false;
        // If pacman clears a level, a background will appear covering the level
        if (clearedLevel)
        {   
            blackBackground.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        blackBackground.enabled = false;

        pelletsCollectedOnThisLife = 0;
        currentGhostMode = GhostMode.scatter;
        gameIsRunning = false;
        currentMunch = 0;

        float waitTimer = 1f;

        if (clearedLevel || newGame)
        {
            pelletsLeft = totalPellets;
            waitTimer = 4f;
            // Pellets will respawn when pacman clears the level or a new game
            for (int i = 0; i < nodeControllers.Count; i++)
            {
                nodeControllers[i].RespawnPellet();
            }
        }

        if (newGame)
        {
            startGameAudio.Play();
            score = 0;
            scoreText.text = "Score : " + score.ToString();
            lives = 3;
            currentLevel = 1;
        }

        pacman.GetComponent<PlayerController>().Setup();

        redGhostController.Setup();
        pinkGhostController.Setup();
        blueGhostController.Setup();
        orangeGhostController.Setup();

        newGame = false;
        clearedLevel = false;
        yield return new WaitForSeconds(waitTimer);

        StartGame();
    }

    void StartGame()
    {
        gameIsRunning = true;
        siren.Play();
    }

    void StopGame()
    {
        gameIsRunning = false;
        siren.Stop();
        powerPelletAudio.Stop();
        pacman.GetComponent<PlayerController>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameIsRunning)
        {
            return;
        }

        if (!completedTimer && runningTimer)
        {
            ghostModeTimer += Time.deltaTime;
            if (ghostModeTimer >= ghostModeTimers[ghostModeTimerIndex])
            {
                ghostModeTimer = 0;
                ghostModeTimerIndex++;
                if (currentGhostMode == GhostMode.chase)
                {
                    currentGhostMode = GhostMode.scatter;
                }
                else
                {
                    currentGhostMode = GhostMode.chase;
                }
                if (ghostModeTimerIndex == ghostModeTimers.Length)
                {
                    completedTimer = true;
                    runningTimer = false;
                    currentGhostMode = GhostMode.chase;
                }
            }
        }

        if (isPowerPelletRunning)
        {
            currentPowerPelletTime += Time.deltaTime;
            if (currentPowerPelletTime >= powerPelletTimer)
            {
                isPowerPelletRunning = false;
                currentPowerPelletTime = 0;
                powerPelletAudio.Stop();
                siren.Play();
                powerPelletMultiplier = 1;
            }
        }
    }

    public void GotPelletFromNodeController(NodeController nodeController)
    {
        nodeControllers.Add(nodeController);
        totalPellets++;
        pelletsLeft++;
    }

    public void AddtoScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }

    public IEnumerator ColledPellet(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }

        pelletsLeft--;
        pelletsCollectedOnThisLife++;

        int requiredBluePellets = 0;
        int requiredOrangePellets = 0;

        if (hadDeathOnThisLevel)
        {
            requiredBluePellets = 12;
            requiredOrangePellets = 32;
        }
        else
        {
            requiredBluePellets = 30;
            requiredOrangePellets = 60;
        }

        if (pelletsCollectedOnThisLife >= requiredBluePellets && !blueGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            blueGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }
        if (pelletsCollectedOnThisLife >= requiredOrangePellets && !orangeGhost.GetComponent<EnemyController>().leftHomeBefore)
        {
            orangeGhost.GetComponent<EnemyController>().readyToLeaveHome = true;
        }
        // Add to our score
        AddtoScore(10);

        // Check if there are any pellets left
        if (pelletsLeft == 0)
        {   
            currentLevel++;
            clearedLevel = true;
            StopGame();
            yield return new WaitForSeconds(1f);
            StartCoroutine(Setup());
        }

        // Is this a power pellet?
        if (nodeController.isPowerPellet)
        {
            siren.Stop();
            powerPelletAudio.Play();
            isPowerPelletRunning = true;
            currentPowerPelletTime = 0;

            redGhostController.SetFrightened(true);
            pinkGhostController.SetFrightened(true);
            blueGhostController.SetFrightened(true);
            orangeGhostController.SetFrightened(true);
        }
    }

    public IEnumerator PauseGame(float timeToPause)
    {
        gameIsRunning = false;
        yield return new WaitForSeconds(timeToPause);
        gameIsRunning = true;
    }

    public void GhostEaten()
    {
        StartCoroutine(PauseGame(1));
    }

    public IEnumerator PlayerEaten()
    {
        hadDeathOnThisLevel = true;
        StopGame();
        yield return new WaitForSeconds(1);

        redGhostController.SetVisible(false);
        pinkGhostController.SetVisible(false);
        blueGhostController.SetVisible(false);
        orangeGhostController.SetVisible(false);

        pacman.GetComponent<PlayerController>().Death();
        deathAudio.Play();
        yield return new WaitForSeconds(3);

        lives--;
        if (lives <= 0)
        {
            // Game over
            newGame = true;
            gameOverText.enabled = true;
            yield return new WaitForSeconds(3);
        }

        StartCoroutine(Setup());
    }
}
