using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] float explosionForce;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Vector3 targetDirection = collision.transform.position - transform.position;
            targetDirection.y += 0.5f;

            Rigidbody colRb = collision.gameObject.GetComponent<Rigidbody>();
            colRb.AddRelativeForce(targetDirection * explosionForce, ForceMode.Impulse);

            //Explosion debris?
        }
    }
}
