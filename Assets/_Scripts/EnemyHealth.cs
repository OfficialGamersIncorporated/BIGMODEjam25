using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    LevelManager levelManager;

    [SerializeField] GameObject upgradeSpawnPoint;
    public TelekinesisInteractTarget dummyUpgradeInteract;

    [Tooltip("This game object gets assigned by level manager when last car")]
    public GameObject dummyUpgradeGo;

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
            if (dummyUpgradeGo != null)
            {
                dummyUpgradeGo.SetActive(true);
                
                dummyUpgradeInteract.PrefabSpawnOnGrab = levelManager.upgradePrefab.GetComponent<Rigidbody>();
            }
            else
            {
                print("No upgrade :((");
            }


            levelManager.UpgradeHasSpawned = true;


            levelManager.AllEnemiesDefeated.Invoke();
        }

        levelManager.EnemyCarsList.Remove(gameObject);
    }
}
