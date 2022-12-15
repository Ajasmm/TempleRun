using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;


    [SerializeField] float gravity = -9.8F;
    [SerializeField] float jumpHeight = 2F;
    [SerializeField] float turningTime = 0.2F;

    [SerializeField] float slideCenter, colliderHeight, centerPos;

    bool isGrounded, isSliding, insideTurn = false;
    Vector3 pos, velocity;
    float xAlign = 0, xOffset = 0;

    int jumpHash, slideHash, turnHash, deadHash;

    Transform myTransform;
    Turn currentTurn;
    PlayerDirection playerDirection = PlayerDirection.Forward;
    Direction turnDirection;

    private void Awake()
    {
        if(characterController == null) characterController = gameObject.GetComponent<CharacterController>();
        if (animator == null) animator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (characterController == null) this.enabled = false;
        if (animator == null) this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        jumpHash = Animator.StringToHash("Jump");
        slideHash = Animator.StringToHash("Slide");
        turnHash = Animator.StringToHash("Turn");
        deadHash = Animator.StringToHash("Dead");

        colliderHeight = characterController.height;
        centerPos = characterController.center.y;

        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = characterController.isGrounded;

        // Jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetTrigger(jumpHash);
        }

        // Slide
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            animator.SetTrigger(slideHash);

            characterController.height = 0F;
            Vector3 colliderCenter = characterController.center;
            colliderCenter.y = slideCenter;
            characterController.center = colliderCenter;
            
            isSliding = true;
        }

        // Turning
        if (insideTurn)
        {
            if (Input.GetKeyDown(KeyCode.Q) && isGrounded)
                turnDirection = Direction.Left;
            else if (Input.GetKeyDown(KeyCode.E) && isGrounded)
                turnDirection = Direction.Right;
        }


        // XOffset
        xOffset = Input.GetAxis("Horizontal");

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < 0 && isGrounded) velocity.y = gravity;

        // Apply velocity
        characterController.Move(velocity * Time.deltaTime);

    }

    public void ResetSliding()
    {
        characterController.height = colliderHeight;
        Vector3 colliderCenter = Vector3.zero;
        colliderCenter.y = centerPos;
        characterController.center = colliderCenter;

        isSliding = false;
    }

    private void OnAnimatorMove()
    {
        animator.ApplyBuiltinRootMotion();
        Vector3 pos = myTransform.position;

        switch (playerDirection)
        {
            case PlayerDirection.Forward:
                pos.x = xAlign + xOffset;
                break;
            case PlayerDirection.Right:
                pos.z = -(xAlign + xOffset);
                break;
            case PlayerDirection.Back:
                pos.x = -(xAlign + xOffset);
                break;
            case PlayerDirection.Left:
                pos.z = (xAlign + xOffset);
                break;
        }

        myTransform.position = pos;

    }

    IEnumerator RotateLeft()
    {
        float time = 0F, deltaTime = 0;

        while(time <= turningTime)
        { 
            deltaTime = Time.deltaTime;
            time += deltaTime;

            if(time > turningTime)
            {
                deltaTime = time - turningTime;
            }

            myTransform.Rotate(Vector3.up, -90 * (1 / turningTime) * deltaTime);
            yield return null;

        }

        // calculate Direction
        int direction = (int)playerDirection;
        direction--;
        if (direction < 0)
            direction = (int) PlayerDirection.Left;

        xAlign = currentTurn.GetCenterPos();
        currentTurn = null;

        playerDirection = (PlayerDirection) direction;
    }

    IEnumerator RotateRight()
    {

        float time = 0F, deltaTime = 0;

        while (time <= turningTime)
        {
            deltaTime = Time.deltaTime;
            time += deltaTime;

            if (time > turningTime)
            {
                deltaTime = time - turningTime;
            }

            myTransform.Rotate(Vector3.up, 90 * (1 / turningTime) * deltaTime);
            yield return null;

        }

        // calculate Direction
        int direction = (int)playerDirection;
        direction++;
        if (direction > (int) PlayerDirection.Left)
            direction = 0;


        xAlign = currentTurn.GetCenterPos();
        currentTurn = null;

        playerDirection = (PlayerDirection)direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InsideTurn"))
        {
            insideTurn = true;

        }else if (other.CompareTag("Turn"))
        {
            insideTurn = false;
            currentTurn = other.gameObject.GetComponent<Turn>();
            if(currentTurn == null || !currentTurn.GetDirections().Contains(turnDirection))
            {
                turnDirection = Direction.None;
                return;
            }
            animator.SetTrigger(turnHash);
        }
        else if (other.CompareTag("Dead"))
        {

        }
    }

    public void Turn()
    {
        switch (turnDirection)
        {
            case Direction.Left:
                StartCoroutine(RotateLeft());
                break;
            case Direction.Right:
                StartCoroutine(RotateRight());
                break;
        }

        turnDirection = Direction.None;
    }
}
