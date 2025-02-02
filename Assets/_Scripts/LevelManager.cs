using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("LevelManager is null");
            return _instance;
        }
    }

    public GameObject upgradePrefab;

    GameObject[] tempGOs;
    public List<GameObject> EnemyCarsList = new();

    int numberOfEnemies;
    public bool UpgradeHasSpawned = false;


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        tempGOs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in tempGOs)
        {
            EnemyCarsList.Add(go);
        }
    }

    void Update()
    {
        numberOfEnemies = EnemyCarsList.Count;

        if (numberOfEnemies == 1)
        {
            EnemyCarsList[0].GetComponent<EnemyHealth>().LastAlive = true;
        }
    }
}
