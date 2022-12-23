using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedObject;
    private IInteractable script;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStateManager.Instance.Input.actions["Interact"].started += _ => HandleButton();
        PlayerStateManager.Instance.DiagRunner.onDialogueComplete.AddListener(() => FinishDialogue());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selectedObject == null && other.TryGetComponent<IInteractable>(out script))
        {
            selectedObject = other.gameObject;
            script.OnEnterRange();
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == selectedObject)
        {
            selectedObject = null;
            script.OnExitRange();
        }   
    }

    public void FinishDialogue()
    {
        if (selectedObject != null)
            script.OnEnterRange();
    }

    void HandleButton()
    {
        if (PlayerStateManager.Instance.DiagRunner.IsDialogueRunning)
            PlayerStateManager.Instance.DiagRunner.dialogueViews[0].UserRequestedViewAdvancement();
        else if (selectedObject != null)
        {
            script.OnInteract(); 
            script.OnExitRange();
        }  
    }
}
