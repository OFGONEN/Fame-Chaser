/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class CameraController : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Event Listeners"), SerializeField ] private EventListenerDelegateResponse level_revealed_listener;
    [ BoxGroup( "Fired Events"), SerializeField ] private GameEvent level_start_event;

    [ BoxGroup( "Setup"), SerializeField ] private SharedReferenceNotifier player_reference;

    private Transform player_transform;
	private Vector3 player_offset;

	private float camera_height;

	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		level_revealed_listener.OnEnable();
	}

    private void OnDisable()
    {
		level_revealed_listener.OnDisable();
    }

    private void Awake()
    {
		level_revealed_listener.response = LevelRevealedResponse;
		updateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelRevealedResponse()
    {
		player_transform = player_reference.SharedValue as Transform;

		var target_position = player_transform.position;
            target_position.y += GameSettings.Instance.camera_follow_height;
            target_position.z -= GameSettings.Instance.camera_follow_depth;

		var target_rotation = Vector3.right * GameSettings.Instance.camera_follow_rotation;

		transform.DOMove( target_position, GameSettings.Instance.camera_follow_duration ).OnComplete( OnCameraTransition );
		transform.DORotate( target_rotation, GameSettings.Instance.camera_follow_duration );
	}

    private void OnCameraTransition()
    {
		player_offset = player_transform.position - transform.position;

		updateMethod = OnUpdate_FollowPlayer;
		level_start_event.Raise();
	}

    private void OnUpdate_FollowPlayer()
    {
		var player_position = player_transform.position;
		var target_position = player_transform.position - player_offset;

		target_position.x  = Mathf.Lerp( transform.position.x, player_position.x, Time.deltaTime * GameSettings.Instance.camera_follow_speed );
		transform.position = target_position;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}