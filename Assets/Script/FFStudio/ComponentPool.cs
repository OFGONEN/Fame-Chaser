/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
	public abstract class ComponentPool< T > : RunTimePool< T > where T: Component
	{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
		public override void InitPool()
        {
            stack = new Stack< T >( stackSize );

            for( var i = 0; i < stackSize; i++ )
			{
				var entity = InitEntity();
				stack.Push( entity );
			}
        }

		public override T GetEntity()
		{
			T entity;

			if( stack.Count > 0 )
				entity = stack.Pop();
			else
				entity = InitEntity();

			return entity;
		}

		public override void ReturnEntity( T entity )
        {
			stack.Push( entity );
		}
#endregion

#region Implementation
		protected override T InitEntity()
        {
            var entity = GameObject.Instantiate( poolEntity );
			return entity;
		}
#endregion
	}
}