/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FFStudio;
using TMPro;
using NaughtyAttributes;

public class PriceTag : MonoBehaviour
{
#region Fields
    public string font_name_dolar;
    public string font_name_star;
	public SharedIntNotifier player_money_notifier;

	private int cost;

	// Components
	private Outline outline;
	private TextMeshProUGUI text_renderer;
    private StringBuilder text_builder;
#endregion

#region Properties
#endregion

#region Unity API
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
        text_renderer = GetComponentInChildren< TextMeshProUGUI >();
        outline       = GetComponentInChildren< Outline >();
		text_builder = new StringBuilder( 32 );

		outline.OutlineWidth = GameSettings.Instance.priceTag_outline_widht;
	}
#endregion

#region API
    public void SetText( int dolar, int real_dolar, int start )
    {
		cost = real_dolar;

		// <font="fontAssetName">
		text_builder.Append( "<color=#FF3737>" + real_dolar + "</color>" );
		text_builder.Append( "<font=\"" );
		text_builder.Append( font_name_dolar );
		text_builder.Append( "\">" );
		text_builder.Append( '$' );

		// for( var i = 0; i < dolar; i++ )
        // {
		// 	text_builder.Append( '$' );
		// }


		// text_builder.Append( "</font>" );

		// text_builder.Append( "<color=\"green\"> +</color>" );

		// text_builder.Append( "<font=\"" );
		// text_builder.Append( font_name_star );
		// text_builder.Append( "\">" );

		// for( var i = 0; i < start; i++ )
        // {
		// 	text_builder.Append( '\\' );
		// }

		// text_builder.Append( "</font>" );

		text_renderer.text = text_builder.ToString();

		OnPlayerMoneyChange();
	}
#endregion

#region Implementation
	private void OnPlayerMoneyChange()
	{
		if( cost > player_money_notifier.SharedValue )
			outline.OutlineColor = GameSettings.Instance.priceTag_color_negative;
		else
			outline.OutlineColor = GameSettings.Instance.priceTag_color_positive;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    public void Test()
    {
		SetText( 1, 1, 1 );
	}
#endif
#endregion
}
