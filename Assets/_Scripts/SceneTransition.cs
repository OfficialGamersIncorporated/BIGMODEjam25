using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public string SceneName = "";

    private void OnTriggerEnter(Collider other) {
        Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
        if(!otherRB.transform.CompareTag("Player")) return;

        if(SceneName != "") SceneManager.LoadScene(SceneName);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
