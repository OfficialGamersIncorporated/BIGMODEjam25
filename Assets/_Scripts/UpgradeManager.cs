using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Vehicle vehicle;
    [SerializeField] Telekensis telekensis;
    [SerializeField] GameObject[] wheels;
    public GameObject upgradePos;

    static bool getDefaults = true;

    private static UpgradeManager _instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UpgradeManager is null");
            return _instance;
        }
    }

    public static float HeldStiffness;
    public static bool Held4WD;
    public static float HeldMotorTorque;
    public static float HeldMaxSpeed;
    public static float HeldSteerMaxSpeed;
    public static float HeldTeleForce;
    public static float HeldTeleRange;

    private void Awake()
    {
        _instance = this;
    }


    private void Start()
    {
        if (getDefaults)
        {
            SaveStartingStats();
            getDefaults = false;
        }
        else
        {
            ApplyStats();
        }

    }


    // !This code is not safe to look at, you have been warned!


    void SaveStartingStats()
    {
        HeldStiffness = wheels[0].GetComponent<WheelControl>().stiffnessMult;
        Held4WD = false;
        HeldMotorTorque = vehicle.motorTorque;
        HeldMaxSpeed = vehicle.maxSpeed;
        HeldSteerMaxSpeed = vehicle.steeringRangeAtMaxSpeed;
        HeldTeleForce = telekensis.TeleForce;
        HeldTeleRange = telekensis.MaxRange;
    }

    void ApplyStats()
    {
        UpgradeStiffnessMult(HeldStiffness);

        if (Held4WD)
        {
            Upgrade4WD();
        }

        UpgradeMotorTorque(HeldMotorTorque);

        UpgradeMaxSpeed(HeldMaxSpeed);

        UpgradeSteerRangeAtMaxSpeed(HeldSteerMaxSpeed);

        UpgradeTeleForce(HeldTeleForce);

        UpgradeTeleMaxRange(HeldTeleRange);
    }

    // WHEELS
    public void UpgradeStiffnessMult(float stiffnessMultParam)
    {
        HeldStiffness = stiffnessMultParam;
        foreach (var wheel in wheels)
        {
            wheel.GetComponent<WheelControl>().UpgradeStiffnessMult(stiffnessMultParam);
        }
    }
    public void Upgrade4WD()
    {
        Held4WD = true;
        foreach (var wheel in wheels)
        {
            wheel.GetComponent<WheelControl>().MakeMotorized();
        }
    }

    // TRUCK ENGINE
    public void UpgradeMotorTorque(float motorTorqueParam)
    {
        HeldMotorTorque = motorTorqueParam;
        vehicle.UpgradeMotorTorque(motorTorqueParam);
    }
    public void UpgradeMaxSpeed(float maxSpeedParam)
    {
        HeldMaxSpeed = maxSpeedParam;
        vehicle.UpgradeMaxSpeed(maxSpeedParam);
    }
    public void UpgradeSteerRangeAtMaxSpeed(float steerRangeAtMaxSpeedParam)
    {
        HeldSteerMaxSpeed = steerRangeAtMaxSpeedParam;
        vehicle.UpgradeSteerRangeAtMaxSpeed(steerRangeAtMaxSpeedParam);
    }

    // TELEKENSIS
    public void UpgradeTeleForce(float forceParam)
    {
        HeldTeleForce = forceParam;
        telekensis.UpgradeTeleForce(forceParam);
    }
    public void UpgradeTeleMaxRange(float teleMaxRangeParam)
    {
        HeldTeleRange = teleMaxRangeParam;
        telekensis.UpgradeTeleMaxRange(teleMaxRangeParam);
    }
}
