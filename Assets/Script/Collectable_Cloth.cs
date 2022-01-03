/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Collectable_Cloth : Interactable
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer cloth_renderer;
    [ BoxGroup( "Setup" ), SerializeField ] private ClothEnum cloth_type;
    [ BoxGroup( "Setup" ), SerializeField ] private bool disable_skirt;
    [ BoxGroup( "Setup" ), SerializeField ] private int cloth_cost;
    [ BoxGroup( "Setup" ), SerializeField ] private int cloth_fame;

    // Private \\
    private TriggerListener cloth_trigger;
    private PriceTag cloth_price_tag;
    private Outline cloth_outline;
#endregion

#region Properties
#endregion

#region Unity API
    protected override void Awake()
    {
		base.Awake();

		cloth_price_tag = GetComponentInChildren< PriceTag >();
		cloth_outline   = GetComponentInChildren< Outline >();

		cloth_price_tag.SetText( cloth_cost, cloth_fame );

        if( cloth_outline != null )
        {
		    cloth_outline.OutlineColor = GameSettings.Instance.cloth_outline_color[ cloth_fame - 1 ];
		    cloth_outline.OutlineWidth = GameSettings.Instance.cloth_outline_widht[ cloth_fame - 1 ];
        }
	}

#endregion

#region API
#endregion

#region Implementation
    protected override void OnTrigger( Collider other )
    {
        var player = other.GetComponent< TriggerListener >().AttachedComponent as Player;

        var currency = GameSettings.Instance.currency_level_dolar;

		var random_currency = Random.Range( currency[ cloth_cost - 1 ], currency[ cloth_cost ] );

		if( !player.SpendMoney( random_currency ) )
			return;
		
        player.DressCloth( new ClothData( cloth_renderer, cloth_type, cloth_cost, cloth_fame ), disable_skirt );
        // Player should spawn particle
		gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}