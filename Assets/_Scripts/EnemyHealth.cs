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

    void Update()
    {
        
    }


    // if last
    // get cig
    // spawn cig
    // die

    public void Die()
    {
        damageOnContact.enabled = false;
        enemyMovement.enabled = false;
        vehicle.enabled = false;
        effects.enabled = false;
        gameObject.GetComponent<Rigidbody>().linearDamping = 5;

        if (LastAlive)
        {
            GameObject spawnedUpgrade = Instantiate(levelManager.upgradePrefab, transform);

        }

        levelManager.EnemyCarsList.Remove(gameObject);
    }
}
