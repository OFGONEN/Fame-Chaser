/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Stackable_Money : MonoBehaviour
{
#region Fields
    [ SerializeField ] private Stackable_Money_Pool money_pool;
    [ SerializeField ] private ParticleSpawnEvent money_particle;
    [ ReadOnly ] public int money_count;

	private UnityMessage depositMethod;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		depositMethod = Disable;
	}
#endregion

#region API
    public void Deposit() 
    {
		depositMethod();
	}

	public void ChangeDepositMethod()
	{
		depositMethod = DisableWithParticle;
	}
#endregion

#region Implementation
    private void DisableWithParticle() 
    {
		depositMethod = Disable;

		money_particle.Raise( "stack_money", transform.position );
		money_pool.ReturnEntity( this );
	}

    private void Disable() 
    {
		money_pool.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
