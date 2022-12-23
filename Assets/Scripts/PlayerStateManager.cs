using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }

    [SerializeField]
    private GameObject player;
    private Animator animator;
    private CharacterController controller;
    public PlayerInput Input { get; private set; }
    public DialogueRunner DiagRunner;

    private States currentState;
    public bool IsMovementBlocked { get; private set; }
    static private bool hasMissionAssigned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        animator = player.GetComponentInChildren<Animator>();
        controller = player.GetComponent<CharacterController>();
        Input = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(States.Idle);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        hasMissionAssigned = false;

        DiagRunner.onDialogueComplete.AddListener(() => ChangeState(States.Idle));
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
            case States.Talking:
                talkState();
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
        IsMovementBlocked = false;
    }

    void walkingState()
    {
        animator.SetBool("Walking", true);
        IsMovementBlocked = false;
    }

    void jumpingState()
    {
        animator.SetBool("Walking", false);
        IsMovementBlocked = false;
    }

    void fishingState()
    {
        animator.SetBool("Walking", false);
        animator.SetTrigger("ThrowLine");
        IsMovementBlocked = true;
    }

    void caughState()
    {
        animator.SetTrigger("FishCaught");
        IsMovementBlocked = true;
    }

    private void talkState()
    {
        IsMovementBlocked = true;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}

public enum States
{
    Idle,
    Walking,
    Jumping,
    Fishing,
    Caught,
    Talking
}
