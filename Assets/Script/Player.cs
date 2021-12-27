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
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse level_start_listener;
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse swipe_left_listener;
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse swipe_right_listener;

	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedFloat input_horizontal;

	[ BoxGroup( "Setup" ), SerializeField ] private ParticleSpawnEvent cloth_particle; 
	[ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer[] cloth_renderers; // Hat, Shirt, Skirt, Shoe
	[ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer cloth_reference_renderer; 

	// Private \\
	[ SerializeField, ReadOnly ] private ClothData[] cloth_data_array;
	[ SerializeField, ReadOnly ] private int money_count;
	[ SerializeField, ReadOnly ] private int fame_count;

	private Animator animator;
	private TriggerListener triggerListener;

    private SwapTriggerLane swapTriggerLane;
    private Sequence triggerLane_Sequence;
	private Tween takeClothOff_Tween;

	private UnityMessage swapLane_Out;
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnEnable()
	{
		level_start_listener.OnEnable();
		swipe_left_listener.OnEnable();
		swipe_right_listener.OnEnable();
	}

	private void OnDisable()
	{
		level_start_listener.OnDisable();
		swipe_left_listener.OnDisable();
		swipe_right_listener.OnDisable();
	}

    private void Awake()
    {
		level_start_listener.response = LevelStartResponse;
		swipe_left_listener.response  = ExtensionMethods.EmptyMethod;
		swipe_right_listener.response = ExtensionMethods.EmptyMethod;

		cloth_data_array = new ClothData[ cloth_renderers.Length ];

		updateMethod 	= ExtensionMethods.EmptyMethod;
		swapLane_Out    = ExtensionMethods.EmptyMethod;

		triggerListener = GetComponentInChildren< TriggerListener >();
		animator        = GetComponentInChildren< Animator >();
    }

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
    public void SwapLane_Money( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		updateMethod = ExtensionMethods.EmptyMethod;
		swapLane_Out = SwapLane_Out_Money;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Money_Complete );
	}

    public void SwapLane_Fame( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		updateMethod = ExtensionMethods.EmptyMethod;
		swapLane_Out = SwapLane_Out_Fame;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Fame_Complete );
	}

	public void GainMoney( int amount )
	{
		money_count += amount;
	}

	public bool SpendMoney( int amount )
	{
		if( amount > money_count )
			return false;
		else
		{
			money_count -= amount;
			return true;
		}
	}

	public void DressCloth( ClothData data )
	{
		var index = data.cloth_type.cloth_index;
		cloth_data_array[ index ] = data;

		var renderer = cloth_renderers[ index ];

		renderer.sharedMaterials = data.cloth_renderer.sharedMaterials;
		renderer.localBounds     = data.cloth_renderer.localBounds;
		renderer.sharedMesh      = data.cloth_renderer.sharedMesh;
		renderer.rootBone        = cloth_reference_renderer.rootBone;
		renderer.bones           = cloth_reference_renderer.bones;

		cloth_particle.Raise( "cloth", renderer.bounds.center );
	}

	public void TakeClothesOff( ClothEnum[] clothesToRemove )
	{
		//TODO(ofg): should play take all of your clothes off animation
		for( var i = 0; i < clothesToRemove.Length; i++ )
		{
			var cloth_index = clothesToRemove[ i ].cloth_index;

			if( cloth_data_array[ cloth_index  ].cloth_type != null )
				TakeClothOff( cloth_index );
		}
	}
#endregion

#region Implementation
	private void LevelStartResponse()
	{
		updateMethod = OnUpdate_Movement;
		animator.SetTrigger( "walk" );
	}

    [ Button() ]
    private void SwapLane_Main()
    {
		takeClothOff_Tween = takeClothOff_Tween.KillProper();
		swapLane_Out();

		animator.SetTrigger( "walk" );

		var position_out = swapTriggerLane.SwapPlayerOut();

		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = null;

		swipe_left_listener.response = ExtensionMethods.EmptyMethod;
		swipe_right_listener.response = ExtensionMethods.EmptyMethod;

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position_out, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_Out_Complete );
	}

    private void OnSwapTriggerLane_In_Money_Complete()
    {
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();

		swipe_right_listener.response = SwapLane_Main;
	}

    private void OnSwapTriggerLane_In_Fame_Complete()
    {
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();

		swipe_left_listener.response = SwapLane_Main;

		animator.SetTrigger( "cloth_off" );

		takeClothOff_Tween = DOVirtual.DelayedCall( GameSettings.Instance.player_duration_cloth_off, () => Delayed_TakeClothOff( 0 ) );
	}

    private void OnSwapTriggerLane_Out_Complete()
	{
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = triggerLane_Sequence.KillProper();

		updateMethod = OnUpdate_Movement;
	}

    private void SwapLane_Out_Fame()
    {
	}

    private void SwapLane_Out_Money()
    {

    }

	private void OnUpdate_Movement()
	{
		transform.position = Vector3.MoveTowards( transform.position,
		transform.position + Vector3.right * input_horizontal.sharedValue.Sign(),
		Time.deltaTime * GameSettings.Instance.player_movement_speed * Mathf.Abs( input_horizontal.sharedValue ) );
	}

	//! Does not play animation
	private void TakeClothOff( int index )
	{
		var data     = cloth_data_array[ index ];
		var renderer = cloth_renderers[ index ];

		fame_count += data.cloth_fame;

		cloth_renderers[ index ].sharedMesh = null;
		cloth_data_array[ index ].Clear();

		cloth_particle.Raise( "fame", renderer.bounds.center );
	}

	private void Delayed_TakeClothOff( int index )
	{
		if( index >= cloth_data_array.Length )
		{
			SwapLane_Main();
			return;
		}

		for( var i = index; i < cloth_data_array.Length; i++ )
		{
			if( cloth_data_array[ i ].cloth_type != null )
			{
				index = i;
				break;
			}
		}

		TakeClothOff( index );
		takeClothOff_Tween = DOVirtual.DelayedCall( GameSettings.Instance.player_duration_cloth_off, () => Delayed_TakeClothOff( index + 1 ) );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
