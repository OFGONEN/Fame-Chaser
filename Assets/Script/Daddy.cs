/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class Daddy : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Shared Variables" ), SerializeField ] private DaddyPool daddy_pool;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private DaddySet daddy_set;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_start_position;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_end_position;

	// Private \\
	private Rigidbody[] ragdoll_rigidbodies;
	private TriggerListener triggerListener;
	private Transform transform_start;
    private Transform transform_end;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnEnable()
	{
		triggerListener.Subscribe( OnTrigger );

		daddy_set.AddDictionary( GetInstanceID(), this );
	}

	private void OnDisable()
	{
		triggerListener.Unsubscribe( OnTrigger );

		daddy_set.RemoveDictionary( GetInstanceID() );
	}

	private void Awake()
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		ragdoll_rigidbodies = GetComponentsInChildren< Rigidbody >();
		triggerListener = GetComponentInChildren< TriggerListener >();

		ToggleRagdoll( false );
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

		ToggleRagdoll( false );
		gameObject.SetActive( true );
		triggerListener.AttachedCollider.enabled = true;

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
		daddy_set.RemoveDictionary( GetInstanceID() );
		updateMethod = ExtensionMethods.EmptyMethod;

		triggerListener.AttachedCollider.enabled = false;
		ToggleRagdoll( true );

		DOVirtual.DelayedCall( GameSettings.Instance.daddy_duration_ragdoll, ReturnToPool );
	}

	private void ToggleRagdoll( bool active )
	{
		foreach( var rb in ragdoll_rigidbodies )
		{
			rb.useGravity  = active;
			rb.isKinematic = !active;
		}
	}

	private void OnTrigger( Collider collider )
	{
		daddy_set.RemoveDictionary( GetInstanceID() );
	}

	private void ReturnToPool()
	{
		gameObject.SetActive( false );
		daddy_pool.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
