/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class UIParticle : UIImage
{
#region Fields
	public UIParticle_Pool ui_pool_particle;
    // Private \\
    private Sequence particle_sequence;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void Spawn( Vector2 position, Vector2 targetPosition )
    {
		gameObject.SetActive( true );
		uiTransform.position = position;

		var ratio = GameSettings.Instance.ui_particle_spawn_radius / ( float )GameSettings.Instance.ui_screen_width;
		var radius = ratio * Screen.width;

		var random_position = Random.insideUnitCircle * radius;

		var initial_target = position + random_position;
		initial_target.x = Mathf.Clamp( initial_target.x, 0, Screen.width );


		particle_sequence = DOTween.Sequence();
		particle_sequence.Append( transform.DOMove( initial_target, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );
		particle_sequence.Append( transform.DOMoveY( targetPosition.y, GameSettings.Instance.ui_Entity_Move_TweenDuration ) );
		particle_sequence.Join( transform.DOMoveX( targetPosition.x, GameSettings.Instance.ui_Entity_Move_TweenDuration ).SetEase( GiveRandomEase() ) );
		particle_sequence.OnComplete( OnParticle_Complete );
	}
#endregion

#region Implementation
    private AnimationCurve GiveRandomEase()
    {
		var random = Random.Range( 0, 2 );

		if( random == 1 )
			return GameSettings.Instance.ui_particle_ease_horizontal_positive;
		else
			return GameSettings.Instance.ui_particle_ease_horizontal_negative;
	}

	private void OnParticle_Complete()
	{
		particle_sequence = particle_sequence.KillProper();
		ui_pool_particle.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
