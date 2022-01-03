/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
public class AppAssetHolder : MonoBehaviour
{
#region Fields
	public GameSettings gameSettings;
	public CurrentLevelData currentLevelData;

	public ScriptableObject[] enums;

	public DaddyPool pool_daddy_1;
	public DaddyPool pool_daddy_2;
	public DaddyPool pool_daddy_3;
	public Stackable_Money_Pool pool_money;

	private void Awake()
	{
		pool_daddy_1.InitPool( transform );
		pool_daddy_2.InitPool( transform );
		pool_daddy_3.InitPool( transform );
		pool_money.InitPool( transform );
	}
#endregion
}
