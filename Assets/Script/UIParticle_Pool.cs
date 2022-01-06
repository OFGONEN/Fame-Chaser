/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "pool_ui_particle", menuName = "FF/Data/Pool/UIParticle" ) ]
public class UIParticle_Pool : ComponentPool< UIParticle >
{
	private Transform initial_parent;

	public Transform InitialParent => initial_parent;

	public void InitPool( Transform parent )
	{
		initial_parent = parent;
		InitPool();
	}

	protected override UIParticle InitEntity()
	{
		var entity = base.InitEntity();
		entity.gameObject.SetActive( false );
		entity.transform.SetParent( initial_parent );

		return entity;
	}

	public override void ReturnEntity( UIParticle entity )
	{
		stack.Push( entity );
		entity.gameObject.SetActive( false );
		entity.transform.SetParent( initial_parent );
	}
}
