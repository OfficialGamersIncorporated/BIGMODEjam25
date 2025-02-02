using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    //public Scene TargetScene;

    private void OnTriggerEnter(Collider other) {
        Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
        if(!otherRB.transform.CompareTag("Player")) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
