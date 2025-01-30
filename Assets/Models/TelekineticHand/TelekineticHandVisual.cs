using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TelekineticHandVisual : MonoBehaviour {

    public Telekensis telekensis;
    public FollowMouse followMouse;
    public Transform baseAttachPoint;
    public Transform cursorPoint;
    public Transform endNode;

    public int nodeCount = 2;
    public float armDragLerpSpeed = 2;
    public Vector3 handGripPointOffset;

    Animator animator;
    InputAction teleknesisAction;

    //Vector3 delayedMidPostion;
    List<Vector3> delayedMidPositions;

    Vector3 delayedTargetPossition;
    //GameObject lastHeldPart = null;

    void OnEnable() {
        teleknesisAction = InputSystem.actions.FindAction("Attack");
        animator = GetComponent<Animator>();
        delayedTargetPossition = cursorPoint.position;
        delayedMidPositions = new List<Vector3>(nodeCount + 1);
        //delayedMidPostion = targetPoint.position;
        for(int i = 0; i < nodeCount; i++) {
            delayedMidPositions.Insert(i, Vector3.Lerp(baseAttachPoint.position, cursorPoint.position, (i / nodeCount)));
        }
    }
    Transform GetNodeFromIndex(int index) {
        Transform node = endNode;
        //if(index == 0) return node;
        for(int i = 0; i < index; i++) {
            //print("Itterating one level up." + i.ToString());
            node = node.parent;
        }
        return node;
    }
    void Update() {
        Transform targetPoint = cursorPoint;
        if(telekensis.heldPart) targetPoint = telekensis.heldPart.transform;
        Vector3 targetPosition = targetPoint.position + endNode.rotation * handGripPointOffset;

        /*if (telekensis.heldPart != lastHeldPart) {
            lastHeldPart = telekensis.heldPart;
            if(telekensis.heldPart)
                animator.SetTrigger("Closed");
            else
                animator.SetTrigger("Opened");
        }*/
        if(teleknesisAction.WasPressedThisFrame())
            animator.SetTrigger("Closed");
        else if (teleknesisAction.WasReleasedThisFrame())
            animator.SetTrigger("Opened");

        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * armDragLerpSpeed);

        transform.LookAt(GetNodeFromIndex(nodeCount));

        for(int i = 0; i < nodeCount; i++) {
            //print(i);
            Vector3 previousPos = targetPosition;
            if(i > 0) previousPos = delayedMidPositions[i - 1];

            Vector3 nextPos = baseAttachPoint.position;
            if(i < nodeCount - 1) nextPos = delayedMidPositions[i + 1];

            Vector3 targetMidPos = Vector3.Lerp(nextPos, previousPos, 0.5f);
            Vector3 currentMidPos = delayedMidPositions[i];
            delayedMidPositions[i] = Vector3.Lerp(currentMidPos, targetMidPos, blend) + Physics.gravity * 0.1f * Time.deltaTime;

            Transform node = GetNodeFromIndex(i + 1);
            node.position = delayedMidPositions[i];
            node.rotation = Quaternion.LookRotation((nextPos - previousPos).normalized) * Quaternion.Euler(90, 180, 0);
        }

        //delayedTargetPossition = Vector3.MoveTowards(delayedTargetPossition, targetPosition)

        endNode.position = targetPosition;
        endNode.rotation = Quaternion.LookRotation((targetPoint.position - GetNodeFromIndex(1).position).normalized, Vector3.up) * Quaternion.Euler(90,0,0);

        /*Vector3 targetLookVect = (targetPosition - delayedTargetPossition); //(followMouse.pointerPos - targetPosition).normalized;
        delayedTargetPossition = targetPosition;
        if (targetLookVect.magnitude > 0.01f)
            targetRotation = Quaternion.LookRotation(targetLookVect.normalized, Vector3.up) * Quaternion.Euler(90, 0, 0);
        endNode.rotation = Quaternion.Slerp(endNode.rotation, targetRotation, blend);*/

        //endNode.position += endNode.rotation * handGripPointOffset;

    }
}
