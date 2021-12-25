/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using UnityEditor;

public abstract class SwapTriggerLane : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] protected TriggerListener_Enter swap_trigger;
    [ BoxGroup( "Setup" ), SerializeField ] protected Transform swap_point_in;
    [ BoxGroup( "Setup" ), SerializeField ] protected Transform swap_point_out;
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
	protected abstract void SwapPlayerIn( Collider collider );

    protected virtual float SwapPlayerOut()
    {
		var randomness = GameSettings.Instance.swap_point_out_randomness;
		return swap_point_out.position.x + Random.Range( -randomness, randomness );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		var position = swap_point_out.position;
		var position_left = swap_point_out.position - Vector3.left * GameSettings.Instance.swap_point_out_randomness;
		var position_right = swap_point_out.position - Vector3.right * GameSettings.Instance.swap_point_out_randomness;

		Handles.color = Color.blue;
		Handles.DrawDottedLine( position_left, position_right, 1f );
		Handles.Label( position + Vector3.up, "Swap Point Out" );
		Handles.DrawSolidDisc( position, Vector3.up, 1 );

		Handles.color = Color.yellow;
		Handles.DrawSolidDisc( position_left, Vector3.up, 1 );
		Handles.DrawSolidDisc( position_right, Vector3.up, 1 );
	}
#endif
#endregion
}
