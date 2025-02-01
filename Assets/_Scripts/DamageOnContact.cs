using UnityEngine;

public class DamageOnContact : MonoBehaviour {

    public bool DestroyOnDamage = false;

    private void OnCollisionEnter(Collision collision) {
        VehicleHealth damagable = collision.gameObject.GetComponent<VehicleHealth>();
        if(!damagable) return;

        damagable.TryDamage();
        if (DestroyOnDamage) Destroy(gameObject);
    }

}
