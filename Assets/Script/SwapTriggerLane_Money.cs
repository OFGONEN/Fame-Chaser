/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class SwapTriggerLane_Money : SwapTriggerLane
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
	protected override void SwapPlayerIn( Collider collider )
    {
        var player = collider.GetComponent< TriggerListener >().AttachedComponent as Player;
		player.SwapLane_Money( swap_point_in.position );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
