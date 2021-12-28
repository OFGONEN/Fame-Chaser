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
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Deposit() 
    {
		gameObject.SetActive( false );
		money_pool.ReturnEntity( this );
	}

    public void DepositWithMoney() 
    {
		money_particle.Raise( "stack_money", transform.position );

		gameObject.SetActive( false );
		money_pool.ReturnEntity( this );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
