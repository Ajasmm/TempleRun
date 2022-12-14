using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;


    [SerializeField] float gravity = -9.8F;
    [SerializeField] float jumpHeight = 2F;

    [SerializeField] float slideCenter, colliderHeight, centerPos;

    bool isGrounded, isSliding;
    Vector3 movement, velocity;

    int jumpHash, slideHash;

    Transform myTransform;

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

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            animator.SetTrigger(slideHash);

            characterController.height = 0F;
            Vector3 colliderCenter = characterController.center;
            colliderCenter.y = slideCenter;
            characterController.center = colliderCenter;
            
            isSliding = true;
        }

        if (Input.GetKeyDown(KeyCode.Q)) RotateLeft();
        else if (Input.GetKeyDown(KeyCode.E)) RotateRight();

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < gravity * Time.deltaTime && isGrounded) velocity.y = gravity * Time.deltaTime;

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

    private void RotateLeft() => myTransform.Rotate(Vector3.up, -90F);

    public void RotateRight() => myTransform.Rotate(Vector3.up, 90F);
}
