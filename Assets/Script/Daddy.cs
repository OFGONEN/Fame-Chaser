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
    [ BoxGroup( "Shared Variables" ), SerializeField ] private Stackable_Money_Pool money_pool;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_spawn_position;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_start_position;
    [ BoxGroup( "Shared Variables" ), SerializeField ] private SharedReferenceNotifier daddy_end_position;

    [ BoxGroup( "Setup" ) ] public Transform daddy_money_position;
     

	// Private \\
	private Animator animator;
	private Rigidbody[] ragdoll_rigidbodies;
	private TriggerListener triggerListener;
	private Transform transform_spawn;
	private Transform transform_start;
    private Transform transform_end;
    private Transform player_money_position;
	private Player player;

	[ SerializeField, ReadOnly ] private int daddy_money_count;
	[ SerializeField, ReadOnly ] private List< Stackable_Money > daddy_money_list  = new List< Stackable_Money >( 64 );
	[ SerializeField, ReadOnly ] private List< Stackable_Money > player_money_list = new List< Stackable_Money >( 64 );

	// Delegates
	private UnityMessage updateMethod;
	private UnityMessage couple_DetachedMethod;
	private Sequence couple_sequence;
	private Tween ragdoll_tween;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnEnable()
	{
		triggerListener.Subscribe( OnTrigger );
	}

	private void OnDisable()
	{
		triggerListener.Unsubscribe( OnTrigger );

		ragdoll_tween   = ragdoll_tween.KillProper();
		couple_sequence = couple_sequence.KillProper();
	}

	private void Awake()
	{
		updateMethod = ExtensionMethods.EmptyMethod;
		couple_DetachedMethod = ExtensionMethods.EmptyMethod;

		animator            = GetComponentInChildren< Animator >();
		ragdoll_rigidbodies = GetComponentsInChildren< Rigidbody >();
		triggerListener     = GetComponentInChildren< TriggerListener >();

		ToggleRagdoll( false );
	}

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
    public void Spawn( int money )
    {
		transform_spawn = daddy_spawn_position.SharedValue as Transform;
		transform_start = daddy_start_position.SharedValue as Transform;
		transform_end   = daddy_end_position.SharedValue as Transform;

		animator.SetBool( "match", false );

		daddy_money_count = money;

		daddy_money_list.Clear();
		player_money_list.Clear();

		ToggleRagdoll( false );
		gameObject.SetActive( true );
		triggerListener.AttachedCollider.enabled = true;

		var spawn_position = transform_spawn.position;

		// Reference spawn position is ahead of start position
		if( transform_spawn.InverseTransformPoint( transform_start.position ).z < 0 )
			spawn_position.z = transform_start.position.z;


		transform.position = spawn_position;
		transform.forward  = ( transform_end.position.SetX( spawn_position.x ) - spawn_position ).normalized;

		updateMethod = OnUpdate_Movement;
	}

	public void RagdollOff()
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		triggerListener.AttachedCollider.enabled = false;
		ToggleRagdoll( true );

		ragdoll_tween = DOVirtual.DelayedCall( GameSettings.Instance.daddy_ragdoll_duration, ReturnToPool );
	}

	public void CoupleDeatch()
	{
		couple_DetachedMethod();
	}
#endregion

