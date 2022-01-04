/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class UIText_Number : UIText
{
#region Fields
    [ BoxGroup( "Setup" ) ] public SharedIntNotifier player_money_notifier;

    private Sequence punch_sequence; 
	private int lastValue;
#endregion

#region UnityAPI
    private void OnEnable()
    {
		player_money_notifier.Subscribe( OnPlayerMoneyChange );
	}

    private void OnDisable()
    {
		player_money_notifier.Unsubscribe( OnPlayerMoneyChange );
    }

    private void Awake()
    {
		textRenderer.text = player_money_notifier.SharedValue.ToString();
    }
#endregion

#region Implementation
    private void OnPlayerMoneyChange()
    {
		punch_sequence = punch_sequence.KillProper();

		Color change;
		if( lastValue > player_money_notifier.SharedValue )
			change = Color.red;
		else
			change = Color.green;

		punch_sequence = DOTween.Sequence();

		lastValue            = player_money_notifier.SharedValue;
		textRenderer.text    = player_money_notifier.SharedValue.ToString();
		transform.localScale = startScale;

		var duration = GameSettings.Instance.ui_Entity_PunchScale_Duration;

		punch_sequence.Append( textRenderer.DOColor( change, duration / 2f ) );
		punch_sequence.Join( transform.DOPunchScale( Vector3.one * GameSettings.Instance.ui_Entity_PunchScale_Strenght, GameSettings.Instance.ui_Entity_PunchScale_Duration ) );
		punch_sequence.AppendInterval( duration / 2f );
		punch_sequence.Join( textRenderer.DOColor( Color.white, duration / 2f ) );

	}
#endregion
}
