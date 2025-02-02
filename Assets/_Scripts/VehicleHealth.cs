using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VehicleHealth : MonoBehaviour {

    Vehicle vehicleControl;
    public Animator vehicleAnimator;
    public AudioSource TirePopSound;
    public AudioSource RepairSound;

    void Start() {
        vehicleControl = GetComponent<Vehicle>();
    }
    void Update() {

    }

    public bool TryDamage() {
        return TryPopATire();
    }
    public bool TryHeal() {
        bool worked = TryFixATire();
        if(worked) {
            vehicleAnimator.SetTrigger("Heal");
            if(RepairSound) RepairSound.Play();
        }
        return worked;
    }
    public bool TryPopATire() {
        foreach(WheelControl wheel in vehicleControl.wheels) {
            if(wheel.isFlat) continue;

            wheel.isFlat = true;
            if(TirePopSound) TirePopSound.Play();
            return true;
        }
        return false;
    }
    public bool TryFixATire() {
        foreach(WheelControl wheel in vehicleControl.wheels) {
            if(!wheel.isFlat) continue;

            wheel.isFlat = false;
            return true;
        }
        return false;
    }
}

#if (UNITY_EDITOR)
[CustomEditor(typeof(VehicleHealth))]
public class VehicleHealthEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        VehicleHealth myScript = (VehicleHealth)target;
        if(GUILayout.Button("Pop a tire!")) {
            myScript.TryPopATire();
        }
        if(GUILayout.Button("Fix a tire!")) {
            myScript.TryHeal();
        }
    }
}
#endif