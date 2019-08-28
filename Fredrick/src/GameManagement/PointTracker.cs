using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class PointTracker : IObserver<string>
	{
		//remember we can reuse the same observer, a kill is a kill
		public int Change = 1;
		//Observer observer;
		public List<string> Activations = new List<string>();
		string Condition;

		public void OnCompleted()
		{
			throw new NotImplementedException();
		}

		public void OnError(Exception error)
		{
			throw new NotImplementedException();
		}

		public void OnNext(string value)
		{
			Activations.Add(value);
		}
	}
}
