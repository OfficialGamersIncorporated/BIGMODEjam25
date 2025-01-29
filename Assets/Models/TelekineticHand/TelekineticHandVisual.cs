using System.Collections.Generic;
using UnityEngine;

public class TelekineticHandVisual : MonoBehaviour {

    public Transform baseAttachPoint;
    public Transform targetPoint;
    public Transform endNode;
    public int nodeCount = 2;
    public float armDragLerpSpeed = 2;

    //Vector3 delayedMidPostion;
    List<Vector3> delayedMidPositions;

    void OnEnable() {
        delayedMidPositions = new List<Vector3>(nodeCount + 1);
        //delayedMidPostion = targetPoint.position;
        for(int i = 0; i < nodeCount; i++) {
            delayedMidPositions.Insert(i, Vector3.Lerp(baseAttachPoint.position, targetPoint.position, (i / nodeCount)));
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
        float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * armDragLerpSpeed);
        Vector3 targetMidPosition = Vector3.Lerp(baseAttachPoint.position, targetPoint.position, 2f/3f);

        for(int i = 0; i < nodeCount; i++) {
            //print(i);
            Vector3 previousPos = targetPoint.position;
            if(i > 0) previousPos = delayedMidPositions[i - 1];

            Vector3 nextPos = baseAttachPoint.position;
            if(i < nodeCount - 1) nextPos = delayedMidPositions[i + 1];

            Vector3 targetMidPos = Vector3.Lerp(nextPos, previousPos, 0.5f);
            Vector3 currentMidPos = delayedMidPositions[i];
            delayedMidPositions[i] = Vector3.Lerp(currentMidPos, targetMidPos, blend);

            Transform node = GetNodeFromIndex(i + 1);
            node.position = delayedMidPositions[i];
            node.rotation = Quaternion.LookRotation((nextPos - previousPos).normalized) * Quaternion.Euler(90, 180, 0);
        }

        endNode.position = targetPoint.position;
        endNode.rotation = Quaternion.LookRotation((targetPoint.position - GetNodeFromIndex(1).position).normalized, Vector3.up) * Quaternion.Euler(90,0,0);

        transform.LookAt(GetNodeFromIndex(nodeCount));
    }
}
