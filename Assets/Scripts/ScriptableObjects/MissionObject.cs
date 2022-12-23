using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/MissionScriptableObject", order = 1)]
public class MissionObject : ScriptableObject
{
    [SerializeField]
    private MissionTypes type;
    [SerializeField]
    private List<FishObject> requiredFishes;
    [SerializeField]
    private string missionText;

    public bool CheckMissionComplete(List<FishObject> fishes)
    {
        return requiredFishes.OrderBy(x => x.Name).SequenceEqual(fishes.OrderBy(x => x.Name));
    }

    public string GetText()
    {
        return missionText;
    }
}

public enum MissionTypes
{
    Retrieve
}
