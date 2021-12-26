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

    // Private \\
    private TriggerListener triggerListener;

    private SwapTriggerLane swapTriggerLane;
    private Sequence triggerLane_Sequence;
    private UnityMessage swapLane_Out;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		swapLane_Out = ExtensionMethods.EmptyMethod;
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
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Complete );
	}

    public void SwapLane_Fame( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		swapLane_Out = SwapLane_Out_Fame;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Complete );
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
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Complete );
	}

    private void OnSwapTriggerLane_In_Complete()
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
