using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement3D : MonoBehaviour
{
    private CharacterController controller;

    Transform cameraTransform;
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
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        PlayerStateManager.Instance.Input.actions["Move"].started += it => OnMove(it);
        PlayerStateManager.Instance.Input.actions["Move"].performed += it => OnMove(it);
        PlayerStateManager.Instance.Input.actions["Move"].canceled += it => OnMove(it);
        PlayerStateManager.Instance.Input.actions["Jump"].started += it => OnJump(it);
    }

    // Update is called once per frame
    void Update()
    {
        handleMovementXZ();
        handleMovementY();

        handleStates();

        if (!PlayerStateManager.Instance.IsMovementBlocked)
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
        if (PlayerStateManager.Instance.CompareState(States.Fishing) || PlayerStateManager.Instance.CompareState(States.Caught) || PlayerStateManager.Instance.CompareState(States.Talking))
            return;

        if (!controller.isGrounded)
        {
            PlayerStateManager.Instance.ChangeState(States.Jumping);
            return;
        }
            
        if (moveInput.magnitude > 0f)
            PlayerStateManager.Instance.ChangeState(States.Walking);
        else
            PlayerStateManager.Instance.ChangeState(States.Idle);
    }

    void rawInputToRelative()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        dir = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0) * dir;
        relativeMoveInput = new Vector2(dir.x, dir.z);
    }

    void rotateCharacter()
    {
        if(moveInput.magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
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
    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded)
            isJumping = true;
        else
            StartCoroutine(bufferJump());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Fishable"))
            PlayerStateManager.Instance.OnWater = true;
        else
            PlayerStateManager.Instance.OnWater = false;
    }
}
