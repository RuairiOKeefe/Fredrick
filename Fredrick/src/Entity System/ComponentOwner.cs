using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	interface ComponentOwner
	{
		string Id { get; }
		T GetComponent<T>() where T : Component;
	}
}
