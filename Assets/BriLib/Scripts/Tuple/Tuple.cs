using System;

namespace BriLib
{
    public class Tuple<T, K>
    {
        public T ItemOne { get; set; }
        public K ItemTwo { get; set; }

        public Tuple(T itemOne, K itemTwo)
        {
            ItemOne = itemOne;
            ItemTwo = itemTwo;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Tuple<T, K>)) return false;
            return this == (Tuple<T, K>)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ItemOne != null ? ItemOne.GetHashCode() : 0);
                hash = hash * 23 + (ItemTwo != null ? ItemOne.GetHashCode() : 0);
                return hash;
            }
        }

        public static bool operator ==(Tuple<T, K> a, Tuple<T, K> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(null, a) != (ReferenceEquals(null, b))) return false;

            if (a.ItemOne == null != (b.ItemOne == null)) return false;
            if (a.ItemTwo == null != (b.ItemTwo == null)) return false;
            return (a.ItemOne != null ? a.ItemOne.Equals(b.ItemOne) : true)
                && (a.ItemTwo != null ? a.ItemTwo.Equals(b.ItemTwo) : true);
        }

        public static bool operator !=(Tuple<T, K> a, Tuple<T, K> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("[Tuple ItemOne={0}, ItemTwo={1}]", ItemOne, ItemTwo);
        }
    }
    
    public class Tuple<T, K, L>
    {
        public T ItemOne { get; set; }
        public K ItemTwo { get; set; }
        public L ItemThree { get; set; }

        public Tuple(T itemOne, K itemTwo, L itemThree)
        {
            ItemOne = itemOne;
            ItemTwo = itemTwo;
            ItemThree = itemThree;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Tuple<T, K, L>)) return false;
            return this == (Tuple<T, K, L>)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ItemOne != null ? ItemOne.GetHashCode() : 0);
                hash = hash * 23 + (ItemTwo != null ? ItemTwo.GetHashCode() : 0);
                hash = hash * 23 + (ItemThree != null ? ItemThree.GetHashCode() : 0);
                return hash;
            }
        }

        public static bool operator ==(Tuple<T, K, L> a, Tuple<T, K, L> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(null, a) != (ReferenceEquals(null, b))) return false;

            if (a.ItemOne == null != (b.ItemOne == null)) return false;
            if (a.ItemTwo == null != (b.ItemTwo == null)) return false;
            if (a.ItemThree == null != (b.ItemThree == null)) return false;
            return (a.ItemOne != null ? a.ItemOne.Equals(b.ItemOne) : true)
                && (a.ItemTwo != null ? a.ItemTwo.Equals(b.ItemTwo) : true)
                && (a.ItemThree != null ? a.ItemThree.Equals(b.ItemThree) : true);
        }

        public static bool operator !=(Tuple<T, K, L> a, Tuple<T, K, L> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("[Tuple ItemOne={0}, ItemTwo={1}, ItemThree={2}]", ItemOne, ItemTwo, ItemThree);
        }
    }

    public class Tuple<T, K, L, M>
    {
        public T ItemOne { get; set; }
        public K ItemTwo { get; set; }
        public L ItemThree { get; set; }
        public M ItemFour { get; set; }

        public Tuple(T itemOne, K itemTwo, L itemThree, M itemFour)
        {
            ItemOne = itemOne;
            ItemTwo = itemTwo;
            ItemThree = itemThree;
            ItemFour = itemFour;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Tuple<T, K, L, M>)) return false;
            return this == (Tuple<T, K, L, M>)obj;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ItemOne != null ? ItemOne.GetHashCode() : 0);
                hash = hash * 23 + (ItemTwo != null ? ItemTwo.GetHashCode() : 0);
                hash = hash * 23 + (ItemThree != null ? ItemThree.GetHashCode() : 0);
                hash = hash * 23 + (ItemFour != null ? ItemFour.GetHashCode() : 0);
                return hash;
            }
        }

        public static bool operator ==(Tuple<T, K, L, M> a, Tuple<T, K, L, M> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(null, a) != (ReferenceEquals(null, b))) return false;

            if (a.ItemOne == null != (b.ItemOne == null)) return false;
            if (a.ItemTwo == null != (b.ItemTwo == null)) return false;
            if (a.ItemThree == null != (b.ItemThree == null)) return false;
            if (a.ItemFour == null != (b.ItemFour == null)) return false;
            return (a.ItemOne != null ? a.ItemOne.Equals(b.ItemOne) : true)
                && (a.ItemTwo != null ? a.ItemTwo.Equals(b.ItemTwo) : true)
                && (a.ItemThree != null ? a.ItemThree.Equals(b.ItemThree) : true)
                && (a.ItemFour != null ? a.ItemFour.Equals(b.ItemFour) : true);
        }

        public static bool operator !=(Tuple<T, K, L, M> a, Tuple<T, K, L, M> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("[Tuple ItemOne={0}, ItemTwo={1}, ItemThree={2}, ItemFour={3}]", 
                ItemOne, ItemTwo, ItemThree, ItemFour);
        }
    }
}
