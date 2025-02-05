using UnityEngine;

public class HealingPickup : MonoBehaviour {

    public bool DestroyOnHeal = true;

    private void OnCollisionEnter(Collision collision) {
        VehicleHealth healable = collision.gameObject.GetComponent<VehicleHealth>();
        if(!healable) return;
        if(!healable.TryHeal()) return;
        if (DestroyOnHeal) Destroy(gameObject);
    }
}
