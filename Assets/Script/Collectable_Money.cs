/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Collectable_Money : Interactable
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private SharedReferenceNotifier camera_reference;
    [ BoxGroup( "Setup" ), SerializeField ] private SharedReferenceNotifier ui_money_reference;
    [ BoxGroup( "Setup" ), SerializeField ] private UIParticle_Event ui_particle_event;
    [ BoxGroup( "Setup" ), SerializeField ] private int money_count;
    [ BoxGroup( "Setup" ), SerializeField ] private ParticleSpawnEvent money_particle;

    // Private \\
    private TriggerListener money_trigger;
    private Camera main_camera;
    private Transform ui_money;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
        main_camera = ( camera_reference.SharedValue as Transform ).GetComponent< Camera >();
		ui_money = ui_money_reference.SharedValue as Transform;
	}
#endregion

#region API
#endregion

#region Implementation
    protected override void OnTrigger( Collider collider )
    {
        var player          = collider.GetComponent< TriggerListener >().AttachedComponent as Player;
        var currency        = GameSettings.Instance.currency_level_dolar;
        var random_currency = Random.Range( currency[ money_count - 1 ], currency[ money_count ] );

		player.GainMoney( random_currency );
		gameObject.SetActive( false );

		// ui particle
		var particle_start  = main_camera.WorldToScreenPoint( transform.position );
		var particle_end    = ui_money.position;
		var particle_sprite = GameSettings.Instance.ui_particle_dolar_sprite;
		int particle_count  = Mathf.CeilToInt( random_currency / GameSettings.Instance.ui_particle_dolar_count );

        for( var i = 0; i < particle_count; i++ )
        {
			ui_particle_event.Raise( particle_sprite, particle_start, particle_end );
		}

		money_particle.Raise( "money", transform.position );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}