using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    // REFERENCES
    Camera cam;
    Rigidbody rb;
    [SerializeField] GameObject mesh;

    [SerializeField] float lookRotationSpeed;

    // MOVEMENT
    [SerializeField] float playerHeight = 2;

    public bool isDashPressed = false;
    public bool isJumpPressed = false;
    public Vector2 movementVector;
    Vector3 relativeMovementVector;

    bool lastDashPressed = false;
    bool lastJumpPressed = false;
    Vector3 gravity;

    [Header("Movement Stats")]
    [SerializeField] float acceleration;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashForce;
    [SerializeField] float airStrafe;

    bool canDash;
    float dashTimer;
    [SerializeField] float dashCooldown;

    [Tooltip("Slerp rotate the player's velocity towards their desired movement direction to turn faster than acceleration allows.")]
    [SerializeField] float velocitySlerp = 5;
    [Tooltip("Gravity will be multiplied by this value for this character after reaching the apex of their jump or releasing the jump key.")]
    [SerializeField] float fallingGravityModifier = 2;

    [Header("Ground Detection")]
    bool grounded;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckCooldown = 0.1f;
    float groundCheckTimer;
    bool canGroundCheck = true;




    Vector3 camForward;
    Vector3 camRight;

    void Start() {
        gravity = Physics.gravity;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity != Vector3.zero)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(rb.linearVelocity, transform.up);
            Quaternion rotationYOnly = Quaternion.Euler(transform.rotation.eulerAngles.x, rotationTarget.eulerAngles.y, transform.rotation.eulerAngles.z);
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, rotationYOnly, Time.deltaTime * lookRotationSpeed);
        }
        
        


        if (!canDash) {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashCooldown) {
                dashTimer = 0;
                canDash = true;
            }
        }

        if (!canGroundCheck)
        {
            groundCheckTimer += Time.deltaTime;

            if (groundCheckTimer >= groundCheckCooldown)
            {
                groundCheckTimer = 0;
                canGroundCheck = true;
            }
        }
    }


    void FixedUpdate() {

        if (canGroundCheck)
        {
            GroundCheck();
        }
        

        bool isJumpThisFrame = isJumpPressed && isJumpPressed != lastJumpPressed;
        bool isDashThisFrame = isDashPressed && isDashPressed != lastDashPressed;
        lastJumpPressed = isJumpPressed;
        lastDashPressed = isDashPressed;

        camForward = cam.transform.forward;
        camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward.normalized;
        Vector3 rightRelative = movementVector.x * camRight.normalized;

        relativeMovementVector = (forwardRelative + rightRelative).normalized * moveSpeed;
        relativeMovementVector.y = 0f;

        float rateOfVelocityChange;

        if(grounded) {

            rateOfVelocityChange = acceleration;

            if(relativeMovementVector.sqrMagnitude > Mathf.Epsilon) {
                if(Vector3.Dot(rb.linearVelocity.normalized, relativeMovementVector) > 0) { //accelerating

                    /* rotate the player's velocity towards their desired movement direction faster
                    than acceleration would normally allow so they don't feel like they're on ice. */
                    float blend = 1 - Mathf.Pow(0.5f, Time.fixedDeltaTime * velocitySlerp);
                    rb.linearVelocity = Vector3.RotateTowards(rb.linearVelocity, relativeMovementVector.normalized * rb.linearVelocity.magnitude + Vector3.up * rb.linearVelocity.y, blend, 0);

                }
                else { //decelerating 

                    // abruptly stop when trying to run the opposite way.
                    rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, relativeMovementVector.normalized);

                }
            }

            if(isJumpThisFrame) { // jumpInput.WasPressedThisFrame())
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, jumpForce, relativeMovementVector.z), ForceMode.Impulse);
            }
            else if(isDashThisFrame && canDash) { //dashInput.WasPressedThisFrame())
                canDash = false;
                rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * dashForce, ForceMode.Impulse);
            }
            
            rb.linearVelocity += Time.fixedDeltaTime * gravity;

        }
        else { // airborne

            rateOfVelocityChange = airStrafe;

            /* Increase gravity after the climax of our jump or releasing the jump key to make the
            character feel less floaty and allow the player to vary their jump height. */
            if (rb.linearVelocity.y > 0 && isJumpPressed) {
                rb.linearVelocity += Time.fixedDeltaTime * gravity;
            }
            else {
                /* Apparently, putting floats to the left of vectors when multiplying is better for
                performance. I'm sure the compiler just does that for us though. */
                rb.linearVelocity += fallingGravityModifier * Time.fixedDeltaTime * gravity;
            }
                
        }

        // acceleration
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, relativeMovementVector + Vector3.up * rb.linearVelocity.y, rateOfVelocityChange * Time.fixedDeltaTime);
    }

    void GroundCheck() {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - (playerHeight / 2), transform.position.z), groundCheckRadius, groundLayer);
        
    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + relativeMovementVector);

        if (rb != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -1 * (playerHeight / 2), 0), groundCheckRadius);
    }
}
