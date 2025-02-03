using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TiresHeld : MonoBehaviour
{
    public int tiresIGot = 0;
    public List<GameObject> TireList = new();

    PersistentFeller persistentFeller;

    private void Start()
    {
        persistentFeller = PersistentFeller.Instance;
    }

    //GameObject teleTire;
    //bool holding = false;


    //[SerializeField] Telekensis telekensis;

    //private void Update()
    //{
    //    if (telekensis.heldPart != null && telekensis.heldPart.TryGetComponent<HealingPickup>(out HealingPickup blarg) && !holding)
    //    {
    //        teleTire = telekensis.heldPart.GetComponent<HealingPickup>().gameObject;
    //        TireList.Add(teleTire);
    //        tiresIGot++;
    //        holding = true;
    //    }
    //    else if (telekensis.heldPart == null && holding)
    //    {
    //        TireList.Remove(teleTire);
    //        tiresIGot--;
    //        teleTire = null;
    //        holding = false;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<HealingPickup>(out HealingPickup itsATire))
        {
            TireList.Add(itsATire.gameObject);
            tiresIGot++;
            persistentFeller.tempTiresHeld++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HealingPickup>(out HealingPickup itsATire))
        {
            TireList.Remove(itsATire.gameObject);
            tiresIGot--;
            persistentFeller.tempTiresHeld--;
        }
    }
}
