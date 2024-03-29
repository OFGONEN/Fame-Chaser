/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "ui_particle_event", menuName = "FF/Event/UIParticle" ) ]
public class UIParticle_Event : GameEvent
{
	public Sprite sprite;
	public Vector2 position_start;
	public Vector2 position_end;
	public float start_delay;
	public UnityMessage complete_delegate;

	public void Raise( Sprite sprite, Vector2 start, Vector2 end )
    {
		position_start = start;
		position_end   = end;
		this.sprite    = sprite;

		start_delay = 0;
		complete_delegate = null;

		Raise();
	}

	public void Raise( Sprite sprite, Vector2 start, Vector2 end, float delay, UnityMessage complete )
    {
		position_start = start;
		position_end   = end;
		this.sprite    = sprite;

		start_delay = delay;
		complete_delegate = complete;

		Raise();
	}
}
