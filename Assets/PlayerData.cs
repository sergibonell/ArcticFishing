using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    List<FishObject> inventory;
    [SerializeField]
    static MissionObject mission;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerStateManager.Instance.Input.actions["Interact"].started += _ => Debug.Log(mission.CheckMissionComplete(inventory));
    }

    private void Update()
    {
        Debug.Log(GetMissionAssigned());
    }

    [YarnFunction("hasMission")]
    public static bool GetMissionAssigned()
    {
        return mission != null;
    }

    public void AssignMission(MissionObject newMission)
    {
        mission = newMission;
    }
}
