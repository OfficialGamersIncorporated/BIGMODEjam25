using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStats : MonoBehaviour
{

    PersistentFeller persistentFeller;


    public TextMeshProUGUI finishTimerText;
    public TextMeshProUGUI tiresCollectedText;

    public GameObject bonus1;
    public GameObject bonus2;
    public GameObject bonus3;
    public GameObject bonus4;
    public GameObject bonus5;



    void Start()
    {
        persistentFeller = PersistentFeller.Instance;

        finishTimerText.text = ("Finish Time: ") + TimeSpan.FromSeconds(Mathf.Round(persistentFeller.GetTimer())).ToString(@"mm\:ss");
        tiresCollectedText.text = ("Tires Collected: ") + persistentFeller.GetTireCount();

        if (persistentFeller.GetBounus1()) bonus1.SetActive(true);
        if (persistentFeller.GetBounus2()) bonus2.SetActive(true);
        if (persistentFeller.GetBounus3()) bonus3.SetActive(true);
        if (persistentFeller.GetBounus4()) bonus4.SetActive(true);
        if (persistentFeller.GetBounus5()) bonus5.SetActive(true);

    }


    public void Menu()
    {
        // Clear data

        SceneManager.LoadScene(0);
    }
}
