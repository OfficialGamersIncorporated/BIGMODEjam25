using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFocusControl : MonoBehaviour {

    private static PlayerFocusControl _instance;
    public static PlayerFocusControl Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("PlayerFocusControl is null");
            return _instance;
        }
    }

    public enum PlayerFocus { OnFoot, Vehicle };

    public PlayerFocus playerFocus = PlayerFocus.Vehicle;

    Vehicle PlayerVehicle;
    PlayerMovement PlayerOnFoot;
    CameraOrbit CameraControl;
    PlayerFocus lastPlayerFocus = PlayerFocus.Vehicle;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction jumpAction;
    InputAction interactAction;

    void Awake()
    {
        _instance = this;

        PlayerVehicle = GetComponentInChildren<Vehicle>();
        PlayerOnFoot = GetComponentInChildren<PlayerMovement>();
        CameraControl = GetComponentInChildren<CameraOrbit>();

        if (playerFocus == PlayerFocus.Vehicle)
        {
            PlayerOnFoot.gameObject.SetActive(false);
            RefreshCameraTarget();
        }
    }

    void Start() {


        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");


    }

    void RefreshCameraTarget() {
        if(playerFocus == PlayerFocus.OnFoot)
            CameraControl.orbitPoint = PlayerOnFoot.transform;
        else
            CameraControl.orbitPoint = PlayerVehicle.transform;
    }
    void Update() {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool sprintValue = sprintAction.IsPressed();
        bool jumpValue = jumpAction.IsPressed();

        if(interactAction.WasPressedThisFrame()) {
            if(playerFocus == PlayerFocus.OnFoot)
                playerFocus = PlayerFocus.Vehicle;
            else
                playerFocus = PlayerFocus.OnFoot;
        }

        if (playerFocus != lastPlayerFocus){
            RefreshCameraTarget();

            lastPlayerFocus = playerFocus;
        }

        if(PlayerVehicle) {
            if(playerFocus == PlayerFocus.Vehicle) {
                PlayerVehicle.SteeringInput = moveValue.x;
                PlayerVehicle.ThrottleInput = Mathf.Max(0, moveValue.y);
                PlayerVehicle.BrakeInput = Mathf.Max(0, -moveValue.y);
                PlayerVehicle.UrgencyInput = sprintValue;
                PlayerVehicle.canBrakeAsReverse = true;
            } else {
                PlayerVehicle.SteeringInput = 0;
                PlayerVehicle.ThrottleInput = 0;
                PlayerVehicle.BrakeInput = 1;
                PlayerVehicle.UrgencyInput = false;
                PlayerVehicle.canBrakeAsReverse = false;
            }
        }

        if(PlayerOnFoot) {
            if(playerFocus == PlayerFocus.OnFoot) {
                PlayerOnFoot.movementVector = moveValue;
                PlayerOnFoot.isDashPressed = sprintValue;
                PlayerOnFoot.isJumpPressed = jumpValue;
                if(!PlayerOnFoot.gameObject.activeSelf) {
                    PlayerOnFoot.transform.position = PlayerVehicle.driverExitPosition.position;

                    Rigidbody vehicleBody = PlayerOnFoot.GetComponent<Rigidbody>();
                    Rigidbody onFootBody = PlayerVehicle.GetComponent<Rigidbody>();
                    vehicleBody.linearVelocity = onFootBody.linearVelocity;

                    PlayerOnFoot.gameObject.SetActive(true);
                }
            } else {
                PlayerOnFoot.movementVector = Vector2.zero;
                PlayerOnFoot.isDashPressed = false;
                PlayerOnFoot.isJumpPressed = false;
                if(PlayerOnFoot.gameObject.activeSelf) PlayerOnFoot.gameObject.SetActive(false);
            }
        }
    }

    public GameObject GetCurrentPlayer()
    {
        if (playerFocus == PlayerFocus.OnFoot)
        {
            return PlayerOnFoot.gameObject;
        }
        else if (playerFocus == PlayerFocus.Vehicle)
        {
            return PlayerVehicle.gameObject;
        }
        else
        {
            print("something bad happened");
            return null;
        }
    }

}
