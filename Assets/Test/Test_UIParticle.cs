/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Test_UIParticle : MonoBehaviour
{
#region Fields
    // public UIParticle[] particle;
    public UIParticle_Event particle_event;

    public RectTransform spawn_position;
    public RectTransform target_position;
	public Sprite sprite;

	public UIParticle_Pool pool;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void  Test()
    {
		particle_event.Raise( sprite, spawn_position.position, target_position.position );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
