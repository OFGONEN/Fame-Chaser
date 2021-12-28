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
    [ BoxGroup( "Setup" ), HideInInspector ] public int daddy_money_count;

	// Private \\
	private Rigidbody[] ragdoll_rigidbodies;
	private TriggerListener triggerListener;
	private Transform transform_spawn;
	private Transform transform_start;
    private Transform transform_end;

	// Delegates
	private UnityMessage updateMethod;
	private UnityMessage couple_DetachedMethod;
	private Sequence couple_sequence;
	private Sequence money_sequence;
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
		couple_DetachedMethod = ExtensionMethods.EmptyMethod;

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
		transform_spawn = daddy_spawn_position.SharedValue as Transform;
		transform_start = daddy_start_position.SharedValue as Transform;
		transform_end   = daddy_end_position.SharedValue as Transform;

		ToggleRagdoll( false );
		gameObject.SetActive( true );
		triggerListener.AttachedCollider.enabled = true;

		var spawn_position = transform_spawn.position;

		// Reference spawn position is ahead of start position
		if( transform_spawn.InverseTransformPoint( transform_start.position ).z < 0 )
			spawn_position.z = transform_start.position.z;


		transform.position = spawn_position;
		transform.forward  = ( transform_end.position - spawn_position ).normalized;

		updateMethod = OnUpdate_Movement;
	}

	public void RagdollOff()
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		triggerListener.AttachedCollider.enabled = false;
		ToggleRagdoll( true );

		//TODO cache this Tween ?
		DOVirtual.DelayedCall( GameSettings.Instance.daddy_ragdoll_duration, ReturnToPool );
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
		foreach( var rb in ragdoll_rigidbodies )
		{
			rb.useGravity  = active;
			rb.isKinematic = !active;
		}
	}

	private void OnTrigger( Collider collider )
	{
		daddy_set.RemoveDictionary( GetInstanceID() );

		foreach( var daddy in daddy_set.itemDictionary.Values )
		{
			daddy.RagdollOff();
		}

		updateMethod = ExtensionMethods.EmptyMethod;
		couple_DetachedMethod = OnCoupleDetached_Coupling;

		var player          = collider.GetComponent< TriggerListener >().AttachedComponent as Player;
		var couple_position = player.MatchDaddy( this );

		transform.SetParent( player.transform );

		couple_sequence = DOTween.Sequence();
		couple_sequence.Append( transform.DOLocalMove( couple_position.localPosition, GameSettings.Instance.daddy_couple_duration ) );
		couple_sequence.Join( transform.DOLocalRotate( couple_position.localEulerAngles, GameSettings.Instance.daddy_couple_duration / 2f ) );
		couple_sequence.OnComplete( OnCoupleComplete );
	}

	private void ReturnToPool()
	{
		gameObject.SetActive( false );
		daddy_pool.ReturnEntity( this );
	}

	private void OnCoupleComplete()
	{
		couple_sequence = couple_sequence.KillProper();

		var money_count = daddy_money_count / GameSettings.Instance.daddy_money_bill;

		money_sequence = DOTween.Sequence();

		for( var i = 0; i < money_count; i++ )
		{
			var index = i;
			money_sequence.AppendCallback( () => SpawnMoney( index, GameSettings.Instance.daddy_money_bill ) );
			money_sequence.AppendInterval( GameSettings.Instance.daddy_money_delay );
		}

		var money_remainder = daddy_money_count % GameSettings.Instance.daddy_money_bill;

		if( money_remainder > 0 )
		{
			money_sequence.AppendCallback( () => SpawnMoney( money_count, money_remainder ) );
			money_sequence.AppendInterval( GameSettings.Instance.daddy_money_delay );
		}
	}

	private void OnCoupleDetached_Coupling()
	{
		couple_sequence = couple_sequence.KillProper();
		transform.SetParent( daddy_pool.InitialParent );

		couple_DetachedMethod = ExtensionMethods.EmptyMethod;

		RagdollOff();
	}

	private void SpawnMoney( int index, int moneyCount )
	{
		var money = money_pool.GetEntity();
		money.transform.SetParent( transform.parent );
		money.transform.position = daddy_money_position.position + index * GameSettings.Instance.daddy_money_height * Vector3.up;
		money.transform.rotation = daddy_money_position.rotation;
		money.gameObject.SetActive( true );

		money.money_count = moneyCount;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
