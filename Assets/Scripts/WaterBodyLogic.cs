using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBodyLogic : MonoBehaviour
{
    [SerializeField]
    private FishHolderObject availableFishes;
    public FishHolderObject AvailableFishes { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        AvailableFishes = availableFishes;
    }
}
