using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Entity_System
{
	interface IComponentOwner
	{
		//Yeah we're doing this again, because there are some cases where we may want to give components to components, like an emitter being given to a projectile
	}
}
