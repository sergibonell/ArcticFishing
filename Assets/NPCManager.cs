using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCManager : MonoBehaviour
{
    [SerializeField]
    List<TalkableNPC> npcs;
    
    private TalkableNPC GetNPC(int i)
    {
        if (i < npcs.Count)
            return npcs[i];

        Debug.LogError("Invalid NPC index");
        return null;
    }

    [YarnFunction("getMission")]
    public string GetNewMission(int npc)
    {
        PlayerStateManager.Instance.GetPlayer().GetComponent<PlayerData>().AssignMission(GetNPC(npc).GetNextMission());
        return GetNPC(npc).GetNextMission().GetText();
    }
}
