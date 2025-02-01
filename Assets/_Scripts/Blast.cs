using UnityEngine;

public class Blast : MonoBehaviour
{
    float flySpeed = 50f;
    float blastForce = 5f;
    Vector3 targetPos;

    bool hasTarget = false;

    [SerializeField] float destroyDelay = 0.5f;

    void Update()
    {
        if (hasTarget)
        {
            transform.position += flySpeed * Time.deltaTime * transform.forward;

            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                Destroy(gameObject, destroyDelay);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Rigidbody>(out Rigidbody otherRB);
        if (!other.CompareTag("Player") && otherRB != null)
        {
            otherRB.AddForceAtPosition
                (
                    (other.transform.position - transform.position) * blastForce,
                    other.transform.position,
                    ForceMode.Impulse
                );
        }
        
    }

    public void FlyInDirection(Vector3 directionParam, Vector3 posParam, float flySpeedParam, float blastForceParam)
    {
        flySpeed = flySpeedParam;
        blastForce = blastForceParam;

        targetPos = posParam;
        transform.forward = directionParam;
        hasTarget = true;
    }
}
