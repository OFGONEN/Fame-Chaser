/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public abstract class Interactable : MonoBehaviour
{
#region Fields
    // Private \\
    protected TriggerListener triggerListener;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		triggerListener.Subscribe( OnTrigger );
	}

    private void OnDisable()
    {
		triggerListener.Unsubscribe( OnTrigger );
	}

    private void Awake()
    {
        triggerListener = GetComponentInChildren< TriggerListener >();
    }
#endregion

#region API
#endregion

#region Implementation
    protected abstract void OnTrigger( Collider collider );
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}