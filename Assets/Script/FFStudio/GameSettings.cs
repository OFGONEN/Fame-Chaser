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
        [ BoxGroup( "Player" ) ] public float player_movement_clamp_lateral = 5f; 
        [ BoxGroup( "Player" ) ] public float player_duration_cloth_off = 0.75f; 

        // Sugar Daddy
        [ BoxGroup( "Daddy" ) ] public float daddy_movement_speed   = 5f;
        [ BoxGroup( "Daddy" ) ] public float daddy_couple_duration  = 0.75f;
        [ BoxGroup( "Daddy" ) ] public float daddy_ragdoll_duration = 1f;
        [ BoxGroup( "Daddy" ) ] public float daddy_ragdoll_force    = 50f;
        [ BoxGroup( "Daddy" ) ] public Vector2 daddy_ragdoll_force_vertical;
        [ BoxGroup( "Daddy" ) ] public int daddy_money_bill         = 10;
        [ BoxGroup( "Daddy" ) ] public float daddy_money_delay      = 0.125f;
        [ BoxGroup( "Daddy" ) ] public float daddy_money_transfer   = 0.2f;
        [ BoxGroup( "Daddy" ) ] public float daddy_money_height     = 0.2f;
        [ BoxGroup( "Daddy" ) ] public Vector2Int daddy_money_amount;

        [ BoxGroup( "Input" ) ] public float input_horizontal_deadzone = 0.01f; // 828 * 0.01 = 8 pixel
        [ BoxGroup( "Input" ) ] public AnimationCurve curve_upward;
        [ BoxGroup( "Input" ) ] public AnimationCurve curve_downward;

        [ BoxGroup( "Camera" ) ] public float camera_follow_speed;
        [ BoxGroup( "Camera" ) ] public float camera_follow_depth;
        [ BoxGroup( "Camera" ) ] public float camera_follow_height;
        [ BoxGroup( "Camera" ) ] public float camera_follow_rotation;
        [ BoxGroup( "Camera" ) ] public float camera_follow_duration;

        [ BoxGroup( "Paparazzi" ) ] public float paparazzi_rotate_duration = 0.15f;
        [ BoxGroup( "Paparazzi" ) ] public float paparazzi_photo_distance = 1f;
        
        [ BoxGroup( "Texture" ) ] public float texture_arrow_scrollSpeed = 1.0f;
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
