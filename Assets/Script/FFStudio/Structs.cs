/* Created by and for usage of FF Studios (2021). */

using System;
using UnityEngine;

namespace FFStudio
{
	[ Serializable ]
	public struct TransformData
	{
		public Vector3 position;
		public Vector3 rotation; // Euler angles.
		public Vector3 scale; // Local scale.
	}

	[ Serializable ]
	public struct ClothData
	{
		public SkinnedMeshRenderer cloth_renderer;
		public ClothType cloth_type;
		public int cloth_cost;
		public int cloth_fame;

		public ClothData( SkinnedMeshRenderer renderer, ClothType type, int cost, int fame )
		{
			cloth_renderer = renderer;
			cloth_type     = type;
			cloth_cost     = cost;
			cloth_fame     = fame;
		}

		public void Clear()
		{
			cloth_renderer = null;
			cloth_type     = ClothType.None;
			cloth_cost     = 0;
			cloth_fame     = 0;
		}
	}
}
