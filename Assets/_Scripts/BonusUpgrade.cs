using System;
using UnityEngine;

public class BonusUpgrade : MonoBehaviour
{
    UpgradeManager upgradeManager;
    [SerializeField] GameObject mcGuffin;
    [SerializeField] float waitTime = 1;
    float tempWaitTimer;

    bool triggered = false;

    bool upgradeGiven = false;
    
    void Start()
    {
        if (mcGuffin == null)
        {
            Debug.LogError("NO MCGUFFIN WTF");
        }
        upgradeManager = UpgradeManager.Instance;
        if (upgradeManager == null)
        {
            Debug.LogError("NO UPGRADE MANAGER OH NO");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == mcGuffin && !upgradeGiven)
        {
            triggered = true;
            Debug.Log("HOLY FREAK YOU DID IT");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == mcGuffin && !upgradeGiven)
        {
            triggered = false;
            tempWaitTimer = 0;
            Debug.Log("MCGUFFIN REMOVED");
        }
    }

    private void Update()
    {
        if (triggered && !upgradeGiven)
        {
            tempWaitTimer += Time.deltaTime;
            if (tempWaitTimer >= waitTime)
            {
                // GIVE UPGRADE WAHOO
                Debug.Log("UPGRADE GET");
                upgradeGiven = true;
            }
        }
    }
}
