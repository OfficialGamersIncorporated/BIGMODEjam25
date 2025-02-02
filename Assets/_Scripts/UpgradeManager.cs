using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] Vehicle vehicle;
    [SerializeField] Telekensis telekensis;
    [SerializeField] GameObject[] wheels;

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

    private void Awake()
    {
        _instance = this;
    }





    // !This code is not safe to look at, you have been warned!


    // WHEELS
    public void UpgradeStiffnessMult(float stiffnessMultParam)
    {
        foreach (var wheel in wheels)
        {
            wheel.GetComponent<WheelControl>().UpgradeStiffnessMult(stiffnessMultParam);
        }
    }
    public void Upgrade4WD()
    {
        foreach (var wheel in wheels)
        {
            wheel.GetComponent<WheelControl>().MakeMotorized();
        }
    }

    // TRUCK ENGINE
    public void UpgradeMotorTorque(float motorTorqueParam)
    {
        vehicle.UpgradeMotorTorque(motorTorqueParam);
    }
    public void UpgradeMaxSpeed(float maxSpeedParam)
    {
        vehicle.UpgradeMaxSpeed(maxSpeedParam);
    }
    public void UpgradeSteerRange(float steerRangeParam)
    {
        vehicle.UpgradeSteerRange(steerRangeParam);
    }
    public void UpgradeSteerRangeAtMaxSpeed(float steerRangeAtMaxSpeedParam)
    {
        vehicle.UpgradeSteerRangeAtMaxSpeed(steerRangeAtMaxSpeedParam);
    }

    // TELEKENSIS
    public void UpgradeTeleForce(float forceParam)
    {
        telekensis.UpgradeTeleForce(forceParam);
    }
    public void UpgradeTeleSpeed(float teleSpeedParam)
    {
        telekensis.UpgradeTeleSpeed(teleSpeedParam);
    }
    public void UpgradeTeleMaxRange(float teleMaxRangeParam)
    {
        telekensis.UpgradeTeleMaxRange(teleMaxRangeParam);
    }
}
