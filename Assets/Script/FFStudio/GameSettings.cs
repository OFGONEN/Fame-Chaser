/* Created by and for usage of FF Studios (2021). */

using NaughtyAttributes;
using UnityEngine;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Singleton Related
        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion
        
#region Fields
        [ BoxGroup( "Remote Config" ) ] public bool useRemoveConfig_GameSettings;
        [ BoxGroup( "Remote Config" ) ] public bool useRemoveConfig_Components;

        public int maxLevelCount;
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Joy Stick"                                        )] public float ui_Entity_JoyStick_Gap;
        [ Foldout( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ BoxGroup( "SwapTriggerLane" ) ] public float swap_point_out_randomness = 1f;
        [ BoxGroup( "SwapTriggerLane" ) ] public float swap_point_out_duration = 0.75f;
        [ BoxGroup( "SwapTriggerLane" ) ] public float swap_point_in_duration = 0.75f;

        // Player
        [ BoxGroup( "Player" ) ] public float player_movement_speed_lateral = 1f; 
        [ BoxGroup( "Player" ) ] public float player_movement_speed_forward = 1f; 
        [ BoxGroup( "Player" ) ] public float player_duration_cloth_off = 0.75f; 

        [ BoxGroup( "Input" ) ] public float input_horizontal_deadzone = 0.01f; // 828 * 0.01 = 8 pixel
#endregion

#region Implementation
        private static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		private static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}
