using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _mainCameraObj;
    [SerializeField] private GameObject _restaurantCameraObj;


    private void Start()
    {
        if(TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeStateChanged += HandleCameraSwitch;
        }
    }

    private void HandleCameraSwitch(_mTimeState state)
    {
        bool isRestaurant = (state == _mTimeState.Restaurant);
        _mainCameraObj.SetActive(!isRestaurant);
        _restaurantCameraObj.SetActive(isRestaurant);
    }

}
