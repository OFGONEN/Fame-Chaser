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
    private int cloth_random_cost;
#endregion

#region Properties
#endregion

#region Unity API
    protected override void Awake()
    {
		base.Awake();

		cloth_price_tag = GetComponentInChildren< PriceTag >();
	}

    private void Start()
    {
         var currency = GameSettings.Instance.currency_level_dolar_cost;
		cloth_random_cost = Random.Range( currency[ cloth_cost - 1 ], currency[ cloth_cost ] );
		cloth_price_tag.SetText( cloth_cost, cloth_random_cost, cloth_fame );
    }
#endregion

#region API
#endregion

#region Implementation
    protected override void OnTrigger( Collider other )
    {
        var player = other.GetComponent< TriggerListener >().AttachedComponent as Player;


		if( !player.SpendMoney( cloth_random_cost ) )
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