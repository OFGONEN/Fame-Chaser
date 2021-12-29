/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

public class ScrollTexture : MonoBehaviour
{
#region Fields
    [ SerializeField ] private Material[] materials;
#endregion

#region Properties
#endregion

#region Unity API
    private void Update()
    {
        foreach( var material in materials )
    		material.SetTextureOffset( "_MainTex", Vector2.down * Time.time * GameSettings.Instance.texture_arrow_scrollSpeed );
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