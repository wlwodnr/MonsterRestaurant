using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    private bool isMoring;
    [SerializeField] private Light2D globalLite2D;

    public void TimeSetNight()
    {
        isMoring = false;
        Debug.Log("밤으로 변경됩니다.");
        globalLite2D.intensity = 0.4f;
    }
    public void TimeSetMoring()
    {
        isMoring = true;
        Debug.Log("낮으로 변경됩니다.");
        globalLite2D.intensity = 1f;


    }
}
