using UnityEngine;

public class DamageOnContact : MonoBehaviour {

    public bool DestroyOnDamage = false;

    private void OnCollisionEnter(Collision collision) {
        VehicleHealth damagable = collision.gameObject.GetComponent<VehicleHealth>();
        if(!damagable) return;

        damagable.Damage();
        if (DestroyOnDamage) Destroy(gameObject);
    }

}
