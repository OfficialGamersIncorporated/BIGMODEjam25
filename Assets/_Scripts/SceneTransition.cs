using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    PersistentFeller persistentFeller;


    public string SceneName = "";


    private void Start()
    {
        persistentFeller = PersistentFeller.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        persistentFeller.IncreaseTireCount();


        Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
        if(!otherRB.transform.CompareTag("Player")) return;

        if(SceneName != "") SceneManager.LoadScene(SceneName);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
