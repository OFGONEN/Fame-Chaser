/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class UIEntity_Event : UIEntity
{
#region Fields
    public EventListenerDelegateResponse level_start_listener;
    public EventListenerDelegateResponse level_end_listener;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		level_start_listener.OnEnable();
		level_end_listener.OnEnable();
	}

    private void OnDisable()
    {
		level_start_listener.OnDisable();
		level_end_listener.OnDisable();
    }

    private void Awake()
    {
		level_start_listener.response = () => GoToTargetPosition();
		level_end_listener.response = () => GoToStartPosition();
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
