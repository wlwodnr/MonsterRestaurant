using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            timeManager.TimeSetMoring();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            timeManager.TimeSetNight();
        }
    }
}
