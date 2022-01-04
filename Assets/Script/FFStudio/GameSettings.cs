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
		[ Foldout( "UI Settings" ), Tooltip( "Strength of the punch scale for ui element" ) ] public float ui_Entity_PunchScale_Strenght;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the punch scale for ui element" ) ] public float ui_Entity_PunchScale_Duration;
		[ Foldout( "UI Settings" ), Tooltip( "Joy Stick"                                        )] public float ui_Entity_JoyStick_Gap;
        [ Foldout( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;
        [ Foldout( "UI Settings" ), Tooltip( "Reference UI width"     ) ] public int ui_screen_width;

        [ Foldout( "UI Particle" ), Tooltip( "UI Particle - spawn radius"     ) ] public int ui_particle_spawn_radius;
        [ Foldout( "UI Particle" ), Tooltip( "UI Particle - horizontal move ease positive"     ) ] public AnimationCurve ui_particle_ease_horizontal_positive;
        [ Foldout( "UI Particle" ), Tooltip( "UI Particle - horizontal move ease negative"     ) ] public AnimationCurve ui_particle_ease_horizontal_negative;
        [ Foldout( "UI Particle" ) ] public float ui_particle_fame_count;
        [ Foldout( "UI Particle" ) ] public float ui_particle_dolar_count;
        [ Foldout( "UI Particle" ) ] public Sprite ui_particle_fame_sprite;
        [ Foldout( "UI Particle" ) ] public Sprite ui_particle_dolar_sprite;

        [ Foldout( "SwapTriggerLane" ) ] public float swap_point_out_randomness = 1f;
        [ Foldout( "SwapTriggerLane" ) ] public float swap_point_out_duration = 0.75f;
        [ Foldout( "SwapTriggerLane" ) ] public float swap_point_in_duration = 0.75f;

        // Player
        [ Foldout( "Player" ) ] public float player_movement_speed_lateral = 1f; 
        [ Foldout( "Player" ) ] public float player_movement_speed_forward = 1f; 
        [ Foldout( "Player" ) ] public float player_movement_clamp_lateral = 5f; 
        [ Foldout( "Player" ) ] public float player_duration_cloth_off = 0.75f; 
        [ Foldout( "Player" ) ] public float player_duration_rotation = 0.5f; 

        // Sugar Daddy
        [ Foldout( "Daddy" ) ] public float daddy_movement_speed   = 5f;
        [ Foldout( "Daddy" ) ] public float daddy_couple_duration  = 0.75f;
        [ Foldout( "Daddy" ) ] public float daddy_ragdoll_duration = 1f;
        [ Foldout( "Daddy" ) ] public float daddy_ragdoll_force    = 50f;
        [ Foldout( "Daddy" ) ] public Vector2 daddy_ragdoll_force_vertical;
        [ Foldout( "Daddy" ) ] public int daddy_money_bill         = 10;
        [ Foldout( "Daddy" ) ] public float daddy_money_delay      = 0.125f;
        [ Foldout( "Daddy" ) ] public float daddy_money_transfer   = 0.2f;
        [ Foldout( "Daddy" ) ] public float daddy_money_height     = 0.2f;
        [ Foldout( "Daddy" ) ] public Vector2Int daddy_money_amount;

        [ Foldout( "Input" ) ] public float input_horizontal_deadzone = 0.01f; // 828 * 0.01 = 8 pixel
        [ Foldout( "Input" ) ] public AnimationCurve curve_upward;
        [ Foldout( "Input" ) ] public AnimationCurve curve_downward;

        [ Foldout( "Camera" ) ] public float camera_follow_speed;
        [ Foldout( "Camera" ) ] public float camera_follow_speed_depth;
        [ Foldout( "Camera" ) ] public float camera_follow_depth;
        [ Foldout( "Camera" ) ] public float camera_follow_height;
        [ Foldout( "Camera" ) ] public float camera_follow_rotation;
        [ Foldout( "Camera" ) ] public float camera_follow_duration;
        [ Foldout( "Camera" ) ] public float camera_trigger_follow_depth;
        [ Foldout( "Camera" ) ] public float camera_trigger_follow_rotation;
        [ Foldout( "Camera" ) ] public float camera_trigger_follow_lateral;

        [ Foldout( "Paparazzi" ) ] public float paparazzi_rotate_duration = 0.15f;
        [ Foldout( "Paparazzi" ) ] public float paparazzi_photo_distance = 1f;

        [ Foldout( "Currency" ) ] public int[] currency_level_dolar;
        [ Foldout( "Currency" ) ] public int[] currency_level_fame;

        [ Foldout( "Cloth" ) ] public Color[] cloth_outline_color;
        [ Foldout( "Cloth" ) ] public float[] cloth_outline_widht;

        [ Foldout( "Particle" ) ] public float particle_cloth_distance;
        [ Foldout( "Particle" ) ] public float particle_cloth_radius;
        
        [ Foldout( "Texture" ) ] public float texture_arrow_scrollSpeed = 1.0f;
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
