/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using TMPro;
using NaughtyAttributes;

public class Player : MonoBehaviour
{
#region Fields
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse level_start_listener;
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse swipe_left_listener;
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse swipe_right_listener;
	[ BoxGroup( "Event Listeners" ), SerializeField ] private EventListenerDelegateResponse level_finished_listener;

	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedFloat input_horizontal;
	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedFloatNotifier level_progress;
	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier camera_reference;
	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier level_progress_indicator_reference;
	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedIntNotifier player_money_notifier;
	[ BoxGroup( "Shared Variables" ), SerializeField ] private SharedIntNotifier player_fame_notifier;

	[ BoxGroup( "Fired Events" ), SerializeField ] private GameEvent level_complete_event; 
	[ BoxGroup( "Fired Events" ), SerializeField ] private GameEvent level_failed_event; 
	[ BoxGroup( "Fired Events" ), SerializeField ] private UIParticle_Event ui_particle_event; 
	[ BoxGroup( "Fired Events" ), SerializeField ] private ParticleSpawnEvent cloth_event; 
	[ BoxGroup( "Fired Events" ), SerializeField ] private TriggerLaneEvent lane_swap_event; 

	[ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer[] cloth_renderers; // Hat, Shirt, Skirt, Shoe
	[ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer cloth_reference_renderer; 
	[ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer cloth_skirt_renderer; 
	[ BoxGroup( "Setup" ), SerializeField ] private TextMeshProUGUI frame_fame_text; 
	[ BoxGroup( "Setup" ), SerializeField ] private Transform frame; 
	[ BoxGroup( "Setup" ), SerializeField ] private Transform couple_position; 
	[ BoxGroup( "Setup" ), SerializeField ] private Transform money_position; 

	// Private \\
	[ SerializeField, ReadOnly ] private Daddy current_daddy;
	[ SerializeField, ReadOnly ] private ClothData[] cloth_data_array;
	[ SerializeField, ReadOnly ] private int money_count;
	[ SerializeField, ReadOnly ] private int fame_count;


	private Animator animator;
	private TriggerListener triggerListener;
	private Transform level_progress_indicator;
	private Camera main_camera;

	private SwapTriggerLane swapTriggerLane;
    private Sequence triggerLane_Sequence;
	private Tween takeClothOff_Tween;

	private UnityMessage swapLane_Out;
	private UnityMessage updateMethod;
	private UnityMessage forceMainLaneMethod;
#endregion

#region Properties
	public Transform MoneyPosition => money_position;
	public UnityMessage ForceMainLaneMethod => forceMainLaneMethod;
	
#endregion

#region Unity API
	private void OnEnable()
	{
		level_start_listener.OnEnable();
		swipe_left_listener.OnEnable();
		swipe_right_listener.OnEnable();
		level_finished_listener.OnEnable();
	}

	private void OnDisable()
	{
		level_start_listener.OnDisable();
		swipe_left_listener.OnDisable();
		swipe_right_listener.OnDisable();
		level_finished_listener.OnDisable();
	}

    private void Awake()
    {
		level_start_listener.response    = LevelStartResponse;
		swipe_left_listener.response     = ExtensionMethods.EmptyMethod;
		swipe_right_listener.response    = ExtensionMethods.EmptyMethod;
		level_finished_listener.response = LevelFinishedResponse;

		cloth_data_array = new ClothData[ cloth_renderers.Length ];

		updateMethod        = ExtensionMethods.EmptyMethod;
		swapLane_Out        = ExtensionMethods.EmptyMethod;
		forceMainLaneMethod = ExtensionMethods.EmptyMethod;

		triggerListener = GetComponentInChildren< TriggerListener >();
		animator        = GetComponentInChildren< Animator >();
    }

	private void Start()
	{
		level_progress_indicator = level_progress_indicator_reference.SharedValue as Transform;
		main_camera         	 = ( camera_reference.SharedValue as Transform ).GetComponent< Camera >();
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

		updateMethod        = OnUpdate_Movement_ChoiceLane;
		swapLane_Out        = SwapLane_Out_Money;
		forceMainLaneMethod = SwapLane_Main;

		lane_swap_event.Raise( LaneType.Money, SwapType.In );

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Money_Complete );
	}

    public void SwapLane_Fame( SwapTriggerLane triggerLane, Vector3 position )
    {
		triggerListener.AttachedCollider.enabled = false;
		swapTriggerLane = triggerLane;

		updateMethod        = OnUpdate_Movement_ChoiceLane;
		swapLane_Out        = SwapLane_Out_Fame;
		forceMainLaneMethod = SwapLane_Main;

		lane_swap_event.Raise( LaneType.Fame, SwapType.In );

		triggerLane_Sequence = DOTween.Sequence();
		triggerLane_Sequence.Append( transform.DOMoveX( position.x, GameSettings.Instance.swap_point_in_duration ) );
		triggerLane_Sequence.OnComplete( OnSwapTriggerLane_In_Fame_Complete );
	}

	public void GainMoney( int amount )
	{
		money_count += amount;

		player_money_notifier.SharedValue = money_count;
	}

	public bool SpendMoney( int amount )
	{
		if( amount > money_count )
		{
			var position = cloth_renderers[ 1 ].transform.position + Vector3.back * GameSettings.Instance.particle_cloth_distance;
			position += Random.insideUnitCircle.ConvertV3() * GameSettings.Instance.particle_cloth_radius;

			cloth_event.Raise( "angry", position, transform );

			return false;
		}
		else
		{
			money_count -= amount;
			player_money_notifier.SharedValue = money_count;
			return true;
		}
	}

	public void DressCloth( ClothData data, bool disable_skirt )
	{
		var index = data.cloth_type.cloth_index;
		cloth_data_array[ index ] = data;

		var renderer = cloth_renderers[ index ];

		renderer.sharedMaterials[ 0 ] = data.cloth_renderer.sharedMaterials[ 0 ];
		renderer.localBounds     = data.cloth_renderer.localBounds;
		renderer.sharedMesh      = data.cloth_renderer.sharedMesh;
		renderer.rootBone        = cloth_reference_renderer.rootBone;
		renderer.bones           = cloth_reference_renderer.bones;

		if( disable_skirt )
			cloth_skirt_renderer.enabled = false;
		else if( index == 1 ) // Shirt index
			cloth_skirt_renderer.enabled = true;

		var random = Random.Range( 1, 4 );

		var position = renderer.transform.position + Vector3.back * GameSettings.Instance.particle_cloth_distance;
		position += Random.insideUnitCircle.ConvertV3() * GameSettings.Instance.particle_cloth_radius;

		cloth_event.Raise( "cloth_" + random, position , transform );
	}

	public bool TakeClothesOff( ClothEnum[] clothesToRemove )
	{
		animator.SetTrigger( "cloth_all" );

		bool takeOff = false;

		for( var i = 0; i < clothesToRemove.Length; i++ )
		{
			var cloth_index = clothesToRemove[ i ].cloth_index;

			if( cloth_data_array[ cloth_index  ].cloth_type != null )
			{
				TakeClothOff( cloth_index );
				takeOff = true;
			}
		}

		return takeOff;
	}

	public bool MatchDaddy( Daddy daddy, ref Transform coupleTransform )
	{
		if( current_daddy != null ) return false;

		animator.SetBool( "walk", false );

		current_daddy 	= daddy;
		coupleTransform = couple_position;
		return true;
	}

	public void OnDaddyMoneyDeplete()
	{
		SwapLane_Main();
	}
#endregion

#region Implementation
	private void LevelStartResponse()
	{
		updateMethod = OnUpdate_Movement_MainLane;
		animator.SetBool( "walk", true );
	}

	private void LevelFinishedResponse()
	{
		updateMethod = ExtensionMethods.EmptyMethod;
		animator.SetTrigger( "end" );

		var direction = ( main_camera.transform.position - transform.position ).SetY( 0 );

		var frame_position = frame.localPosition;

		var sequence = DOTween.Sequence();
		sequence.Append( transform.DORotate( Quaternion.LookRotation( direction ).eulerAngles, GameSettings.Instance.player_duration_rotation ) );
		sequence.AppendCallback( RepositionFrame );
		sequence.Append( frame.DOLocalMove( frame_position, GameSettings.Instance.camera_follow_duration ) );

		if( fame_count <= 0 )
			sequence.OnComplete( level_failed_event.Raise );
		else
			sequence.OnComplete( SpawnFameParticle );
	}

	private void RepositionFrame()
	{
		frame.position = main_camera.transform.position;
		frame.gameObject.SetActive( true );
	}

	private void SpawnFameParticle()
	{
		var position_end = main_camera.WorldToScreenPoint( frame_fame_text.transform.position );

		var position_start = level_progress_indicator.position;
		int particle_count = Mathf.CeilToInt( fame_count / GameSettings.Instance.ui_particle_fame_count_final );

		float delay = 0;

		for( var i = 0; i < particle_count; i++ )
		{
			delay = i * GameSettings.Instance.ui_particle_delay.GiveRandom();

			ui_particle_event.Raise( GameSettings.Instance.ui_particle_fame_sprite, position_start, position_end,
			delay, OnFameParticleDone );
		}

		DOVirtual.DelayedCall( delay + GameSettings.Instance.ui_Entity_Move_TweenDuration * 2f, level_complete_event.Raise );
	}

	private void OnFameParticleDone()
	{
		player_fame_notifier.SharedValue += ( int )GameSettings.Instance.ui_particle_fame_count;
	}

    private void SwapLane_Main()
    {
		takeClothOff_Tween = takeClothOff_Tween.KillProper();

		swapLane_Out();

		swapLane_Out        = ExtensionMethods.EmptyMethod;
		forceMainLaneMethod = ExtensionMethods.EmptyMethod;

		animator.SetBool( "walk", true );

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
		triggerLane_Sequence = null;

		swipe_right_listener.response = SwapLane_Main;
	}

    private void OnSwapTriggerLane_In_Fame_Complete()
    {
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = null;

		swipe_left_listener.response = SwapLane_Main;

		animator.SetBool( "cloth", true );

		takeClothOff_Tween = DOVirtual.DelayedCall( GameSettings.Instance.player_duration_cloth_off, () => Delayed_TakeClothOff( 0 ) );
	}

    private void OnSwapTriggerLane_Out_Complete()
	{
		triggerListener.AttachedCollider.enabled = true;
		triggerLane_Sequence = null;

		updateMethod = OnUpdate_Movement_MainLane;
	}

    private void SwapLane_Out_Fame()
    {
		animator.SetBool( "cloth", false );
		lane_swap_event.Raise( LaneType.Fame, SwapType.Out );
	}

    private void SwapLane_Out_Money()
    {
		lane_swap_event.Raise( LaneType.Money, SwapType.Out );

		if( current_daddy == null ) return;

		current_daddy.CoupleDeatch();
		current_daddy = null;
	}

	private void OnUpdate_Movement_MainLane()
	{
		var position = transform.position;

		var lerp = Mathf.Lerp( position.x, position.x + 1 * input_horizontal.sharedValue.Sign(), Time.deltaTime * GameSettings.Instance.player_movement_speed_lateral * Mathf.Abs( input_horizontal.sharedValue ) );
		position.x = Mathf.Clamp( lerp, -GameSettings.Instance.player_movement_clamp_lateral, GameSettings.Instance.player_movement_clamp_lateral );
		position.z = Mathf.Lerp( position.z, position.z + 1, Time.deltaTime * GameSettings.Instance.player_movement_speed_forward );

		transform.position = position;
	}

	private void OnUpdate_Movement_ChoiceLane()
	{
		var position = transform.position;
		position.z 	 = Mathf.Lerp( position.z, position.z + 1, Time.deltaTime * GameSettings.Instance.player_movement_speed_forward );

		transform.position = position;
	}

	private void DressCloth( ClothEnum clothEnum )
	{
		var index = clothEnum.cloth_index;

		var renderer = cloth_renderers[ index ];

		if( clothEnum.cloth_index == 1 ) // Shirt
			cloth_skirt_renderer.enabled = true;

		renderer.sharedMaterials[ 0 ] = clothEnum.default_cloth.sharedMaterials[ 0 ];
		renderer.localBounds     = clothEnum.default_cloth.localBounds;
		renderer.sharedMesh      = clothEnum.default_cloth.sharedMesh;
		renderer.rootBone        = cloth_reference_renderer.rootBone;
		renderer.bones           = cloth_reference_renderer.bones;
	}

	//! Does not play animation
	private void TakeClothOff( int index )
	{
		var data     = cloth_data_array[ index ];
		var renderer = cloth_renderers[ index ];

		var currency = GameSettings.Instance.currency_level_fame;

		var random_fame = Random.Range( currency[ data.cloth_fame - 1 ], currency[ data.cloth_fame ] );

		fame_count += random_fame;
		level_progress.SharedValue = fame_count / CurrentLevelData.Instance.levelData.fame_target_count;

		var position_renderer = cloth_renderers[ index ].transform.position;
		var position_start    = main_camera.WorldToScreenPoint( position_renderer );

		int particle_count = Mathf.CeilToInt( fame_count / GameSettings.Instance.ui_particle_fame_count );
		SpawnFameUIParticle( position_start, particle_count );

		DressCloth( cloth_data_array[ index ].cloth_type );
		cloth_data_array[ index ].Clear();

		cloth_event.Raise( "fame", renderer.bounds.center, transform );
	}

	private void SpawnFameUIParticle( Vector2 position_start, int count )
	{
		var position_end = level_progress_indicator.position;

		for( var i = 0; i < count; i++ )
		{
			ui_particle_event.Raise( GameSettings.Instance.ui_particle_fame_sprite, position_start, position_end );
		}
	}

	private void Delayed_TakeClothOff( int index )
	{
		takeClothOff_Tween = null;

		if( index >= cloth_data_array.Length || !HasAnyCloth() )
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

	private bool HasAnyCloth()
	{
		bool hasCloth = false;

		for( var i = 0; i < cloth_data_array.Length; i++ )
		{
			hasCloth = cloth_data_array[ i ].cloth_type != null;

			if( hasCloth )
				break;
		}

		return hasCloth;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
