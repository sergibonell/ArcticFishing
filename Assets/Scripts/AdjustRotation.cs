using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AdjustRotation : MonoBehaviour
{
    CinemachineTargetGroup group;
    Transform x1;
    Transform x2;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CinemachineTargetGroup>();
        x1 = group.m_Targets[0].target.transform;
        x2 = group.m_Targets[1].target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = x2.position - x1.position;
        dir = Quaternion.AngleAxis(-45f, Vector3.up) * dir;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
