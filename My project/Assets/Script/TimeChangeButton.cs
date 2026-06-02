using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeChangeButton : MonoBehaviour
{
    [SerializeField]
    private Button Button_Base;
    [SerializeField]
    private Animator Animator_Coin;

    private Action _onTimeChangeCompleteCallback;
    private bool _isAnimating = false;

    private void Awake()
    {
        if(Button_Base == null )
        {
            Button_Base = GetComponent<Button>();
        }
        if( Animator_Coin == null )
        {
            Animator_Coin = GetComponent<Animator>();
        }

        Button_Base.onClick.RemoveAllListeners();
        Button_Base.onClick.AddListener(OnClickTimeChangeButton);
    }

    private void OnDisable()
    {
        Button_Base.onClick.RemoveAllListeners();
    }

    public void BindOnTimeChangeCompleteEvent(Action onCompleteCallback)
    {
        this._onTimeChangeCompleteCallback = onCompleteCallback;
    }

    private void OnClickTimeChangeButton()
    {
        if(_isAnimating)
        {
            return;
        }
        bool isCurrentlyMorning = Animator_Coin.GetBool("IsMorning");

        StartCoroutine(PlayerTransitionRoutine(isCurrentlyMorning));
    }

    private IEnumerator PlayerTransitionRoutine(bool isCurrentlyMorning)
    {
        _isAnimating = true;
        string targetStateName = "";

        if(isCurrentlyMorning )
        {
            Animator_Coin.SetTrigger("OnChangedToNight");
            Animator_Coin.SetBool("IsMorning", false);
            targetStateName = "TimeSetNight";
        }
        else
        {
            Animator_Coin.SetTrigger("OnChangedToMorning");
            Animator_Coin.SetBool("IsMorning", true);
            targetStateName = "TimeSetMorning";
        }

        yield return null;

        var stateInfo = Animator_Coin.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(targetStateName))
        {
            yield return new WaitForSeconds(stateInfo.length);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        _isAnimating = false;

        _onTimeChangeCompleteCallback?.Invoke();
    }





}
