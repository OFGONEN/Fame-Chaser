/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "pool_daddy", menuName = "FF/Data/Pool/Daddy" ) ]
public class DaddyPool : ComponentPool< Daddy >
{
	private Transform initial_parent;

	public Transform InitialParent => initial_parent;

	public void InitPool( Transform parent )
	{
		initial_parent = parent;
		InitPool();
	}

	protected override Daddy InitEntity()
	{
		var entity = base.InitEntity();
		entity.gameObject.SetActive( false );
		entity.transform.SetParent( initial_parent );

		return entity;
	}

	public override void ReturnEntity( Daddy entity )
	{
		stack.Push( entity );
		entity.transform.SetParent( initial_parent );
	}
}
