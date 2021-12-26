/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Collectable_Cloth : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private SkinnedMeshRenderer cloth_renderer;
    [ BoxGroup( "Setup" ), SerializeField ] private ClothType cloth_type;
    [ BoxGroup( "Setup" ), SerializeField ] private int cloth_cost;
    [ BoxGroup( "Setup" ), SerializeField ] private int cloth_fame;

    // Private \\
    private TriggerListener cloth_trigger;
#endregion

#region Properties
    private void OnEnable()
    {
		cloth_trigger.Subscribe( OnTrigger );
	}

    private void OnDisable()
    {
		cloth_trigger.Unsubscribe( OnTrigger );
    }

    private void Awake()
    {
        cloth_trigger = GetComponentInChildren< TriggerListener >();
    }
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
    private void OnTrigger( Collider other )
    {
        var player = other.GetComponent< TriggerListener >().AttachedComponent as Player;

        if( !player.SpendMoney( cloth_cost ) )
			return;
		
        player.DressCloth( new ClothData( cloth_renderer, cloth_type, cloth_cost, cloth_fame ) );
        // Player should spawn particle
		gameObject.SetActive( false );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}