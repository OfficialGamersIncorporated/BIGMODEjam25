using System;
using TMPro;
using UnityEngine;

public class PersistentFeller : MonoBehaviour
{
    public static float timer;

    public bool bonus1 = false;
    public bool bonus2 = false;
    public bool bonus3 = false;
    public bool bonus4 = false;
    public bool bonus5 = false;

    public TextMeshProUGUI text;

    private void Awake()
    {
        text.text = ("0.00");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        text.text = ("Time: ") + Math.Round(timer, 2, MidpointRounding.AwayFromZero);
    }
}
