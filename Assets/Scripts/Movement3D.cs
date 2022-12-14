using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement3D : MonoBehaviour
{
    private CharacterController controller;
    private PlayerStateManager manager;

    Transform camera;
    private Vector3 velocity;
    private Vector2 moveInput;
    private Vector2 relativeMoveInput;
    bool isJumping = false;
    bool bufferedJump = false;

    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed = 5f;
    [Header("JUMPING")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float bufferThreshhold = 0.5f;

    private void Awake()
    {
        manager = GetComponent<PlayerStateManager>();
        controller = GetComponent<CharacterController>();
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovementXZ();
        handleMovementY();

        handleStates();

        if (!manager.isMovementBlocked)
            commitMove();
            
    }

    void handleMovementXZ()
    {
        rawInputToRelative();

        velocity.x = relativeMoveInput.x * moveSpeed;
        velocity.z = relativeMoveInput.y * moveSpeed;
    }

    void handleMovementY()
    {
        if ((isJumping || bufferedJump) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            isJumping = false;
        }
        else
        {
            velocity.y = controller.isGrounded ? -2f : velocity.y + gravity * Time.deltaTime;
        }
    }

    void commitMove()
    {
        rotateCharacter();
        controller.Move(velocity * Time.deltaTime);
    }

    void handleStates()
    {
        if (manager.GetCurrent() == States.Fishing || manager.GetCurrent() == States.Caught)
            return;

        if (!controller.isGrounded)
        {
            manager.ChangeState(States.Jumping);
            return;
        }
            
        if (moveInput.magnitude > 0f)
            manager.ChangeState(States.Walking);
        else
            manager.ChangeState(States.Idle);
    }

    void rawInputToRelative()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        dir = Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0) * dir;
        relativeMoveInput = new Vector2(dir.x, dir.z);
    }

    void rotateCharacter()
    {
        if(moveInput.magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, camera.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator bufferJump()
    {
        StopCoroutine(bufferJump());

        bufferedJump = true;
        yield return new WaitForSeconds(bufferThreshhold);
        bufferedJump = false;
    }

    // INPUT EVENT HANDLING
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (controller.isGrounded)
            isJumping = true;
        else
            StartCoroutine(bufferJump());
    }
}
