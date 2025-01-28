using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyVehicleControl : MonoBehaviour
{
    Vehicle vehicleControl;

    Vector2 moveValue = Vector2.zero;
    bool sprintValue;

    bool hasTarget;

    PlayerFocusControl playerFocusControl;
    GameObject targetGo;
    Rigidbody targetRB;
    Vector3 predictTarget;


    [Header("Targeting")]
    [SerializeField] float distanceToTarget;
    [SerializeField] Vector3 localTargetVector;
    [SerializeField] float range = 500;
    [SerializeField] bool inRange;
    [SerializeField] Vector3 targetDirection;
    [SerializeField] float dotProduct;
    [SerializeField] bool front;
    [SerializeField] bool side;
    [SerializeField] bool back;
    [SerializeField] bool left;
    [SerializeField] bool right;

    void Start()
    {
        vehicleControl = GetComponent<Vehicle>();

        playerFocusControl = PlayerFocusControl.Instance;
        if (playerFocusControl != null)
        {
            targetGo = playerFocusControl.GetCurrentPlayer();
            targetRB = targetGo.GetComponent<Rigidbody>();
            hasTarget = true;
        }

    }

    void Update()
    {
        if (hasTarget)
        {
            predictTarget = (targetRB.linearVelocity / 2) + targetGo.transform.position;

            vehicleControl.UrgencyInput = true;

            distanceToTarget = Vector3.Distance(transform.position, predictTarget);
            localTargetVector = transform.InverseTransformPoint(predictTarget).normalized;

            inRange = (distanceToTarget < range);

            targetDirection = (predictTarget - transform.position).normalized;

            dotProduct = Vector3.Dot(transform.forward, targetDirection);
            front = dotProduct > 0.707;
            side = dotProduct < 0.707 && dotProduct > -0.707;
            back = dotProduct < -0.707;
            
            left = localTargetVector.x < 0 && side;
            right = localTargetVector.x > 0 && side;

            //

            moveValue.y = dotProduct;

            if (left)
            {
                moveValue.x = Mathf.Clamp((-1 - dotProduct), -1, 1);
            }
            if (right)
            {
                moveValue.x = Mathf.Clamp((1 - dotProduct), -1, 1);
            }
            if (front)
            {
                vehicleControl.ThrottleInput = 1;
                vehicleControl.BrakeInput = 0;
                //moveValue.y = 1;
            }
            if (back)
            {
                vehicleControl.ThrottleInput = 0;
                vehicleControl.BrakeInput = 1;
                //moveValue.y = -1;
            }


            vehicleControl.SteeringInput = moveValue.x;
            vehicleControl.UrgencyInput = sprintValue;
            vehicleControl.canBrakeAsReverse = true;
        }
        else
        {
            moveValue = Vector2.zero;

            vehicleControl.SteeringInput = 0;
            vehicleControl.ThrottleInput = 0;
            vehicleControl.BrakeInput = 1;
            vehicleControl.UrgencyInput = false;
            vehicleControl.canBrakeAsReverse = false;
        }

    }

    //public IEnumerator TurnAround()
    //{
    //    while (true)
    //    {

    //        yield return null;
    //    }
    //}



    private void OnDrawGizmos()
    {
        if (hasTarget)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, targetGo.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, predictTarget);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(predictTarget, 2);
        }
    }
}
