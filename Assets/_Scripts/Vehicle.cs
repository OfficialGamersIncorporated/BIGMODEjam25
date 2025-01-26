using UnityEngine;
using UnityEngine.InputSystem;

public class Vehicle : MonoBehaviour {
    public float maxSteerRateOfChange = 2;
    public float maxThrottleRateOfChange = 4;
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    WheelControl[] wheels;
    Rigidbody rigidBody;

    InputAction moveAction;
    InputAction sprintAction;

    float lastSteer = 0;
    float lastAccell = 0;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelControl>();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    // Update is called once per frame
    void Update() {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool sprintValue = sprintAction.IsPressed();
        float sprintModifier = sprintValue ? 1.0f : 0.5f;

        lastSteer = Mathf.MoveTowards(lastSteer, moveValue.x, maxSteerRateOfChange * Time.deltaTime);
        lastAccell = Mathf.MoveTowards(lastAccell, moveValue.y * sprintModifier, maxThrottleRateOfChange * sprintModifier * Time.deltaTime);

        float vInput = lastAccell;
        float hInput = lastSteer;

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        print(forwardSpeed);


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
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach(var wheel in wheels) {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if(wheel.steerable) {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if(isAccelerating) {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if(wheel.motorized) {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            } else {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}