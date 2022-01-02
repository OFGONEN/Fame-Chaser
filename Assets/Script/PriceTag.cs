/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class PriceTag : MonoBehaviour
{
#region Fields
    public string font_name_dolar;
    public string font_name_star;

    private TextMeshProUGUI text_renderer;
    private StringBuilder text_builder;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
        text_renderer = GetComponentInChildren< TextMeshProUGUI >();
		text_builder = new StringBuilder( 32 );
	}
#endregion

#region API
    public void SetText( int dolar, int start )
    {
		// <font="fontAssetName">
		text_builder.Append( "<font=\"" );
		text_builder.Append( font_name_dolar );
		text_builder.Append( "\">" );

		for( var i = 0; i < dolar; i++ )
        {
			text_builder.Append( '$' );
		}

		text_builder.Append( "</font>" );

		text_builder.Append( '/' );

		text_builder.Append( "<font=\"" );
		text_builder.Append( font_name_star );
		text_builder.Append( "\">" );

		for( var i = 0; i < start; i++ )
        {
			text_builder.Append( '\\' );
		}

		text_builder.Append( "</font>" );

		text_renderer.text = text_builder.ToString();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    public void Test()
    {
		SetText( 1, 1 );
	}
#endif
#endregion
}