#region Implementation
	private void OnUpdate_Movement()
	{
		var spawn_position = transform_spawn.position;

		var end_position = transform_end.position;
		end_position.x = spawn_position.x;

		var position = Vector3.MoveTowards( transform.position, end_position, Time.deltaTime * GameSettings.Instance.daddy_movement_speed );

		if( Vector3.Distance( position, end_position ) <= 0.1f )
			RagdollOff();
		else
			transform.position = position;
	}

	private void ToggleRagdoll( bool active )
	{
		animator.enabled = !active;

		foreach( var rb in ragdoll_rigidbodies )
		{
			rb.useGravity  = active;
			rb.isKinematic = !active;
		}
	}

	private void OnTrigger( Collider collider )
	{
		triggerListener.AttachedCollider.enabled = false;
		updateMethod = ExtensionMethods.EmptyMethod;
		couple_DetachedMethod = OnCoupleDetached_Coupling;

		Transform couple_position = null;

		player = collider.GetComponent< TriggerListener >().AttachedComponent as Player;
		var canMatch = player.MatchDaddy( this, ref couple_position );

		if( !canMatch )
		{
			RagdollOff();
			ragdoll_rigidbodies[ 0 ].AddForce( 
				( transform.position - collider.transform.position ).normalized.SetY( GameSettings.Instance.daddy_ragdoll_force_vertical.GiveRandom()  )
				 * GameSettings.Instance.daddy_ragdoll_force, 
				 ForceMode.Impulse );
			return;
		}

		player_money_position = player.MoneyPosition;

		transform.SetParent( player.transform );

		couple_sequence = DOTween.Sequence();
		couple_sequence.Append( transform.DOLocalMove( couple_position.localPosition, GameSettings.Instance.daddy_couple_duration ) );
		couple_sequence.Join( transform.DOLocalRotate( couple_position.localEulerAngles, GameSettings.Instance.daddy_couple_duration / 2f ) );
		couple_sequence.OnComplete( OnCoupleComplete );
	}

	private void ReturnToPool()
	{
		ragdoll_tween = ragdoll_tween.KillProper();

		gameObject.SetActive( false );
		daddy_pool.ReturnEntity( this );
	}

	private void OnCoupleComplete()
	{
		couple_sequence = couple_sequence.KillProper();
		couple_sequence = DOTween.Sequence();

		couple_DetachedMethod = OnCoupleDetached_Money;

		animator.SetBool( "match", true );

		SpawnMoneyOnDaddy(); // Edits couple_sequence
		couple_sequence.OnComplete( TransferMoneyToPlayer );
	}

	private void SpawnMoneyOnDaddy()
	{
		var money_count = daddy_money_count / GameSettings.Instance.daddy_money_bill;

		for( var i = 0; i < money_count; i++ )
		{
			var index = i;
			couple_sequence.AppendCallback( () => SpawnMoney( index, GameSettings.Instance.daddy_money_bill ) );
			couple_sequence.AppendInterval( GameSettings.Instance.daddy_money_delay );
		}

		var money_remainder = daddy_money_count % GameSettings.Instance.daddy_money_bill;

		if( money_remainder > 0 )
		{
			couple_sequence.AppendCallback( () => SpawnMoney( money_count, money_remainder ) );
			couple_sequence.AppendInterval( GameSettings.Instance.daddy_money_delay );
		}
	}

	private void TransferMoneyToPlayer()
	{
		couple_sequence = couple_sequence.KillProper();
		couple_sequence = DOTween.Sequence();

		var duration = GameSettings.Instance.daddy_money_transfer;
		var delay    = GameSettings.Instance.daddy_money_delay;
		var height   = GameSettings.Instance.daddy_money_height;

		for( var i = daddy_money_list.Count - 1; i >= 0; i-- )
		{
			AnimationCurve curve;
			var index    = daddy_money_list.Count - i - 1;
			var money    = daddy_money_list[ i ];
			var position = player_money_position.localPosition;
			var rotation = player_money_position.eulerAngles;

			money.ChangeDepositMethod();

			if( index <= daddy_money_list.Count / 2 )
				curve = GameSettings.Instance.curve_downward;
			else
				curve = GameSettings.Instance.curve_upward;

			couple_sequence.AppendCallback( () => player_money_list.Add( money ) );
			couple_sequence.Append( money.transform.DOLocalMoveX( position.x, duration ) );
			couple_sequence.Join( money.transform.DOLocalMoveY( position.y + index * height, duration ).SetEase( curve ) );
			couple_sequence.Join( money.transform.DOLocalMoveZ( position.z, duration ) );
			couple_sequence.Join( money.transform.DORotate( rotation, duration ) );
			couple_sequence.AppendInterval( delay );
		}

		couple_sequence.OnComplete( player.OnDaddyMoneyDeplete );
	}

	private void OnCoupleDetached_Coupling()
	{
		couple_sequence = couple_sequence.KillProper();
		transform.SetParent( daddy_pool.InitialParent );

		couple_DetachedMethod = ExtensionMethods.EmptyMethod;

		RagdollOff();
	}

	private void OnCoupleDetached_Money()
	{
		couple_sequence = couple_sequence.KillProper();
		transform.SetParent( daddy_pool.InitialParent );

		couple_DetachedMethod = ExtensionMethods.EmptyMethod;
		RagdollOff();

		// Handle Money
		int money_amount = 0;

		for( var i = 0; i < player_money_list.Count; i++ )
			money_amount += player_money_list[ i ].money_count;

		for( var i = 0; i < daddy_money_list.Count; i++ )
		{
			daddy_money_list[ i ].Deposit();
		}

		player.GainMoney( money_amount );
	}

	private void SpawnMoney( int index, int moneyCount )
	{
		var money = money_pool.GetEntity();
		money.transform.SetParent( transform.parent );
		money.transform.position = daddy_money_position.position + index * GameSettings.Instance.daddy_money_height * Vector3.up;
		money.transform.rotation = daddy_money_position.rotation;
		money.gameObject.SetActive( true );

		money.money_count = moneyCount;

		daddy_money_list.Add( money );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
