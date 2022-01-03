/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Collectable_Money : Interactable
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
#endregion

#region API
#endregion

#region Implementation
    protected override void OnTrigger( Collider collider )
    {
        var player = collider.GetComponent< TriggerListener >().AttachedComponent as Player;

		var currency = GameSettings.Instance.currency_level_dolar;

		player.GainMoney( Random.Range( currency[ money_count - 1 ], currency[ money_count ] ) );

		money_particle.Raise( "money", transform.position );

		gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}