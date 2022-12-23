using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableNPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string initialConversation;
    private Canvas hintCanvas;
    [SerializeField]
    private List<MissionObject> missionsAvailable;
    private List<MissionObject> missionsCompleted;

    // Start is called before the first frame update
    void Start()
    {
        hintCanvas = GetComponentInChildren<Canvas>();
        hintCanvas.enabled = false;
    }

    public void OnEnterRange()
    {
        setHintActive(true);
    }

    public void OnExitRange()
    {
        setHintActive(false);
    }

    public void OnInteract()
    {
        PlayerStateManager.Instance.ChangeState(States.Talking);
        openConversation();
    }

    private void openConversation()
    {
        PlayerStateManager.Instance.DiagRunner.StartDialogue(initialConversation);
    }

    private void setHintActive(bool x)
    {
        hintCanvas.enabled = x;
    }

    public MissionObject GetNextMission()
    {
        return missionsAvailable[0];
    }

    public void SetCompletedMission()
    {
        missionsCompleted.Add(missionsAvailable[0]);
        missionsAvailable.RemoveAt(0);
    }
}
