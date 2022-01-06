/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "pool_money", menuName = "FF/Data/Pool/Money" ) ]
public class Stackable_Money_Pool : ComponentPool< Stackable_Money >
{
	private Transform initial_parent;

	public Transform InitialParent => initial_parent;

	public void InitPool( Transform parent )
	{
		initial_parent = parent;
		InitPool();
	}

	protected override Stackable_Money InitEntity()
	{
		var entity = base.InitEntity();
		entity.gameObject.SetActive( false );
		entity.transform.SetParent( initial_parent );

		return entity;
	}

	public override void ReturnEntity( Stackable_Money entity )
	{
		stack.Push( entity );
		entity.gameObject.SetActive( false );
		entity.transform.SetParent( initial_parent );
	}
}
