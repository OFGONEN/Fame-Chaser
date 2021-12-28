/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class SwapTriggerLane_Main : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        var player = other.GetComponent< TriggerListener >().AttachedComponent as Player;
		player.ForceMainLaneMethod();
	}
}
