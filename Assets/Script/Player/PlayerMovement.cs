using JetBrains.Annotations;
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

    bool isGrounded, isSliding, insideTurn = false, isDead = false;
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

        if (!isDead)
        {

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
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
            switch (playerDirection)
            {
                case PlayerDirection.Forward:
                case PlayerDirection.Left:
                    xOffset = Input.GetAxis("Horizontal");
                    break;
                case PlayerDirection.Back:
                case PlayerDirection.Right:
                    xOffset = -Input.GetAxis("Horizontal");
                    break;
            }
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < 0 && isGrounded) velocity.y = gravity;

        // Apply velocity
        characterController.Move(velocity * Time.deltaTime);

    }

    public void ResetSliding()
    {
        Vector3 colliderCenter = Vector3.zero;
        colliderCenter.y = centerPos;
        // characterController.Move(Vector3.up * 0.1F);
        characterController.center = colliderCenter;
        characterController.height = colliderHeight;

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
                pos.z = xAlign + xOffset;
                break;
            case PlayerDirection.Back:
                pos.x = xAlign + xOffset;
                break;
            case PlayerDirection.Left:
                pos.z = xAlign + xOffset;
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
                deltaTime -= (time - turningTime);
            }

            myTransform.Rotate(Vector3.up, -90 * (1 / turningTime) * deltaTime);
            yield return null;

        }

        // calculate Direction
        int direction = (int)playerDirection;
        direction--;
        if (direction < 0)
            direction = (int) PlayerDirection.Left;

        playerDirection = (PlayerDirection)direction;

        switch (playerDirection)
        {
            case PlayerDirection.Forward:
            case PlayerDirection.Back:
                xAlign = currentTurn.GetCenterPos().x;
                break;
            case PlayerDirection.Right:
            case PlayerDirection.Left:
                xAlign = currentTurn.GetCenterPos().z;
                break;
        }

        currentTurn = null;

        Vector3 euler = Vector3.zero;
        switch (playerDirection)
        {
            case PlayerDirection.Forward:
                euler = myTransform.localEulerAngles;
                euler.y = 0f;
                break;
            case PlayerDirection.Right:
                euler = myTransform.localEulerAngles;
                euler.y = 90f;
                break;
            case PlayerDirection.Back:
                euler = myTransform.localEulerAngles;
                euler.y = 180f;
                break;
            case PlayerDirection.Left:
                euler = myTransform.localEulerAngles;
                euler.y = 270f;
                break;
        }
        myTransform.localEulerAngles = euler;

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
                deltaTime -= (time - turningTime);
            }

            myTransform.Rotate(Vector3.up, 90 * (1 / turningTime) * deltaTime);
            yield return null;

        }

        // calculate Direction
        int direction = (int)playerDirection;
        direction++;
        if (direction > (int) PlayerDirection.Left)
            direction = 0;

        playerDirection = (PlayerDirection)direction;


        switch (playerDirection)
        {
            case PlayerDirection.Forward:
            case PlayerDirection.Back:
                xAlign = currentTurn.GetCenterPos().x;
                break;
            case PlayerDirection.Right:
            case PlayerDirection.Left:
                xAlign = currentTurn.GetCenterPos().z;
                break;
        }

        currentTurn = null;

        Vector3 euler = Vector3.zero;
        switch (playerDirection)
        {
            case PlayerDirection.Forward:
                euler = myTransform.localEulerAngles;
                euler.y = 0f;
                break;
            case PlayerDirection.Right:
                euler = myTransform.localEulerAngles;
                euler.y = 90f;
                break;
            case PlayerDirection.Back:
                euler = myTransform.localEulerAngles;
                euler.y = 180f;
                break;
            case PlayerDirection.Left:
                euler = myTransform.localEulerAngles;
                euler.y = 270f;
                break;
        }
        myTransform.localEulerAngles = euler;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            coin.Destroy();

        }else if (other.CompareTag("InsideTurn"))
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
           isDead = true;
            GamePlayManager.instance.OnGameOver();
           animator.SetTrigger(deadHash);
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
