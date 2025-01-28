using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyVehicleControl : MonoBehaviour
{
    Vehicle vehicleControl;

    Vector2 moveValue = Vector2.zero;
    bool sprintValue = false;

    bool hasTarget;

    PlayerFocusControl playerFocusControl;
    GameObject targetGo;


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
            hasTarget = true;
        }

    }

    void Update()
    {
        if (hasTarget)
        {
            distanceToTarget = Vector3.Distance(transform.position, targetGo.transform.position);
            localTargetVector = transform.InverseTransformPoint(targetGo.transform.position).normalized;

            inRange = (distanceToTarget < range);

            targetDirection = (targetGo.transform.position - transform.position).normalized;

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
                moveValue.x = dotProduct;
            }
            if (right)
            {
                moveValue.x = -1 * dotProduct;
            }
            if (front)
            {
                sprintValue = true;
            }
            if (back)
            {
                sprintValue = true;
            }


            vehicleControl.SteeringInput = -1 * moveValue.x;
            vehicleControl.ThrottleInput = Mathf.Max(0, moveValue.y);
            vehicleControl.BrakeInput = Mathf.Max(0, -moveValue.y);
            vehicleControl.UrgencyInput = sprintValue;
            vehicleControl.canBrakeAsReverse = true;
        }
        else
        {
            moveValue = Vector2.zero;
            sprintValue = false;

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
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetGo.transform.position);
        }
    }
}
