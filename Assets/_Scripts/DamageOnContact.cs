using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float hitCooldown = 0.25f;
    float hitTimer = 0;
    public bool canHit = true;
    public float minVelocity = 1;
    float linearVelocity;

    public bool DestroyOnDamage = false;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (!canHit)
        {
            hitTimer += Time.deltaTime;

            if (hitTimer >= hitCooldown)
            {
                canHit = true;
            }
        }
    }

    private void FixedUpdate()
    {
        linearVelocity = rb.linearVelocity.magnitude;
        if (linearVelocity < minVelocity)
        {
            canHit = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canHit)
        {
            print("ON COOLDOWN");
            return;
        }
        else 
        {
            VehicleHealth damagable = collision.gameObject.GetComponent<VehicleHealth>();
            if (!damagable) return;

            damagable.TryDamage();
            if (DestroyOnDamage) Destroy(gameObject);

            canHit = false;
            print("HIT: " + collision.gameObject.name);
        }
    }
}
