using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerStateManager : MonoBehaviour
{
    private States currentState;
    private Animator animator;
    private CharacterController controller;

    [SerializeField]
    private CinemachineInputProvider camInput;
    public static PlayerStateManager Instance { get; private set; }
    public bool isMovementBlocked { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(States.Idle);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void ChangeState(States state)
    {
        switch (state)
        {
            case States.Idle:
                idleState();
                break;
            case States.Walking:
                walkingState();
                break;
            case States.Jumping:
                jumpingState();
                break;
            case States.Fishing:
                fishingState();
                break;
            case States.Caught:
                caughState();
                break;
        }

        currentState = state;
    }

    public States GetCurrent()
    {
        return currentState;
    }

    public bool CompareState(States state)
    {
        return currentState == state;
    }

    void idleState()
    {
        animator.SetBool("Walking", false);
        
        if(currentState == States.Fishing || currentState == States.Caught) 
            animator.SetTrigger("ReturnIdle");
        isMovementBlocked = false;
    }

    void walkingState()
    {
        animator.SetBool("Walking", true);
        isMovementBlocked = false;
    }

    void jumpingState()
    {
        animator.SetBool("Walking", false);
        isMovementBlocked = false;
    }

    void fishingState()
    {
        animator.SetBool("Walking", false);
        animator.SetTrigger("ThrowLine");
        isMovementBlocked = true;
    }

    void caughState()
    {
        animator.SetTrigger("FishCaught");
        isMovementBlocked = true;
    }
}

public enum States
{
    Idle,
    Walking,
    Jumping,
    Fishing,
    Caught
}
