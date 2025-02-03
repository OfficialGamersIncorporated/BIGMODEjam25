using System;
using TMPro;
using UnityEngine;
using static BonusUpgrade;

public class PersistentFeller : MonoBehaviour
{
    public static float timer;

    public static int TiresCollected;

    public static bool bonus1Achieved = false;
    public static bool bonus2Achieved = false;
    public static bool bonus3Achieved = false;
    public static bool bonus4Achieved = false;
    public static bool bonus5Achieved = false;

    public TextMeshProUGUI text;


    private static PersistentFeller _instance;
    public static PersistentFeller Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("PersistentFeller is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        text.text = ("0.00");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        text.text = ("Time: ") + Math.Round(timer, 2, MidpointRounding.AwayFromZero);
    }

    public void StoreBonus(BonusUpgrade.BonusID IDParam)
    {
        switch (IDParam)
        {
            case BonusID.bonus1:
                bonus1Achieved = true;
                break;
            case BonusID.bonus2:
                bonus2Achieved = true;
                break;
            case BonusID.bonus3:
                bonus3Achieved = true;
                break;
            case BonusID.bonus4:
                bonus4Achieved = true;
                break;
            case BonusID.bonus5:
                bonus5Achieved = true;
                break;
        }
    }
}
