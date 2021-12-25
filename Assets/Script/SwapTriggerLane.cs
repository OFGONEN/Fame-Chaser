/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public abstract class SwapTriggerLane : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private TriggerListener_Enter swap_trigger;
    [ BoxGroup( "Setup" ), SerializeField ] private Transform swap_point_int;
    [ BoxGroup( "Setup" ), SerializeField ] private Transform swap_point_out;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		swap_trigger.Subscribe( SwapPlayerIn );
	}

    private void OnDisable()
    {
		swap_trigger.Unsubscribe( SwapPlayerIn );
    }
#endregion

#region API
#endregion

#region Implementation
	protected abstract void SwapPlayerIn( Collider player );

    protected virtual float SwapPlayerOut()
    {
		var randomness = GameSettings.Instance.swap_point_out_randomness;
		return swap_point_out.position.x + Random.Range( -randomness, randomness );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
