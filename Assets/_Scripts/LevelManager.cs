using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject upgradePrefab;

    GameObject[] enemyCars;

    void Start()
    {
        enemyCars = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        
    }
}
