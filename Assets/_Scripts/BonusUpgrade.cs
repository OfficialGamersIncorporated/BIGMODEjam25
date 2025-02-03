using System;
using UnityEngine;

public class BonusUpgrade : MonoBehaviour {
    UpgradeManager upgradeManager;
    PersistentFeller persistentFeller;
    [SerializeField] Rigidbody mcGuffin;
    [SerializeField] float waitTime = 1;
    float tempWaitTimer;

    bool triggered = false;

    bool bonusStored = false;

    public enum BonusID { bonus1, bonus2, bonus3, bonus4, bonus5 }

    public BonusID mybonusID;

    void Start() {
        if(mcGuffin == null) {
            Debug.LogError("NO MCGUFFIN WTF");
        }
        upgradeManager = UpgradeManager.Instance;
        persistentFeller = PersistentFeller.Instance;
        if(upgradeManager == null) {
            Debug.LogError("NO UPGRADE MANAGER OH NO");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == mcGuffin.gameObject && !bonusStored) {
            triggered = true;
            Debug.Log("MCGUFFIN IN");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject == mcGuffin.gameObject && !bonusStored) {
            triggered = false;
            tempWaitTimer = 0;
            Debug.Log("MCGUFFIN REMOVED");
        }
    }

    private void Update() {
        if(triggered && !bonusStored && mcGuffin.IsSleeping()) {
            tempWaitTimer += Time.deltaTime;
            if(tempWaitTimer >= waitTime) {
                
                persistentFeller.StoreBonus(mybonusID);

                bonusStored = true;
            }
        }
    }
}
