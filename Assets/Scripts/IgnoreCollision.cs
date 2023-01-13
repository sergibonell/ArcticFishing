using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    string[] layersToIgnore;
    [SerializeField]
    AudioClip splash;

    // Start is called before the first frame update
    void Start()
    {
        foreach(string x in layersToIgnore)
        {
            Debug.Log(LayerMask.GetMask(x));
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(x));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Stuck") && PlayerStateManager.Instance.GetCurrent() == States.Fishing)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            PlayerStateManager.Instance.sound.playSound(splash);
        }
            
    }
}
