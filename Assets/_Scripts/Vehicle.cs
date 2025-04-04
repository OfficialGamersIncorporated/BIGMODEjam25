using UnityEngine;

public class Vehicle : MonoBehaviour {
    public float maxSteerRateOfChange = 2;
    public float maxThrottleRateOfChange = 4;
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;
    public bool canBrakeAsReverse = true;
    public bool repeatBrakeForReverse = true;

    public float SteeringInput = 0;
    public float ThrottleInput = 0;
    public float BrakeInput = 0;
    public bool UrgencyInput = false;

    [HideInInspector] public WheelControl[] wheels;
    Rigidbody rigidBody;
    Collider[] colliders;
    public Transform centerOfGravity;
    //public Transform driverExitPosition;

    public float SmoothedThrottle { get; private set; } = 0;
    public float SmoothedSteer { get; private set; } = 0;
    public float SmoothedBrake { get; private set; } = 0;
    public bool LastIsBraking { get; private set; } = false;
    bool brakeAsReverse = false;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        //rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfGravity.localPosition;

        colliders = GetComponentsInChildren<Collider>();

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();
        foreach(WheelCollider wheel in GetComponentsInChildren<WheelCollider>()) {
            foreach(Collider collider in colliders) {
                if(collider is WheelCollider) continue;
                Physics.IgnoreCollision(collider, wheel);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
        float sprintModifier = UrgencyInput ? 1.0f : 0.5f;

        SmoothedSteer = Mathf.MoveTowards(SmoothedSteer, SteeringInput, maxSteerRateOfChange * Time.deltaTime);
        SmoothedThrottle = Mathf.MoveTowards(SmoothedThrottle, ThrottleInput * sprintModifier, maxThrottleRateOfChange * sprintModifier * Time.deltaTime);
        SmoothedBrake = Mathf.MoveTowards(SmoothedBrake, BrakeInput * sprintModifier, maxThrottleRateOfChange * sprintModifier * Time.deltaTime);
        float signedAccellInput = SmoothedThrottle - SmoothedBrake;

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        //print(forwardSpeed);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // �and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        if(!repeatBrakeForReverse) {
            brakeAsReverse = canBrakeAsReverse;
        } else {
            if(canBrakeAsReverse && forwardSpeed <= 1 && !LastIsBraking && BrakeInput > 0.1f) {
                brakeAsReverse = true;
            }
        }
        if(BrakeInput < 0.1f) brakeAsReverse = false;

        bool isAccelerating;
        if (canBrakeAsReverse)
            isAccelerating = Mathf.Sign(signedAccellInput) == Mathf.Sign(forwardSpeed);
        else
            isAccelerating = SmoothedBrake == 0; //Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach(var wheel in wheels) {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if(wheel.steerable) {
                wheel.WheelCollider.steerAngle = SmoothedSteer * currentSteerRange;
            }

            if(isAccelerating) {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if(wheel.motorized) {
                    float currAccell = SmoothedThrottle;
                    if(brakeAsReverse) currAccell = signedAccellInput;
                    wheel.WheelCollider.motorTorque = currAccell * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            } else {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(signedAccellInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }

        LastIsBraking = BrakeInput > 0.1f;
    }


    public void UpgradeMotorTorque(float motorTorqueParam)
    {
        motorTorque = motorTorqueParam;
    }
    public void UpgradeMaxSpeed(float maxSpeedParam)
    {
        maxSpeed = maxSpeedParam;
    }
    public void UpgradeSteerRange(float steerRangeParam)
    {
        steeringRange = steerRangeParam;
    }
    public void UpgradeSteerRangeAtMaxSpeed(float steerRangeAtMaxSpeedParam)
    {
        steeringRangeAtMaxSpeed = steerRangeAtMaxSpeedParam;
    }
}