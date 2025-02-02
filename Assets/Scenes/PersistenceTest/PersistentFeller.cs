using UnityEngine;

public class PersistentFeller : MonoBehaviour
{
    [SerializeField] float wahoo = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
