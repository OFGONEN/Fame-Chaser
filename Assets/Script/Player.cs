/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
#region Fields
	[ BoxGroup( "Event Listeners" ) ] public EventListenerDelegateResponse swipe_left_listener;
	[ BoxGroup( "Event Listeners" ) ] public EventListenerDelegateResponse swipe_right_listener;

    // Private \\
    private TriggerListener triggerListener;

    private SwapTriggerLane swapTriggerLane;
    private Sequence triggerLane_Sequence;
    private UnityMessage swapLane_Out;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnEnable()
	{
		swipe_left_listener.OnEnable();
		swipe_right_listener.OnEnable();
	}

	private void OnDisable()
	{
		swipe_left_listener.OnDisable();
		swipe_right_listener.OnDisable();
	}

    private void Awake()
    {
		swipe_left_listener.response  = ExtensionMethods.EmptyMethod;
		swipe_right_listener.response = ExtensionMethods.EmptyMethod;

		swapLane_Out    = ExtensionMethods.EmptyMethod;
		triggerListener = GetComponentInChildren< TriggerListener >();
    }
#endregion

#region API
    public void SwapLane_Money( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		swapLane_Out = SwapLane_Out_Money;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Money_Complete );
	}

    public void SwapLane_Fame( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		swapLane_Out = SwapLane_Out_Fame;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Fame_Complete );
	}
#endregion

#region Implementation
    [ Button() ]
    private void SwapLane_Main()
    {
		var position_out = swapTriggerLane.SwapPlayerOut();

		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = null;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position_out, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_Out_Complete );
	}

    private void OnSwapTriggerLane_In_Money_Complete()
    {
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();
	}

    private void OnSwapTriggerLane_In_Fame_Complete()
    {
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();
	}

    private void OnSwapTriggerLane_Out_Complete()
	{
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();

		swapLane_Out();
	}

    private void SwapLane_Out_Fame()
    {

    }

    private void SwapLane_Out_Money()
    {

    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
