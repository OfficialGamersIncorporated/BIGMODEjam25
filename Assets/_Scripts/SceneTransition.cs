using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public SceneAsset TargetScene;

    private void OnTriggerEnter(Collider other) {
        Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
        if(!otherRB.transform.CompareTag("Player")) return;

        SceneManager.LoadScene(TargetScene.name);
    }
}
