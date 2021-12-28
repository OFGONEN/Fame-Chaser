/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class SkinUpdater : MonoBehaviour
{
	public SkinnedMeshRenderer current;
	public SkinnedMeshRenderer target;
	public SkinnedMeshRenderer reference;

	[Button()]
	public void UpdateSkin()
	{
		current.sharedMaterials = target.sharedMaterials;
		current.localBounds = target.localBounds;
		current.sharedMesh = target.sharedMesh;
		current.rootBone = reference.rootBone;
		current.bones = reference.bones;
	}
}