/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Door : Interactable
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] public ClothType[] cloth_remove_array;
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
		player.TakeClothesOff( cloth_remove_array );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}