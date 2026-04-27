using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sprite;
    MovementController movementController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("moving", true);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementController.SetDirection("left");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementController.SetDirection("right");
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            movementController.SetDirection("up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movementController.SetDirection("down");
        }

        bool flipX = false;
        bool flipY = false;
        if (movementController.lastMovingDirection == "left")
        {
            animator.SetInteger("direction", 0);
        }
        else if (movementController.lastMovingDirection == "right")
        {
            animator.SetInteger("direction", 0);
            flipX = true;
        }
        else if (movementController.lastMovingDirection == "up")
        {
            animator.SetInteger("direction", 1);
        }
        else if (movementController.lastMovingDirection == "down")
        {
            animator.SetInteger("direction", 1);
            flipY = true;
        }

        sprite.flipX = flipX;
        sprite.flipY = flipY;
    }
}
