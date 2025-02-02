using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    LevelManager levelManager;

    [SerializeField] GameObject upgradeSpawnPoint;

    DamageOnContact damageOnContact;
    EnemyMovementV2 enemyMovement;
    Vehicle vehicle;
    VehicleEffects effects;



    public bool LastAlive;

    void Start()
    {
        levelManager = LevelManager.Instance;

        damageOnContact = gameObject.GetComponent<DamageOnContact>();
        enemyMovement = gameObject.GetComponent<EnemyMovementV2>();
        vehicle = gameObject.GetComponent<Vehicle>();
        effects = gameObject.GetComponent<VehicleEffects>();
    }

    public void Die()
    {
        damageOnContact.enabled = false;
        enemyMovement.enabled = false;
        vehicle.enabled = false;
        effects.enabled = false;
        gameObject.GetComponent<Rigidbody>().linearDamping = 5;

        if (LastAlive && !levelManager.UpgradeHasSpawned)
        {
            GameObject spawnedUpgrade = Instantiate(levelManager.upgradePrefab, new Vector3(upgradeSpawnPoint.transform.position.x, upgradeSpawnPoint.transform.position.y + 1, upgradeSpawnPoint.transform.position.z), Quaternion.identity);
            levelManager.UpgradeHasSpawned = true;
        }

        levelManager.EnemyCarsList.Remove(gameObject);
    }
}
