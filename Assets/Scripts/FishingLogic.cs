using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingLogic : MonoBehaviour
{
    private SpringJoint joint;
    private WaterBodyLogic water;

    [SerializeField]
    private AudioClip reel;
    [SerializeField]
    private AudioClip store;

    [SerializeField]
    private Rigidbody hookRb;
    [SerializeField]
    private GameObject bobber;
    [SerializeField]
    private MeshFilter fishMesh;

    [SerializeField]
    private Transform reelPoint;
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

    [SerializeField]
    private string fishableLayer;
    private int layerIndex;

    private void Awake()
    {
        joint = GetComponentInChildren<SpringJoint>();
        layerIndex = LayerMask.GetMask(fishableLayer);
    }

    private void Start()
    {
        PlayerStateManager.Instance.Input.actions["Fire"].started += it => OnFire(it);
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase.ToString());
        if(reelPoint == null)
        {
            Debug.LogError("reelPoint is not assigned");
            return;
        }
        
        if(PlayerStateManager.Instance.GetCurrent() == States.Caught)
        {
            bobber.SetActive(true);
            fishMesh.mesh = null;
            PlayerStateManager.Instance.ChangeState(States.Idle);
            PlayerStateManager.Instance.sound.playSound(store);
            return;
        }

        Debug.DrawRay(reelPoint.position, Vector3.down * 50, Color.red, 5f);
        RaycastHit hit;
        if (!PlayerStateManager.Instance.IsMovementBlocked && !PlayerStateManager.Instance.CompareState(States.Jumping) && Physics.Raycast(reelPoint.position, Vector3.down, out hit, raycastLen, layerIndex))
        {
            water = hit.collider.gameObject.GetComponent<WaterBodyLogic>();
            PlayerStateManager.Instance.ChangeState(States.Fishing);
            return;
        }
        
    }

    void reelDown()
    {
        StartCoroutine(reelCoroutine(distanceLong, reelDownTime));
        if (hookRb != null)
            hookRb.AddForce(transform.TransformDirection(Vector3.forward) * 8f, ForceMode.Impulse);
        StartCoroutine(fishingWait());
    }

    IEnumerator fishingWait()
    {
        float time = Random.Range(2f, 5f);
        yield return new WaitForSecondsRealtime(time);
        PlayerStateManager.Instance.ChangeState(States.Caught);
        bobber.SetActive(false);
        fishMesh.mesh = water.AvailableFishes.GetRandomFish().Model;
        reelUp();
    }

    void reelUp()
    {
        hookRb.constraints = RigidbodyConstraints.None;
        StartCoroutine(reelCoroutine(distanceShort, reelUpTime));
        PlayerStateManager.Instance.sound.playSound(reel);
    }

    IEnumerator reelCoroutine(float target, float time)
    {
        float currentTime = 0f;
        float startLen = joint.maxDistance;

        while(currentTime < time)
        {
            currentTime += Time.deltaTime;
            joint.maxDistance = Mathf.Lerp(startLen, target, currentTime/time);
            yield return null;
        }
    }
}
