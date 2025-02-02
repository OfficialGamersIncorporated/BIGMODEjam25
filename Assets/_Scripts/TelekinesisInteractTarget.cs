using UnityEngine;
using UnityEngine.Events;

public class TelekinesisInteractTarget : MonoBehaviour {

    public Rigidbody PrefabSpawnOnGrab;
    public UnityEvent GrabbedEvent;
    public bool DestroyOnGrab;

    public Rigidbody Grab() {
        GrabbedEvent.Invoke();
        if(DestroyOnGrab) Destroy(gameObject);
        if(PrefabSpawnOnGrab) {
            Rigidbody instance = Instantiate<Rigidbody>(PrefabSpawnOnGrab, transform.position, transform.rotation);
            return instance;
        }
        return null;
    }
}
