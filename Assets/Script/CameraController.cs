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
	[BoxGroup( "Event Listeners" ), SerializeField] private EventListenerDelegateResponse level_revealed_listener;
	[BoxGroup( "Event Listeners" ), SerializeField] private EventListenerDelegateResponse player_lane_swap_listener;
	[BoxGroup( "Fired Events" ), SerializeField] private GameEvent level_start_event;

	[BoxGroup( "Setup" ), SerializeField] private SharedReferenceNotifier player_reference;

	private Transform player_transform;
	private Vector3 player_offset;
	private Vector3 player_trigger_offset;

	private float camera_height;

	private UnityMessage updateMethod;
	private Tween rotationTween;
	#endregion

	#region Properties
	#endregion

	#region Unity API
	private void OnEnable()
	{
		level_revealed_listener.OnEnable();
		player_lane_swap_listener.OnEnable();
	}

	private void OnDisable()
	{
		level_revealed_listener.OnDisable();
		player_lane_swap_listener.OnDisable();
	}

	private void Awake()
	{
		level_revealed_listener.response = LevelRevealedResponse;
		player_lane_swap_listener.response = LaneSwapResponse;
		// player_lane_swap_listener.response = ExtensionMethods.EmptyMethod;

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

	private void LaneSwapResponse()
	{
		var swap_event = player_lane_swap_listener.gameEvent as TriggerLaneEvent;

		if( swap_event.swap == SwapType.In )
		{
			if( swap_event.lane == LaneType.Money )
				SetTriggerOffSet( -1 );
			else
				SetTriggerOffSet( 1 );

			updateMethod = OnUpdate_FollowPlayer_TriggerLane;
		}
		else
		{
			rotationTween = rotationTween.KillProper();
			rotationTween = transform.DORotate( Vector3.right * GameSettings.Instance.camera_follow_rotation, GameSettings.Instance.camera_follow_duration );
			updateMethod = OnUpdate_FollowPlayer;

		}
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

		target_position.x = Mathf.Lerp( transform.position.x, player_position.x, Time.deltaTime * GameSettings.Instance.camera_follow_speed );
		target_position.z = Mathf.Lerp( transform.position.z, target_position.z, Time.deltaTime * GameSettings.Instance.camera_follow_speed_depth );
		transform.position = target_position;
	}

	private void OnUpdate_FollowPlayer_TriggerLane()
	{
		var player_position = player_transform.position;
		var target_position = player_transform.position + player_trigger_offset;

		target_position.x = Mathf.Lerp( transform.position.x, target_position.x, Time.deltaTime * GameSettings.Instance.camera_follow_speed );
		target_position.z = Mathf.Lerp( transform.position.z, target_position.z, Time.deltaTime * GameSettings.Instance.camera_follow_speed_depth );

		// transform.position = Vector3.MoveTowards( transform.position, target_position, Time.deltaTime * GameSettings.Instance.camera_follow_speed );
		transform.position = target_position;
	}

	private void SetTriggerOffSet( int cofactor )
	{
		rotationTween = rotationTween.KillProper();

		player_trigger_offset.x = GameSettings.Instance.camera_trigger_follow_lateral * cofactor;
		player_trigger_offset.y = GameSettings.Instance.camera_follow_height;
		player_trigger_offset.z = -GameSettings.Instance.camera_trigger_follow_depth;

		var target_rotation = new Vector3( GameSettings.Instance.camera_follow_rotation, -1f * GameSettings.Instance.camera_trigger_follow_rotation * cofactor, 0 );

		rotationTween = transform.DORotate( target_rotation, GameSettings.Instance.camera_follow_duration );
	}
	#endregion

	#region Editor Only
#if UNITY_EDITOR
#endif
	#endregion
}