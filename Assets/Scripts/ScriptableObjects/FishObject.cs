using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/FishScriptableObject", order = 1)]
public class FishObject : ScriptableObject
{
    public string Name;
    public GameObject Model;
}
