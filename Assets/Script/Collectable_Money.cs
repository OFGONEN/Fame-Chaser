/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Collectable_Money : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private int money_count;
    [ BoxGroup( "Setup" ), SerializeField ] private ParticleSpawnEvent money_particle;

    // Private \\
    private TriggerListener money_trigger;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		money_trigger.Subscribe( OnTrigger );
	}

    private void OnDisable()
    {
		money_trigger.Unsubscribe( OnTrigger );
    }

    private void Awake()
    {
        money_trigger = GetComponentInChildren< TriggerListener >();
    }
#endregion

#region API
#endregion

#region Implementation
    private void OnTrigger( Collider collider )
    {
        var player = collider.GetComponent< TriggerListener >().AttachedComponent as Player;
		player.GainMoney( money_count );

		money_particle.Raise( "money", transform.position );

		gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}