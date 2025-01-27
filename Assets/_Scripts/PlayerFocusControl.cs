using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFocusControl : MonoBehaviour {

    public enum PlayerFocus { Character, Vehicle };

    public PlayerFocus playerFocus = PlayerFocus.Vehicle;

    Vehicle PlayerCar;
    PlayerMovement PlayerChar;
    PlayerFocus lastPlayerFocus = PlayerFocus.Vehicle;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction jumpAction;
    InputAction interactAction;

    void Start() {
        PlayerCar = GetComponentInChildren<Vehicle>();
        PlayerChar = GetComponentInChildren<PlayerMovement>();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");

        if(playerFocus == PlayerFocus.Vehicle) PlayerChar.gameObject.SetActive(false);
    }
    void Update() {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool sprintValue = sprintAction.IsPressed();
        bool jumpValue = jumpAction.IsPressed();

        if(interactAction.WasPressedThisFrame()) {
            if(playerFocus == PlayerFocus.Character)
                playerFocus = PlayerFocus.Vehicle;
            else
                playerFocus = PlayerFocus.Character;
        }

        if (playerFocus != lastPlayerFocus){
            lastPlayerFocus = playerFocus;
        }

        if(PlayerCar) {
            if(playerFocus == PlayerFocus.Vehicle) {
                PlayerCar.SteeringInput = moveValue.x;
                PlayerCar.ThrottleInput = Mathf.Max(0, moveValue.y);
                PlayerCar.BrakeInput = Mathf.Max(0, -moveValue.y);
                PlayerCar.UrgencyInput = sprintValue;
                PlayerCar.canBrakeAsReverse = true;
            } else {
                PlayerCar.SteeringInput = 0;
                PlayerCar.ThrottleInput = 0;
                PlayerCar.BrakeInput = 1;
                PlayerCar.UrgencyInput = false;
                PlayerCar.canBrakeAsReverse = false;
            }
        }

        if(PlayerChar) {
            if(playerFocus == PlayerFocus.Character) {
                PlayerChar.movementVector = moveValue;
                PlayerChar.isDashPressed = sprintValue;
                PlayerChar.isJumpPressed = jumpValue;
                if(!PlayerChar.gameObject.activeSelf) {
                    PlayerChar.transform.position = PlayerCar.driverExitPosition.position;
                    PlayerChar.gameObject.SetActive(true);
                }
            } else {
                PlayerChar.movementVector = Vector2.zero;
                PlayerChar.isDashPressed = false;
                PlayerChar.isJumpPressed = false;
                if(PlayerChar.gameObject.activeSelf) PlayerChar.gameObject.SetActive(false);
            }
        }
    }
}
