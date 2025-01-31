using System;
using UnityEngine;

public class WheelControl : MonoBehaviour {
    public Transform wheelModel;

    [HideInInspector] public WheelCollider WheelCollider;

    // Create properties for the CarControl script
    // (You should enable/disable these via the 
    // Editor Inspector window)
    public bool steerable;
    public bool motorized;
    private bool _isFlat;
    public bool isFlat {
        get {
            return _isFlat;
        }
        set {
            _isFlat = value;

            // these variables HAVE to be defined. If you try to set stiffness all in one line you'll get an error.
            WheelFrictionCurve forwardCurve = WheelCollider.forwardFriction;
            WheelFrictionCurve sidewaysCurve = WheelCollider.sidewaysFriction;

            if(_isFlat) {
                WheelCollider.radius = flatRadius;
                forwardCurve.stiffness = flatStiffness;
                sidewaysCurve.stiffness = flatStiffness;
            } else {
                WheelCollider.radius = startRadius;
                forwardCurve.stiffness = 1;
                sidewaysCurve.stiffness = 1;
            }

            WheelCollider.forwardFriction = forwardCurve;
            WheelCollider.sidewaysFriction = sidewaysCurve;
        }
    }

    public float flatRadius = 0.15f;
    public float flatStiffness = 0.25f;

    Vector3 position;
    Quaternion rotation;

    float startRadius;

    private void Start() {
        WheelCollider = GetComponent<WheelCollider>();
        startRadius = WheelCollider.radius;
    }

    void Update() {
        // Get the Wheel collider's world pose values and
        // use them to set the wheel model's position and rotation
        WheelCollider.GetWorldPose(out position, out rotation);
        wheelModel.transform.position = position;
        wheelModel.transform.rotation = rotation;
    }
}