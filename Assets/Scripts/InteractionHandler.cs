using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    private SpringJoint joint;
    [SerializeField]
    private Rigidbody bobber;

    [SerializeField]
    private Transform reelPoint;
    private int layer;
    [SerializeField]
    private float raycastLen = 5f;
    [SerializeField]
    private float distanceShort = 3f;
    [SerializeField]
    private float distanceLong = 3f;
    [SerializeField]
    private float reelDownTime = 1f;
    [SerializeField]
    private float reelUpTime = 1f;

    private void Awake()
    {
        joint = GetComponentInChildren<SpringJoint>();
        layer = LayerMask.GetMask("Fishable");
    }

    void OnInteract()
    {
        if(reelPoint == null)
        {
            Debug.LogError("reelPoint is not assigned");
            return;
        }
        
        if(PlayerStateManager.Instance.GetCurrent() == States.Caught)
        {
            PlayerStateManager.Instance.ChangeState(States.Idle);
            return;
        }

        Debug.DrawRay(reelPoint.position, Vector3.down * 50, Color.red, 5f);
        RaycastHit hit;
        if (!PlayerStateManager.Instance.isMovementBlocked && Physics.Raycast(reelPoint.position, Vector3.down, out hit, raycastLen, layer))
        {
            PlayerStateManager.Instance.ChangeState(States.Fishing);
            return;
        }
        
    }

    void reelDown()
    {
        StartCoroutine(reelCoroutine(distanceLong, reelDownTime));
        if (bobber != null)
            bobber.AddForce(transform.TransformDirection(Vector3.forward) * 8f, ForceMode.Impulse);
        StartCoroutine(fishingWait());
    }

    IEnumerator fishingWait()
    {
        yield return new WaitForSecondsRealtime(5f);
        PlayerStateManager.Instance.ChangeState(States.Caught);
        reelUp();
    }

    void reelUp()
    {
        bobber.constraints = RigidbodyConstraints.None;
        StartCoroutine(reelCoroutine(distanceShort, reelUpTime));
    }

    IEnumerator reelCoroutine(float target, float time)
    {
        float currentTime = 0f;
        float startLen = joint.maxDistance;

        while(currentTime < time)
        {
            currentTime += Time.deltaTime;
            joint.maxDistance = Mathf.Lerp(startLen, target, currentTime);
            yield return null;
        }
    }

    void showFish()
    {

    }
}
