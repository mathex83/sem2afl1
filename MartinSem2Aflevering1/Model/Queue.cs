using System;
using System.Collections;
using System.Text;

namespace MartinSem2Aflevering1.Model
{
	public class Queue<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
	{
		public Queue();
		public Queue(IEnumerable<T> collection);
	}
}
