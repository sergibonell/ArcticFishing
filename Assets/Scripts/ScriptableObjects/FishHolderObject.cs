using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishHolder", menuName = "ScriptableObjects/FishHolderScriptableObject", order = 2)]
public class FishHolderObject : ScriptableObject
{
    public FishObject[] Fishes;

    public FishObject GetRandomFish()
    {
        int i = Random.Range(0, Fishes.Length);
        return Fishes[i];
    }
}
