using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary
{
    // Interface som definere en kø. Det er en container, som grundlæggende har to operationer:
    //   Enqueue, som indsætter et element i enden af køen
    //   Dequeue, som fjerner og returnerer det første (ældste) element i køen
    // Svarende hertil omtales køen ofte som en FIFO (first in, first out) struktur.
    // Typisk vil typen dog også implementerer andre metoder, og det er ofte et valg, hvilke metoder man ønsker at implementere,
    // men som minimum skal der være en metode, som kan teste, om køen er tom.
    // Køen implementerer iterator mønsteret, og man kan diskutere, om det er fornuftigt, at man på den måde kan
    // iterere over køens elementer, da man på den måde får adgang til andre end det første element.
    public interface IQueue<T> : IEnumerable<T>
    {
        void Enqueue(T elem);     // metoden indsætter et element bagest i køen.
        T Dequeue();              // returnerer og fjerner det forreste (ældste) element i køen.
        T Peek();                 // returnerer det forreste element uden at fjerne det.
        void Clear();             // sletter køen, så resultatet er en tom kø.
        int Count { get; }        // Readonly property til antallet af elementer i køen.
    }
    
    public interface IPriorityQueue<T> : IEnumerable<T>
    {
        void Enqueue(T elem);         // metoden indsætter et element bagest i køen.
        void Enqueue(T elem, int p);  // metoden indsætter et element køen med prioritet p.
        T Dequeue();                  // returnerer og fjerner det forreste (ældste) element i køen.
        T Peek();                     // returnerer det forreste element uden at fjerne det.
        void Clear();                 // sletter køen, så resultatet er en tom kø.
        int Count { get; }            // readonly property til antallet af elementer i køen.
        T[] ToArray();                // returnerer indholdet af køen som et array.
    }

    public class QueueException : Exception
    {
        public QueueException(string message)
          : base(message)
        {
        }
    }
    public class Queue<T> : IQueue<T>
    {
        private ISingleList<T> list = new SingleList<T>();   // køens container

        // Returnerer antallet af elementer i køen: O(1)
        public int Count
        {
            get { return list.Count; }
        }

        // Sletter alle elementer i køen (tømmer køen): O(1)
        public void Clear()
        {
            list.Clear();
        }

        // Indsætter et element i slutningen af køen: O(1)
        public void Enqueue(T elem)
        {
            list.AddLast(elem);
        }

        // Sletter og returnerer det første (ældste) element i køen: O(1)
        // Hvis køen er tom, rejser metoden en QueueException.
        public T Dequeue()
        {
            if (list.Count == 0)
                throw new QueueException("The queue is empty...");
            return list.RemoveFirst();
        }

        // Returnerer det første (ældste) element i køen: O(1)
        // Hvis køen er tom, rejser metoden en QueueException.
        public T Peek()
        {
            if (list.Count == 0)
                throw new QueueException("The queue is empty...");
            return list.First();
        }

        // Implementerer iterator mønsteret, hvor man kan iterere over køens elementer.
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Brugerdefinerede type casts mellem klassen Queue<T> og et array.
        // Begge type casts virker ved kloning og har lineær kompleksitet. De er derfor defineret explicit.
        public static explicit operator T[](Queue<T> queue)
        {
            T[] elems = queue.list.ToArray();
            T[] arr = new T[elems.Length];
            for (int n = 0; n < elems.Length; ++n) 
                arr[n] = Tools.Clone(elems[n]);
            return arr;
        }

        public static explicit operator Queue<T>(T[] arr)
        {
            Queue<T> queue = new Queue<T>();
            for (int n = 0; n < arr.Length; ++n) 
                queue.Enqueue(Tools.Clone(arr[n]));
            return queue;
        }
    }

    // Implementerer en prioritetskø som et array af køer. Det er en meget simpel måde til at implementere en prioritets kø, og som
    // også virker, men der er bedre måder til det. Denne implementering er bedst egnet, hvis antallet af prioriteter er få, da
    // kompleksiteten af klassens metoder er knyttet til antallet af prioriteter P.
    public class PQueue<T> : IPriorityQueue<T>
    {
        private Queue<T>[] queue;     // array af køer til en prioritetskø - hvert array svarer til en prioritet

        // Opretter en prioritetskø med n prioriteter
        public PQueue(int n)
        {
            queue = new Queue<T>[n];
            for (int i = 0; i < queue.Length; ++i) 
                queue[i] = new Queue<T>();
        }

        // Returnerer antallet af prioriteter
        public int Priorities
        {
            get { return queue.Length; }
        }

        // Returnerer antallet af elementer i køen
        public int Count
        {
            get
            {
                int count = 0;
                foreach (Queue<T> q in queue) 
                    count += q.Count;
                return count;
            }
        }

        // Indsætter et element som det sidste element i køen - det sidste element i den sidste kø: O(1)
        public void Enqueue(T elem)
        {
            queue[queue.Length - 1].Enqueue(elem);
        }

        // Indsætter et element med prioritet p - dvs. at elementet indsættes i den p'te kø: O(1)
        // Hvis p har en ulovlig værdi, rejser metoden en QueueException.
        public void Enqueue(T elem, int p)
        {
            if (p < 0 || p >= queue.Length) 
                throw new QueueException("Illegal priority");
            queue[p].Enqueue(elem);
        }

        // Returnerer og sletter det ældste element i køen: O(P)
        // Hvis køen er tom, rejser metoden en QueueException.
        public T Dequeue()
        {
            for (int n = 0; n < queue.Length; ++n) 
                if (queue[n].Count > 0) 
                    return queue[n].Dequeue();
            throw new QueueException("The queue is empty...");
        }

        // Returnerer det ældste element i køen: O(P)
        // Hvis køen er tom, rejser metoden en QueueException.
        public T Peek()
        {
            for (int n = 0; n < queue.Length; ++n) 
                if (queue[n].Count > 0) 
                    return queue[n].Peek();
            throw new QueueException("The queue is empty...");
        }

        // Sletter indholdet af køen: O(P)
        public void Clear()
        {
            for (int n = 0; n < queue.Length; ++n)
                queue[n].Clear();
        }

		// Implementerer iterator mønsteret. Det er ikke oplagt, at det er fornuftigt at implementere dette mønster på en kø, da
		// man på den måde får adgang til andre end det ældste element.
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < queue.Length; ++i) 
                foreach (var elem in queue[i]) 
                    yield return elem;
		}

		public T[] GetElementsWithPriority(int i)
        {
            if (i < 0 || i >= queue.Length)
                throw new QueueException("Illegal priority");
            return queue[i].ToArray();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Returnerer indholdet af køen som et array. Det er et array med referencer til køens elementer, og også her skal man overveje
        // om denne operation er fornuftig.
        public T[] ToArray()
        {
            T[] arr = new T[Count];
            int n = 0;
            foreach (T e in this)
            {
                //Svarer til arr[n++]
                arr[n] = e;
                n++;
            }
            return arr;
        }
    }
}
