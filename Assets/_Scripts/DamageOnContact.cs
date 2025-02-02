using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float hitCooldown = 2f;
    float hitTimer = 0;
    public bool canHit = true;
    public float minVelocity = 1;
    float linearVelocity;

    public float pushOtherForce;
    public float pushSelfForce;

    public float yUpIncrease = 0.5f;

    public bool stationaryDamage = false;
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
        if (linearVelocity < minVelocity && !stationaryDamage)
        {
            canHit = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canHit)
        {
            return;
        }
        else 
        {
            canHit = false;

            // Force on hit
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector3 targetDirection = collision.transform.position - transform.position;
                targetDirection.y += yUpIncrease;

                Rigidbody colRb = collision.gameObject.GetComponent<Rigidbody>();
                colRb.AddRelativeForce(targetDirection * pushOtherForce, ForceMode.Impulse);


                Vector3 directionToSelf = transform.position - collision.transform.position;
                directionToSelf.y += yUpIncrease;
                rb.AddRelativeForce(directionToSelf * pushSelfForce, ForceMode.Impulse);

            }


            // Damage
            VehicleHealth damagable = collision.gameObject.GetComponent<VehicleHealth>();
            if (!damagable) return;

            damagable.TryDamage();
            if (DestroyOnDamage) Destroy(gameObject);

        }
    }
}
