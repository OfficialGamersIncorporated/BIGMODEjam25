using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetOnContact : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if(!rb) return;
        if(!rb.gameObject.CompareTag("Player")) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
