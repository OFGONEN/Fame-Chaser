/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Door : Interactable
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] public ClothEnum[] cloth_remove_array;
    [ BoxGroup( "Setup" ), SerializeField ] public ParticleSpawnEvent cloth_remove_particle;
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
		var player = collider.GetComponentInChildren<TriggerListener>().AttachedComponent as Player;
		var takeOff = player.TakeClothesOff( cloth_remove_array );

		triggerListener.Unsubscribe( OnTrigger );

		if( takeOff )
			cloth_remove_particle.Raise( "door", collider.bounds.center, player.transform );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}