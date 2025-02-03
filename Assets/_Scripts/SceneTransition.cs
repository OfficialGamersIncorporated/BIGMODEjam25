using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    bool loading = false;

    PersistentFeller persistentFeller;


    public string SceneName = "";


    private void Start()
    {
        persistentFeller = PersistentFeller.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!loading)
        {
            loading = true;
            persistentFeller.IncreaseTireCount();


            Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
            if (!otherRB.transform.CompareTag("Player")) return;

            StartCoroutine(NextScene());
        }

    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.5f);
        if (SceneName != "") SceneManager.LoadScene(SceneName);
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }
}
