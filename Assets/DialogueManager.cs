using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    private LineView line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponentInChildren<LineView>();
        PlayerStateManager.Instance.Input.actions["Interact"].started += _ => AdvanceLine();
    }

    public void AdvanceLine()
    {
        line.UserRequestedViewAdvancement();
    }

    // https://docs.yarnspinner.dev/using-yarnspinner-with-unity/components/dialogue-view/dialogue-advance-input
}
