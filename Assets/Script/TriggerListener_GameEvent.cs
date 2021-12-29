/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class TriggerListener_GameEvent : MonoBehaviour
{
	public ReferenceGameEvent gameEvent;

	private void OnTriggerEnter( Collider other )
    {
		gameEvent.eventValue = other;
		gameEvent.Raise();
	}
}
