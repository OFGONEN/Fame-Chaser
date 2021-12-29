/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu( fileName = "cloth_", menuName = "FF/Data/Enum/Cloth" ) ]
public class ClothEnum : ScriptableObject
{
	public int cloth_index;

	public SkinnedMeshRenderer default_cloth;
}
