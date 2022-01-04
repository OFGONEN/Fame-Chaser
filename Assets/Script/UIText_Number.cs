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

    private Tween punch_tween; 
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
		punch_tween = punch_tween.KillProper();

		textRenderer.text = player_money_notifier.SharedValue.ToString();

		transform.localScale = startScale;
		punch_tween = transform.DOPunchScale( Vector3.one * GameSettings.Instance.ui_Entity_PunchScale_Strenght, GameSettings.Instance.ui_Entity_PunchScale_Duration );
	}
#endregion
}
