/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Daddy : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared Variables" ), SerializeField ] private DaddyPool daddy_pool;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private DaddySet daddy_set;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_start_position;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_end_position;

    // Private \\
    private Transform transform_start;
    private Transform transform_end;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		updateMethod = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
	[ Button() ]
    public void Spawn()
    {
		transform_start = daddy_start_position.SharedValue as Transform;
		transform_end   = daddy_end_position.SharedValue as Transform;

		gameObject.SetActive( true );

		transform.position = transform_start.position;
		transform.forward  = ( transform_end.position - transform_start.position ).normalized;

		updateMethod = OnUpdate_Movement;
	}
#endregion

#region Implementation
	private void OnUpdate_Movement()
	{
		var position = Vector3.MoveTowards( transform.position, transform_end.position, Time.deltaTime * GameSettings.Instance.daddy_movement_speed );

		if( Vector3.Distance( position, transform_end.position ) <= 0.1f )
			RagdollOff();
		else
			transform.position = position;
	}

	private void RagdollOff()
	{
		updateMethod = ExtensionMethods.EmptyMethod;
		gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
