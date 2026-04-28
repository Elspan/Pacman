using Unity.GraphToolkit.Editor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeState;
    public GhostNodeStatesEnum respawnState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }

    public GhostType ghostType;
    

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public MovementController movementController;
    public GameObject startingNode;
    public bool readyToLeaveHome = false;

    public GameManager gameManager;

    public bool testRespawn = false;
    public bool isFrightened = false;
    public GameObject[] scatterNodes;
    public int scatterNodeIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        scatterNodeIndex = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStatesEnum.movingInNodes;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodeStatesEnum.centerNode;
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            respawnState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            respawnState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (testRespawn)
        {
            ghostNodeState = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStatesEnum.movingInNodes)
        {
            // Scatter mode
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                
            }
            // Frightened mode
            else if (isFrightened)
            {
                
            }
            // Chase mode
            else
            {
                if (ghostType == GhostType.red)
                {
                    DeterminRedGhostDirection();
                }
            }
        }
        else if (ghostNodeState == GhostNodeStatesEnum.respawning)
        {
            string direction = "";
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                else if (respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }
            // If our respawn state is the left node and we are in the left node, change our state to center node
            else if ((transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y) || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y))
            {
                ghostNodeState = respawnState;
            }
            // We are in the gameboard
            else
            {
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }

            movementController.SetDirection(direction);
        }
        else
        {
            if (readyToLeaveHome)
            {
                if (ghostNodeState == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                else if (ghostNodeState == GhostNodeStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                else if (ghostNodeState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                else if (ghostNodeState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    void DeterminRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }

    void DeterminePinkGhostDirection()
    {
        
    }
    void DetermineBlueGhostDirection()
    {
        
    }
    void DetermineOrangeGhostDirection()
    {
        
    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestiDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {
            GameObject nodeUp = nodeController.nodeUp;
            // Get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            if (distance < shortestiDistance || shortestiDistance == 0)
            {
                shortestiDistance = distance;
                newDirection = "up";
            }
        }
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            GameObject nodeDown = nodeController.nodeDown;
            // Get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            if (distance < shortestiDistance || shortestiDistance == 0)
            {
                shortestiDistance = distance;
                newDirection = "down";
            }
        }
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {
            GameObject nodeLeft = nodeController.nodeLeft;
            // Get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            if (distance < shortestiDistance || shortestiDistance == 0)
            {
                shortestiDistance = distance;
                newDirection = "left";
            }
        }
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {
            GameObject nodeRight = nodeController.nodeRight;
            // Get the distance between our top node and pacman
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            if (distance < shortestiDistance || shortestiDistance == 0)
            {
                shortestiDistance = distance;
                newDirection = "right";
            }
        }

        return newDirection;
    }
}
