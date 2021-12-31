/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Paparazzi : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Event Listeners" ) ] public EventListenerDelegateResponse level_start_listener;
    [ BoxGroup( "Event Listeners" ) ] public EventListenerDelegateResponse level_finished_listener;

    [ BoxGroup( "Shared Variables" ) ] public SharedReferenceNotifier player_reference;

	// Private \\
    private Transform player_transform;
	private Vector3 position_start;

	// Components
	private Animator animator;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		level_start_listener.OnEnable();
		level_finished_listener.OnEnable();
	}

    private void OnDisable()
    {
		level_start_listener.OnDisable();
		level_finished_listener.OnDisable();
    }

    private void Awake()
    {
		level_start_listener.response = LevelStartResponse;
		level_finished_listener.response = LevelFinishedResponse;

		animator = GetComponentInChildren< Animator >();

		updateMethod   = ExtensionMethods.EmptyMethod;
		position_start = transform.position;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelStartResponse()
    {
		player_transform = player_reference.SharedValue as Transform;

		animator.SetBool( "walk", true );

		updateMethod = OnUpdate_Movement;
	}

	private void LevelFinishedResponse()
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		animator.SetBool( "walk", false );
		animator.SetBool( "match", false );
	}

	private void OnUpdate_Movement()
    {
		var position = transform.position;
		transform.position = Vector3.Lerp( position, position + Vector3.forward, Time.deltaTime * GameSettings.Instance.player_movement_speed_forward );
	}

	private void OnUpdate_Photo()
    {
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}