using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovementV2 : MonoBehaviour
{
    Vehicle vehicleControl;

    public Vector2 moveValue = Vector2.zero;
    bool sprintValue;

    bool hasTarget;

    PlayerFocusControl playerFocusControl;
    GameObject targetGo;
    Rigidbody targetRB;
    Vector3 predictTarget;

    [Header("Move Stats")]
    [SerializeField] float steerSpeed = 10;
    [SerializeField] float throttleSpeed = 10;


    [Header("Targeting")]
    [SerializeField] float range = 500;
    bool inRange;
    [SerializeField] float distanceToTarget;
    [SerializeField] Vector3 localTargetVector;
    [SerializeField] Vector3 targetDirection;
    [SerializeField] float dotProduct;


    [Header("Directions")]
    [SerializeField] bool front;
    [SerializeField] bool side;
    [SerializeField] bool back;
    [SerializeField] bool left;
    [SerializeField] bool right;


    float adjustedDot;
    int coin;

    float currentThrottle;
    float currentSteer;
    float currentBrake;
    bool waitImBraking;

    float steeringGoal;
    float throttleGoal;

    
    enum MovementState { None, Front, Back, Left, Right };

    MovementState movementState = MovementState.None;


    void Start()
    {
        vehicleControl = GetComponent<Vehicle>();

        GetVehicleControlStats();

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
        GetVehicleControlStats();

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



            adjustedDot = dotProduct - 1;

            if (front)
            {
                if (movementState != MovementState.Front)
                {
                    EnterFront();
                }
                
            }
            else if (back)
            {
                if (movementState != MovementState.Back)
                {
                    EnterBack();
                }
            }

            if (left)
            {
                if (movementState != MovementState.Left)
                {
                    EnterLeft();
                }
            }
            else if (right)
            {
                if (movementState != MovementState.Right)
                {
                    EnterRight();
                }
            }


            //

            if (currentThrottle < throttleGoal)
            {
                moveValue.y += throttleSpeed * Time.deltaTime;
                moveValue.x = Mathf.Clamp(moveValue.x, -1, 1);
            }
            else if (currentThrottle > throttleGoal)
            {
                moveValue.y -= throttleSpeed * Time.deltaTime;
            }

            if (currentSteer > steeringGoal)
            {
                moveValue.x -= steerSpeed * Time.deltaTime;
            }
            else if (currentSteer < steeringGoal)
            {
                moveValue.x += steerSpeed * Time.deltaTime;
            }

            moveValue.x = Mathf.Clamp(moveValue.x, -1, 1);
            moveValue.y = Mathf.Clamp(moveValue.y, -1, 1);


            /*
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
                vehicleControl.ThrottleInput = 1;
                vehicleControl.BrakeInput = 1;
                //moveValue.y = -1;
            }
            */

            vehicleControl.ThrottleInput = moveValue.y;
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

    void GetVehicleControlStats()
    {
        currentThrottle = vehicleControl.SmoothedThrottle;
        currentSteer = vehicleControl.SmoothedSteer;
        currentBrake = vehicleControl.SmoothedBrake;
        waitImBraking = vehicleControl.LastIsBraking;
    }


    void EnterFront()
    {
        movementState = MovementState.Front;
        steeringGoal = adjustedDot;
        throttleGoal = 1;
    }
    void EnterBack()
    {
        movementState = MovementState.Back;
        throttleGoal = -1;
        coin = Random.Range(0, 2);
        if (coin == 0)
        {
            steeringGoal = 1;
        }
        else if (coin == 1)
        {
            steeringGoal = -1;
        }
    }
    void EnterLeft()
    {
        movementState = MovementState.Left;
        steeringGoal = -1;
        throttleGoal = 1;
        adjustedDot = -1 * adjustedDot;
    }
    void EnterRight()
    {
        movementState = MovementState.Right;
        steeringGoal = 1;
        throttleGoal = 1;
    }

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
