using UnityEngine;

public class Wall : MonoBehaviour
{
    float newYPos;
    float startingHeight;
    float raiseSpeed;
    float wallForce;

    float wallHeight;
    float wallWidth;

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 targetDirection = collision.transform.position - transform.position;

        print("COLLISION WITH: " + collision.gameObject.name);
        Rigidbody colRb = collision.gameObject.GetComponent<Rigidbody>();
        colRb.AddRelativeForce(targetDirection * wallForce, ForceMode.Impulse);
    }

    private void Update()
    {

        if (transform.position.y < (wallHeight / 2) + startingHeight)
        {
            newYPos += Time.deltaTime * raiseSpeed;
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);

            if (transform.position.y > (wallHeight / 2) + startingHeight)
            {
                transform.position = new Vector3(transform.position.x, (wallHeight / 2) + startingHeight, transform.position.z);
            }
        }
    }


    public void RaiseWall(float startingHeightParam, float spawnHeightParam, float raiseSpeedParam, float wallForceParam, float wallHeightParam, float lifeSpanParam)
    {
        startingHeight = startingHeightParam;
        newYPos = spawnHeightParam;
        raiseSpeed = raiseSpeedParam;
        wallForce = wallForceParam;
        wallHeight = wallHeightParam;

        Destroy(gameObject, lifeSpanParam);
    }
}
