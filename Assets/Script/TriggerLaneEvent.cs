/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "lane_trigger_event", menuName = "FF/Event/TriggerLane" ) ]
public class TriggerLaneEvent : GameEvent
{
	public LaneType lane;
	public SwapType swap;

    public void Raise( LaneType lane, SwapType swap )
    {
		this.lane = lane;
		this.swap = swap;

		Raise();
	}
}