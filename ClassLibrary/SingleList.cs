using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    // Definerer en enkelt hægtet liste.
    // Listen implementerer iterator mønsteret, så man kan traversere listen fra start til slut.
    public interface ISingleList<T> : IEnumerable<T>
    {
        int Count { get; }                  // antallet af elementer i listen
        T First { get; }                    // returnerer det første element i listen
        void AddFirst(T elem);              // indsætter et element som det første i listen
        void AddLast(T elem);               // indsætter et element som det sidste i listen
        void AddAfter(T elem1, T elem2);    // indsætter elementet elem2 som en efterfølger til elem1
        T RemoveFirst();                         // sletter det første element i listen og returnerer det
        void Remove(T elem);                // sletter elementet elem
        void Clear();                       // sletter alle elementer i listen
        bool Contains(T elem);              // tester om elementet elem findes i listen
        T[] ToArray();                      // returnerer listens elementer som et array
        T[] Clone();                        // returnerer listens elementer som et array, men sådan at hvert element er en kopi af listens element
    }

    public class SingleListException : Exception
    {
        public SingleListException(string message)
          : base(message)
        {
        }
    }

    // Implementerer en enkelt hægtet liste.
    public class SingleList<T> : ISingleList<T>
    {
        private Node start = null;    // pointer til starten af listen
        private Node end = null;      // pointer til enden af listen
        private int count = 0;        // tæller antal af listens elementer

        // Opretter en tom liste.
        public SingleList()
        {
        }

        // Opretter en liste initialiseret med elementer i arrayet elems. Listen vil indeholde elementerne i samme orden som i elems.
        public SingleList(params T[] elems)
        {
            for (int i = elems.Length - 1; i >= 0; --i) AddFirst(elems[i]);
        }

        // Returnerer antallet af elementer i listen.
        // Propertyen er implementeret med konstant tidskompleksitet.
        public int Count
        {
            get { return count; }
        }

        // Returnerer det første element i listen. Er listen tom, rejses en exception.
        // Propertyen er implementeret med konstant tidskompleksitet.
        public T First
        {
            get
            {
                if (count == 0) throw new SingleListException("The list is empty");
                return start.Element;
            }
        }

        // Indsætter et nyt element som det første element i listen. Metoden har konstant tidskompleksitet.
        public void AddFirst(T elem)
        {
            start = new Node { Element = elem, Next = start };
            ++count;
        }

        // Indsætter et nyt element som det sidste element i listen. Metoden har konstant tidskompleksitet.
        public void AddLast(T elem)
        {
            // implementeringen starter med et specielt tilfælde, hvor listen er tom
            // i dette tilfælde skal der oprettes en ny node, som hverken har en efterfølger eller forgænger, 
            // og både start og end skal referere til denne node
            if (count == 0) end = start = new Node { Element = elem };

            // i det generelle tilfælde skal der oprettes en ny node, som pege tilbage på den node, som før var det sidste element
            // den node der før var det sidste, skal pege fremad på nye node, og end skal sættes til at pege på den nye node
            else end = end.Next = new Node { Element = elem, Prev = end };
            ++count;
        }

        // Indsætter et nyt element elem2 som efterfølger til elem1. Hvis elem1 ikke findes, rejser metoden en exception.
        // Metoden kræver en søgning efter elem1 fra starten af listen, og metoden har derfor lineær middel tidskompleksitet.
        public void AddAfter(T elem1, T elem2)
        {
            // itererer over listens elementer indtil man finder den node, som indeholder elem1
            // findes elem1 indsættes elem2 som en efterfølger til elem1 og metoden returnerer
            for (Node node = start; node != null; node = node.Next)
                if (node.Element.Equals(elem1))
                {
                    // det nye element skal pege på efterfølgeren til elem1 og elem1 skal pege på det nye element
                    node.Next = new Node { Element = elem2, Next = node.Next };
                    ++count;
                    return;
                }
            throw new SingleListException(string.Format("The element {0} not found", elem1));
        }

        // Metoden returnerer true, hvis elementet elem findes i listen.
        // Metoden traverserer listen og har dermed lineær tidskompleksitet.
        public bool Contains(T elem)
        {
            for (Node node = start; node != null; node = node.Next) if (node.Element.Equals(elem)) return true;
            return false;
        }

        // Sletter og returnerer det første element i listen. Hvis listen er tom, rejser metoden en exception.
        // Metoden har konstant tidskompleksitet.
        public T RemoveFirst()
        {
            if (count == 0) throw new SingleListException("The list is empty");
            T elem = start.Element;
            start = start.Next;
            --count;
            return elem;
        }

        // Sletter elementet elem i listen. Hvis elementet ikke findes, rejser metoden en exception.
        // Metoden skal søge efter elementet elem og har derfor lineær tidskompleksitet.
        public void Remove(T elem)
        {
            if (count == 0) throw new SingleListException("The list is empty");

            // sletning af det første element behandles specielt, da start pointeren i dette tilfælde skal ændres
            if (start.Element.Equals(elem))
            {
                start = start.Next;
                --count;
                return;
            }

            // det generelle tilfælde, hvor man skal søge efter elementet elem
            // findes elementet, skal next pointeren i forgængeren ændres, så den peger på efterfølgeren til det element, der skal slettes
            for (Node node = start; node.Next != null; node = node.Next)
                if (node.Next.Element.Equals(elem))
                {
                    node.Next = node.Next.Next;
                    --count;
                    return;
                }
            throw new SingleListException(string.Format("The element {0} not found", elem));
        }

        // Sletter indholdet af listen. Metoden har konstant tidskompleksitet.
        public void Clear()
        {
            start = null;
            count = 0;
        }

        // Implementerer iterator mønsteret
        public IEnumerator<T> GetEnumerator()
        {
            for (Node node = start; node != null; node = node.Next) yield return node.Element;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Returnerer et array med listens elementer
        public T[] ToArray()
        {
            T[] arr = new T[count];
            int n = 0;
            for (Node node = start; node != null; node = node.Next) arr[n++] = node.Element;
            return arr;
        }

        // Returnerer et array med listens elementer, hvor alle elementer er en klon af listens elementer
        public T[] Clone()
        {
            try
            {
                T[] arr = new T[count];
                int n = 0;
                for (Node node = start; node != null; node = node.Next) arr[n++] = Tools.Clone(node.Element);
                return arr;
            }
            catch
            {
                throw new SingleListException(string.Format("{0} er ikke serialiserbar", typeof(T)));
            }
        }

        // Overstyring af ToString() som returnerer en streng med listens elementer adskilt af komma.
        // Metoden er alene beregnet til test.
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            if (count > 0)
            {
                builder.Append(string.Format(" {0}", start.Element));
                for (Node node = start.Next; node != null; node = node.Next) builder.Append(string.Format(", {0}", node.Element));
            }
            builder.Append(" ]");
            return builder.ToString();
        }

        // Definerer en node til listen.
        private class Node
        {
            public T Element { get; set; }
            public Node Next { get; set; }
            // Der er ikke behov for bagudrettede pointere i en SingleList, modsat en DoubleList. 
            // Men AddLast metoden, indsat for at implementere en kø, skal have en Prev reference for at virke korrekt..
            public Node Prev { get; set; }
        }
    }
}
