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
    [ BoxGroup( "Setup" ), SerializeField ] protected Transform swap_point_in;
    [ BoxGroup( "Setup" ), SerializeField ] protected Transform swap_point_out;
	
	// Private \\
    private TriggerListener swap_trigger;
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

	private void Awake()
	{
		swap_trigger = GetComponentInChildren< TriggerListener >();
	}
#endregion

#region API
    public virtual float SwapPlayerOut()
    {
		var randomness = GameSettings.Instance.swap_point_out_randomness;
		return swap_point_out.position.x + Random.Range( -randomness, randomness );
	}
#endregion

#region Implementation
	protected abstract void SwapPlayerIn( Collider collider );
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var position_in    = swap_point_in.position;
		var position_out   = swap_point_out.position;
		var position_left  = swap_point_out.position - Vector3.left * GameSettings.Instance.swap_point_out_randomness;
		var position_right = swap_point_out.position - Vector3.right * GameSettings.Instance.swap_point_out_randomness;

		Handles.color = Color.blue;
		Handles.DrawDottedLine( position_left, position_right, 5f );
		Handles.DrawDottedLine( position_out, position_out + Vector3.up, 5f );
		Handles.Label( position_out + Vector3.up, "Swap Point Out" );
		Handles.DrawSolidDisc( position_out, Vector3.up, 0.1f );

		Handles.color = Color.yellow;
		Handles.DrawSolidDisc( position_left, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( position_right, Vector3.up, 0.1f );

		Handles.color = Color.green;
		Handles.DrawSolidDisc( position_in, Vector3.up, 0.1f );
		Handles.DrawDottedLine( position_in, position_in + Vector3.up, 5f );
		Handles.Label( position_in + Vector3.up, "Swap Point In" );
	}
#endif
#endregion
}
