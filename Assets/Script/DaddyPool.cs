/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "pool_daddy", menuName = "FF/Data/Pool/Daddy" ) ]
public class DaddyPool : ComponentPool< Daddy >
{
	protected override Daddy InitEntity()
	{
		var entity = base.InitEntity();
		entity.gameObject.SetActive( false );

		return entity;
	}
}
