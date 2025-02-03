using System;
using TMPro;
using UnityEngine;
using static BonusUpgrade;

public class PersistentFeller : MonoBehaviour
{
    public bool debug = false;

    public static float timer;
    float debugTimer;

    public static int TiresCollected;
    int debugTiresCollected;

    TiresHeld tiresHeld;

    public static bool bonus1Achieved = false;
    bool debugBonus1Achieved;
    public static bool bonus2Achieved = false;
    bool debugBonus2Achieved;
    public static bool bonus3Achieved = false;
    bool debugBonus3Achieved;
    public static bool bonus4Achieved = false;
    bool debugBonus4Achieved;
    public static bool bonus5Achieved = false;
    bool debugBonus5Achieved;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI tiresText;


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
    }

    private void Start()
    {
        tiresHeld = TiresHeld.Instance;

        //timerText.text = ("Time: ") + Math.Round(timer, 2, MidpointRounding.AwayFromZero);
        timerText.text = ("Time: ") + TimeSpan.FromSeconds(Mathf.Round(timer)).ToString(@"mm\:ss");
        tiresText.text = ("Tires: ") + TiresCollected;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        timerText.text = ("Time: ") + TimeSpan.FromSeconds(Mathf.Round(timer)).ToString(@"mm\:ss");

        if (debug)
        {
            debugTimer = timer;
            debugTiresCollected = TiresCollected;
            debugBonus1Achieved = bonus1Achieved;
            debugBonus2Achieved = bonus2Achieved;
            debugBonus3Achieved = bonus3Achieved;
            debugBonus4Achieved = bonus4Achieved;
            debugBonus5Achieved = bonus5Achieved;
        }
    }

    public void IncreaseTireCount()
    {
        TiresCollected += tiresHeld.SortTireList();
        tiresText.text = ("Tires: ") + TiresCollected;
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
