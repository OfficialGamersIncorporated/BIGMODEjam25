using UnityEngine;

public class Blast : MonoBehaviour
{
    [SerializeField] float flySpeed = 50f;
    [SerializeField] float blastForce = 50f;
    Vector3 targetPos;

    bool hasTarget = false;

    [SerializeField] float destroyDelay = 0.5f;

    Vector3 debugPos;

    void Start()
    {
        
    }

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
        other.GetComponent<Rigidbody>().AddRelativeForce((other.transform.position - transform.position) * blastForce, ForceMode.Impulse);
        debugPos = other.transform.position;
    }

    public void FlyInDirection(Vector3 directionParam, Vector3 posParam)
    {
        targetPos = posParam;
        transform.forward = directionParam;
        hasTarget = true;
    }

    private void OnDrawGizmos()
    {
        if (debugPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(debugPos, debugPos - transform.position);
        }
    }
}
