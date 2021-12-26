/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "SwipeInputEvent", menuName = "FF/Event/Input/SwipeInputEvent" ) ]
    public class SwipeInputEvent : Vector2GameEvent
    {
        public float angleThreshold;

		[ BoxGroup( "Fired Events" ) ] public GameEvent swiped_left_event;
		[ BoxGroup( "Fired Events" ) ] public GameEvent swiped_right_event;
        [ HideInInspector ] public Vector2 inputValue;

		public void ReceiveInput( Vector2 swipeDelta )
        {
            eventValue = swipeDelta;
			inputValue = DecideDirection( Vector2.Angle( Vector2.right, swipeDelta ), swipeDelta );

			if( inputValue != Vector2.zero )
                Raise();
        }

		Vector2 DecideDirection( float unsignedAngle, Vector2 delta )
        {
			if( unsignedAngle > 180 - angleThreshold )
			{
				swiped_left_event.Raise();
				return Vector2.left;
			}
			else if( angleThreshold <= unsignedAngle && unsignedAngle <= 180 - angleThreshold )
			{
				if( delta.y >= 0 )
					return Vector2.up;
				else
					return Vector2.down;
			}
			else if( unsignedAngle < angleThreshold )
			{
				swiped_right_event.Raise();
				return Vector2.right;
			}
			else
				return Vector2.zero;
		}
    }
}