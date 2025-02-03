using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStats : MonoBehaviour
{

    PersistentFeller persistentFeller;

    float timer;

    public TextMeshProUGUI finishTimerText;
    public TextMeshProUGUI tiresCollectedText;

    int TiresCollected;

    bool bonus1Achieved;
    bool bonus2Achieved;
    bool bonus3Achieved;
    bool bonus4Achieved;
    bool bonus5Achieved;



    void Start()
    {
        persistentFeller = PersistentFeller.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Menu()
    {
        // Clear data

        SceneManager.LoadScene(0);
    }
}
