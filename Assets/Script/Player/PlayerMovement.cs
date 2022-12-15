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

    [SerializeField] float slideCenter, colliderHeight, centerPos;

    bool isGrounded, isSliding, insideTurn = false;
    Vector3 pos, velocity;
    float xAlign = 0, xOffset = 0;

    int jumpHash, slideHash;

    Transform myTransform;
    PlayerDirection playerDirection = PlayerDirection.Forward;

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
        if (Input.GetKeyDown(KeyCode.Q)) RotateLeft();
        else if (Input.GetKeyDown(KeyCode.E)) RotateRight();

        // XOffset
        xOffset = Input.GetAxis("Horizontal");

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < 0 && isGrounded) velocity.y = gravity * Time.deltaTime;

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

    private void RotateLeft()
    {
        if (!insideTurn) return;

        myTransform.Rotate(Vector3.up, -90F);

        // calculate Direction
        int direction = (int)playerDirection;
        direction--;
        if (direction < 0)
            direction = (int) PlayerDirection.Left;

        playerDirection = (PlayerDirection) direction;
    }

    public void RotateRight()
    {
        if(!insideTurn) return;

        myTransform.Rotate(Vector3.up, 90F);

        // calculate Direction
        int direction = (int)playerDirection;
        direction++;
        if (direction > (int) PlayerDirection.Left)
            direction = 0;

        playerDirection = (PlayerDirection)direction;
    }
}
