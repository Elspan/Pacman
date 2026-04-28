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

    public enum GhostMode
    {
        chase,
        scatter,
    }

    public GhostMode currentGhostMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {   
        currentGhostMode = GhostMode.chase;
        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;
        score = 0;
        currentMunch = 0;
        siren.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddtoScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score.ToString();
    }

    public void ColledPellet(NodeController nodeController)
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

        // Add to our score
        AddtoScore(10);

        // Check if there are any pellets left

        // Check how many pellet were eaten

        // Is this a power pellet?
    }
}
