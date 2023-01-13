using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;
using Cinemachine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }
    public bool OnWater = false;

    [SerializeField]
    private GameObject player;
    private Animator animator;
    private CharacterController controller;
    public PlayerInput Input { get; private set; }
    public DialogueRunner DiagRunner;
    public CinemachineInputProvider camInput;
    public AmbientSoundManager sound;

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
        sound = GetComponent<AmbientSoundManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(States.Idle);
        
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
        SetCamMovement(true);
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
        SetCamMovement(false);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetCamMovement(bool x)
    {
        camInput.enabled = x;
        Cursor.visible = !x;
        Cursor.lockState = x ? CursorLockMode.Locked : CursorLockMode.Confined;
    }

    [YarnCommand("close_game")]
    public static void CloseGame()
    {
        Application.Quit();
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
