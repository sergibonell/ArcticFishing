using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardObject : MonoBehaviour
{
    [SerializeField]
    private Transform pov;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.position - pov.position;
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
