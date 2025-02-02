using System.Collections;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    UpgradeManager upgradeManager;

    [Header("Upgrade Values (All values will be applied)")]
    public float stiffnessMult;
    public bool FourWheelDrive;
    public float motorTorque;
    public float maxSpeed;
    public float steerRangeAtMaxSpeed;
    public float teleForce;
    public float teleMaxRange;
    
    [Header("Anim")]
    [SerializeField] float animationSpeed = 1;
    [SerializeField] float animStart = 1;
    [SerializeField] float animEnd = 0.25f;
    GameObject upgradePos;
    MeshRenderer mesh;

    bool canUpgrade = true;
    Rigidbody rb;
    Collider col;

    void Start()
    {
        upgradeManager = UpgradeManager.Instance;
        upgradePos = upgradeManager.upgradePos;
        mesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpgradeManager upgradeManager = collision.gameObject.GetComponent<UpgradeManager>();
        if (!upgradeManager)
        {
            return;
        }
        else if (canUpgrade)
        {
            StartCoroutine(UpgradeAnimation());
            canUpgrade = false;
        }

    }

    IEnumerator UpgradeAnimation()
    {
        rb.isKinematic = true;
        col.isTrigger = true;
        gameObject.tag = "Untagged";
        transform.SetParent(upgradePos.transform);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        float currentFloat = mesh.material.GetFloat("_UnburntAlpha");

        while (currentFloat > animEnd)
        {
            currentFloat = mesh.material.GetFloat("_UnburntAlpha");
            print(currentFloat);
            mesh.material.SetFloat("_UnburntAlpha", Mathf.MoveTowards(currentFloat, animEnd, animationSpeed * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }

        gameObject.tag = "Telekinetic";
        transform.parent = null;
        rb.isKinematic = false;
        col.isTrigger = false;

        
        DoTheUpgrade();
        yield return null;
    }

    void DoTheUpgrade()
    {
        upgradeManager.UpgradeStiffnessMult(stiffnessMult);

        if (FourWheelDrive)
        {
            upgradeManager.Upgrade4WD();
        }

        upgradeManager.UpgradeMotorTorque(motorTorque);

        upgradeManager.UpgradeMaxSpeed(maxSpeed);

        upgradeManager.UpgradeSteerRangeAtMaxSpeed(steerRangeAtMaxSpeed);

        upgradeManager.UpgradeTeleForce(teleForce);

        upgradeManager.UpgradeTeleMaxRange(teleMaxRange);
    }
}
