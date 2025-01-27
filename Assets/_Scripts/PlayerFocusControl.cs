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

    void Start() {
        PlayerCar = GetComponentInChildren<Vehicle>();
        PlayerChar = GetComponentInChildren<PlayerMovement>();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }
    void Update() {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool sprintValue = sprintAction.IsPressed();

        if (playerFocus != lastPlayerFocus){
            lastPlayerFocus = playerFocus;
        }

        if (playerFocus == PlayerFocus.Vehicle) {
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

        if (playerFocus == PlayerFocus.Character) {
            // do other stuff to it
        } else {
            // kill your parents
        }
    }
}
