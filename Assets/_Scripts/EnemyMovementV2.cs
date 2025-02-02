using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyMovementV2 : MonoBehaviour
{
    Vehicle vehicleControl;

    public Vector2 moveValue = Vector2.zero;
    bool sprintValue;

    PlayerFocusControl playerFocusControl;
    GameObject targetGo;
    Rigidbody targetRB;
    Vector3 predictTarget;

    [Header("Move Stats")]
    [SerializeField] float steerSpeed = 10;
    [SerializeField] float throttleSpeed = 10;


    [Header("Targeting")]
    [SerializeField] bool alwaysHasTarget = false;
    [SerializeField] float targetRange = 100;
    [SerializeField] float closeRange = 5;

    bool predict = true;
    float distanceToTarget;
    bool inRange = false;
    float distanceToPredictTarget;
    Vector3 localTargetVector;
    Vector3 targetDirection;
    float dotProduct;


    //Direction
    bool front;
    bool side;
    bool back;
    bool left;
    bool right;


    float adjustedDot;
    int coin;

    //float currentThrottle;
    //float currentSteer;
    //float currentBrake;
    //bool waitImBraking;

    float steeringGoal;
    float throttleGoal;

    
    enum MovementState { None, Front, Back, Left, Right };

    MovementState movementState = MovementState.None;


    void Start()
    {
        vehicleControl = GetComponent<Vehicle>();

        //GetVehicleControlStats();

        playerFocusControl = PlayerFocusControl.Instance;
        if (playerFocusControl != null)
        {
            targetGo = playerFocusControl.GetCurrentPlayer();
            targetRB = targetGo.GetComponent<Rigidbody>();
        }

    }

    void Update()
    {
        //GetVehicleControlStats();

        if (targetGo)
        {
            predictTarget = (targetRB.linearVelocity / 2) + targetGo.transform.position;

            vehicleControl.UrgencyInput = true;

            distanceToTarget = Vector3.Distance(transform.position, targetGo.transform.position);
            distanceToPredictTarget = Vector3.Distance(transform.position, predictTarget);
            localTargetVector = transform.InverseTransformPoint(predictTarget).normalized;

            inRange = (distanceToTarget < targetRange);

            predict = (distanceToTarget > closeRange);

            if (predict)
            {
                targetDirection = (predictTarget - transform.position).normalized;
            }
            else
            {
                targetDirection = (targetGo.transform.position - transform.position).normalized;
            }
            

            dotProduct = Vector3.Dot(transform.forward, targetDirection);
            front = dotProduct > 0.707;
            side = dotProduct < 0.707 && dotProduct > -0.707;
            back = dotProduct < -0.707;

            left = localTargetVector.x < 0 && side;
            right = localTargetVector.x > 0 && side;

            //

            adjustedDot = dotProduct - 1;

            if (inRange || alwaysHasTarget)
            {
                if (front)
                {
                    if (movementState != MovementState.Front)
                    {
                        EnterFront();
                    }

                    if (localTargetVector.x < 0)
                    {
                        adjustedDot = Mathf.Abs(adjustedDot) * -1;
                    }

                    steeringGoal = adjustedDot * 2;
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

                moveValue.x = Mathf.MoveTowards(moveValue.x, steeringGoal, steerSpeed * Time.deltaTime);
                moveValue.y = Mathf.MoveTowards(moveValue.y, throttleGoal, throttleSpeed * Time.deltaTime);

                moveValue.x = Mathf.Clamp(moveValue.x, -1, 1);
                moveValue.y = Mathf.Clamp(moveValue.y, -1, 1);

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

    //void GetVehicleControlStats()
    //{
    //    currentThrottle = vehicleControl.SmoothedThrottle;
    //    currentSteer = vehicleControl.SmoothedSteer;
    //    currentBrake = vehicleControl.SmoothedBrake;
    //    waitImBraking = vehicleControl.LastIsBraking;
    //}


    void EnterFront()
    {
        movementState = MovementState.Front;
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
        throttleGoal = 0.50f;
    }
    void EnterRight()
    {
        movementState = MovementState.Right;
        steeringGoal = 1;
        throttleGoal = 0.50f;
    }

    private void OnDrawGizmos()
    {
        if (targetGo)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, targetGo.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, predictTarget);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(predictTarget, 2);


            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, targetRange / 2);
        }
    }
}
