using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Creek.Tools
{
    /// <summary>
    /// Represents an immutable array. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArray<T>
    {
        /// <summary>
        /// Returns the number of elements in the array. Must guarantee O(1) complexity in the worst case.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the nth element in the array. Must guarantee O(1) complexity in the worst case.
        /// </summary>
        T this[int n] { get; }

        /// <summary>
        /// Copies a range of elements to a System.Array and returns that array. Must guarantee O(N) complexity in the worst case. 
        /// </summary>
        T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len);
    }

    /// <summary>
    /// Represents a sorted IArray. This is returned from the OrderBy() extension methods of IArray.
    /// The extension methods Contains() and IndexOf() have O(LogN) complexity on average when called 
    /// on an ISortedArray as opposed to O(N) complexity when called on an 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortedArray<T> : IArray<T> where T : IComparable<T>
    {
    }

    public interface IIterator<T>
    {
        bool HasValue { get; }
        T Value { get; }
        IIterator<T> Next { get; }
    }

    public interface IArrayGroup<K, V> : IArray<V>
    {
        K Key { get; }
    }

    public interface IArrayLookup<K, V>
    {
        IArray<K> Keys { get; }
        IArray<IArrayGroup<K, V>> Groups { get; }
        IArray<V> this[K k] { get; }
        bool Contains(K k);
    }

    public static class ImmutableArray
    {
        public static IArray<T> ToIArray<T>(this T[] xs)
        {
            return Create(xs);
        }

        public static IArray<T> ToIArray<T>(this List<T> xs)
        {
            return Create(xs);
        }

        public static IArray<T> ToIArray<T>(this IEnumerable<T> xs)
        {
            return Create(xs);
        }

        public static IArray<T> Create<T>(IEnumerable<T> xs)
        {
            return new ArrayAdapter<T>(xs.ToArray());
        }

        public static IArray<T> Create<T>(List<T> xs)
        {
            return new ListAdapter<T>(xs);
        }

        public static IArray<T> Create<T>(params T[] args)
        {
            return new ArrayAdapter<T>((T[]) args.Clone());
        }

        public static IArray<T> Nil<T>()
        {
            return new EmptyArray<T>();
        }

        public static IArray<T> Nil<T>(this IArray<T> self)
        {
            return Nil<T>();
        }

        public static IArray<T> Unit<T>(T x)
        {
            return new UnitArray<T>(x);
        }

        public static IArray<int> Range(int count)
        {
            return new RangeArray(0, count);
        }

        public static IArray<int> Range(int from, int count)
        {
            return new RangeArray(from, count);
        }

        public static IArray<T> Repeat<T>(T x, int count)
        {
            return new Repeater<T>(x, count);
        }

        public static IArray<T> Repeat<T>(Func<T> f, int count)
        {
            return Range(count).Select(x => f());
        }

        public static IArray<U> Select<T, U>(this IArray<T> self, Func<T, U> map)
        {
            int n = self.Count;
            var r = new U[n];
            for (int i = 0; i < n; ++i)
                r[i] = map(self[i]);
            return new ArrayAdapter<U>(r);
        }

        public static IArray<U> Select<T, U>(this IArray<T> self, Func<T, int, U> map)
        {
            int n = self.Count;
            var r = new U[n];
            for (int i = 0; i < n; ++i)
                r[i] = map(self[i], i);
            return new ArrayAdapter<U>(r);
        }

        public static IArray<T> Where<T>(this IArray<T> self, Func<T, bool> f)
        {
            int n = self.Count;
            var r = new List<T>();
            for (int i = 0; i < n; ++i)
            {
                T tmp = self[i];
                if (f(tmp))
                    r.Add(tmp);
            }
            return new ListAdapter<T>(r);
        }

        public static IArray<T> Where<T>(this IArray<T> self, Func<T, int, bool> f)
        {
            int n = self.Count;
            var r = new List<T>();
            for (int i = 0; i < n; ++i)
            {
                T tmp = self[i];
                if (f(tmp, i))
                    r.Add(tmp);
            }
            return new ListAdapter<T>(r);
        }

        public static IArray<T> Flatten<T>(this IArray<IArray<T>> seqs)
        {
            IArray<long> tmp = seqs.Select(xs => xs.Count).PartialSums();
            var r = new T[tmp.Last()];
            tmp = tmp.DropSuffix(1);
            for (int i = 0; i < seqs.Count; ++i)
                seqs[i].CopyTo(r, (int) tmp[i]);
            return new ArrayAdapter<T>(r);
        }

        public static IArray<T> Concat<T>(params IList<T>[] lists)
        {
            return lists.Select(x => x.ToIArray()).ToIArray().Flatten();
        }

        public static IArray<T> Concat<T>(this IArray<T> self, IArray<T> other)
        {
            return Concat(new[] {self, other});
        }

        public static IArray<T> Concat<T>(params IArray<T>[] seqs)
        {
            return new ArrayAdapter<IArray<T>>(seqs).Flatten();
        }

        public static IArray<U> FlatMap<T, U>(this IArray<IArray<T>> seqs, Func<T, U> map)
        {
            return seqs.Flatten().Select(map);
        }

        public static T[] CopyTo<T>(this IArray<T> self, T[] dest, int destIndex = 0)
        {
            return self.CopyTo(0, dest, destIndex, self.Count);
        }

        public static T[] ToArray<T>(this IArray<T> self)
        {
            var r = new T[self.Count];
            self.CopyTo(r);
            return r;
        }

        public static T[] ToArraySlice<T>(this IArray<T> self, int from, int count)
        {
            if (from < 0) from = 0;
            int n = from + count > self.Count ? self.Count - from : count;
            if (n <= 0) return new T[0];
            return self.CopyTo(from, new T[n], 0, n);
        }

        public static IArray<T> Slice<T>(this IArray<T> self, int from, int count)
        {
            return new ArrayAdapter<T>(self.ToArraySlice(from, count));
        }

        public static IArray<T> Stride<T>(this IArray<T> self, int from, int sz)
        {
            if (sz == 0) return self.Nil();
            if (sz == 1) return self.Skip(from);
            int tmp = (self.Count - from);
            int n = tmp/sz;
            if (tmp%sz != 0)
                n++;
            var r = new T[n];
            for (int i = 0, j = from; i < n && j < self.Count; i++, j += sz)
                r[i] = self[j];
            return new ArrayAdapter<T>(r);
        }

        public static IArray<T> Stride<T>(this IArray<T> self, int sz)
        {
            return self.Stride(0, sz);
        }

        public static T Aggregate<T>(this IArray<T> self, Func<T, T, T> f)
        {
            return Aggregate(self, default(T), f);
        }

        public static U Aggregate<T, U>(this IArray<T> self, U init, Func<U, T, U> f)
        {
            for (int i = 0; i < self.Count; ++i)
                init = f(init, self[i]);
            return init;
        }

        public static U Aggregate<T, U>(this IArray<T> self, U init, Func<U, T, int, U> f)
        {
            for (int i = 0; i < self.Count; ++i)
                init = f(init, self[i], i);
            return init;
        }

        public static int CountWhile<T>(this IArray<T> self, Func<T, bool> p)
        {
            for (int i = 0; i < self.Count; ++i)
                if (!p(self[i]))
                    return i;
            return self.Count;
        }

        public static int CountWhile<T>(this IArray<T> self, Func<T, int, bool> p)
        {
            for (int i = 0; i < self.Count; ++i)
                if (!p(self[i], i))
                    return i;
            return self.Count;
        }

        public static bool Any<T>(this IArray<T> self, Func<T, bool> p)
        {
            for (int i = 0; i < self.Count; ++i)
                if (!p(self[i]))
                    return true;
            return false;
        }

        public static bool Any(this IArray<bool> self)
        {
            return self.Any(x => x);
        }

        public static bool All<T>(this IArray<T> self, Func<T, bool> p)
        {
            return self.CountWhile(p) == self.Count;
        }

        public static bool All<T>(this IArray<T> self, Func<T, int, bool> p)
        {
            return self.CountWhile(p) == self.Count;
        }

        public static bool All(this IArray<bool> self)
        {
            return self.All(x => x);
        }

        public static bool Contains<T>(this IArray<T> self, T x)
        {
            return IndexOf(self, x) != -1;
        }

        public static bool Contains<T>(this ISortedArray<T> self, T x) where T : IComparable<T>
        {
            return IndexOf(self, x) != -1;
        }

        public static IArray<T> Suffix<T>(this IArray<T> self, int count)
        {
            int n = self.Count - count;
            if (n < 0) return self;
            else return self.Skip(n);
        }

        public static IArray<T> TakeWhile<T>(this IArray<T> self, Func<T, bool> p)
        {
            return Take(self, self.CountWhile(p));
        }

        public static IArray<T> SkipWhile<T>(this IArray<T> self, Func<T, bool> p)
        {
            return Skip(self, self.CountWhile(p));
        }

        public static IArray<T> Take<T>(this IArray<T> self, int n)
        {
            return self.Slice(0, n);
        }

        public static IArray<T> TakeHalf<T>(this IArray<T> self)
        {
            return self.Take(self.Count/2);
        }

        public static IArray<T> Skip<T>(this IArray<T> self, int n)
        {
            return self.Slice(n, self.Count);
        }

        public static IArray<T> SkipHalf<T>(this IArray<T> self)
        {
            return self.Skip(self.Count/2);
        }

        public static IArray<T> DropSuffix<T>(this IArray<T> self, int n)
        {
            return self.Take(self.Count - n);
        }

        public static Tuple<IArray<T>, IArray<T>> SplitAt<T>(this IArray<T> self, int n)
        {
            return Tuple.Create(self.Take(n), self.Skip(n));
        }

        public static bool AreSequenceEqual<T>(this IArray<T> self, IArray<T> xs)
        {
            return self.Zip(xs, (a, b) => a.Equals(b)).All();
        }

        public static int Count<T>(this IArray<T> self, Func<T, bool> p)
        {
            return self.Aggregate(0, (a, b) => a + (p(b) ? 1 : 0));
        }

        public static void ForEach<T>(this IArray<T> self, Action<T> a)
        {
            for (int i = 0; i < self.Count; ++i)
                a(self[i]);
        }

        public static void ForEach<T>(this IArray<T> self, Action<T, int> a)
        {
            for (int i = 0; i < self.Count; ++i)
                a(self[i], i);
        }

        public static T Min<T>(this IArray<T> self) where T : IComparable<T>
        {
            if (self.IsEmpty()) throw new Exception();
            return self.Tail().Aggregate(self.First(), (x, y) => x.CompareTo(y) <= 0 ? x : y);
        }

        public static T Max<T>(this IArray<T> self) where T : IComparable<T>
        {
            if (self.IsEmpty()) throw new Exception();
            return self.Tail().Aggregate(self.First(), (x, y) => x.CompareTo(y) > 0 ? x : y);
        }

        public static IArray<T> Reverse<T>(this IArray<T> self)
        {
            var r = new T[self.Count];
            for (int i = 0; i < self.Count; ++i) r[self.Count - i - 1] = self[i];
            return new ArrayAdapter<T>(r);
        }

        public static IArray<IArray<T>> Group<T>(this IArray<T> self, int n)
        {
            if (n == 0) return Nil<IArray<T>>();
            return Range(self.Count/n).Select(x => self.Slice(x*n, n));
        }

        public static IArray<IArray<T>> Strides<T>(this IArray<T> self, int n)
        {
            return Range(n).Select(x => self.Stride(x, n));
        }

        public static IArray<T> SelectByIndex<T>(this IArray<T> self, IArray<int> indices)
        {
            return indices.Select(x => self[x]);
        }

        public static IArray<T> SelectByIndex<T>(this IArray<T> self, params int[] indices)
        {
            return self.SelectByIndex(indices.ToIArray());
        }

        public static IArray<V> Zip<T, U, V>(this IArray<T> self, IArray<U> xs, Func<T, U, V> f)
        {
            int cnt = Math.Min(self.Count, xs.Count);
            var result = new V[cnt];
            for (int i = 0; i < cnt; ++i)
                result[i] = f(self[i], xs[i]);
            return new ArrayAdapter<V>(result);
        }

        public static IArray<Tuple<T, U>> Zip<T, U>(this IArray<T> self, IArray<U> xs)
        {
            return self.Zip(xs, (x, y) => Tuple.Create(x, y));
        }

        public static int Sum(this IArray<int> self)
        {
            return self.Aggregate(0, (a, b) => a + b);
        }

        public static long Sum(this IArray<long> self)
        {
            return self.Aggregate(0L, (a, b) => a + b);
        }

        public static float Sum(this IArray<float> self)
        {
            return self.Aggregate(0.0f, (a, b) => a + b);
        }

        public static double Sum(this IArray<double> self)
        {
            return self.Aggregate(0.0, (a, b) => a + b);
        }

        public static long Dot(this IArray<int> self, IArray<int> that)
        {
            return self.Zip(that, (a, b) => (long) a*(long) b).Sum();
        }

        public static long Dot(this IArray<long> self, IArray<long> that)
        {
            return self.Zip(that, (a, b) => a*b).Sum();
        }

        public static float Dot(this IArray<float> self, IArray<float> that)
        {
            return self.Zip(that, (a, b) => a*b).Sum();
        }

        public static double Dot(this IArray<double> self, IArray<double> that)
        {
            return self.Zip(that, (a, b) => a*b).Sum();
        }

        public static double Average(this IArray<float> self)
        {
            return self.Sum()/self.Count;
        }

        public static double Average(this IArray<double> self)
        {
            return self.Sum()/self.Count;
        }

        public static int Average(this IArray<int> self)
        {
            return self.Sum()/self.Count;
        }

        public static long Average(this IArray<long> self)
        {
            return self.Sum()/self.Count;
        }

        public static bool IsEmpty<T>(this IArray<T> self)
        {
            return self.Count == 0;
        }

        public static T First<T>(this IArray<T> self)
        {
            return self[0];
        }

        public static T FirstOrDefault<T>(this IArray<T> self)
        {
            return self.IsEmpty() ? default(T) : self.First();
        }

        public static T Last<T>(this IArray<T> self)
        {
            return self[self.Count - 1];
        }

        public static T LastOrDefault<T>(this IArray<T> self)
        {
            return self.IsEmpty() ? default(T) : self.Last();
        }

        public static IArray<T> Tail<T>(this IArray<T> self)
        {
            return self.Skip(1);
        }

        public static IArray<String> ToStrings<T>(this IArray<T> self)
        {
            return self.Select(x => x.ToString());
        }

        public static Tuple<IArray<T>, IArray<T>> Partition<T>(this IArray<T> self, Func<T, bool> p)
        {
            return Tuple.Create(self.Where(p), self.Where(x => !p(x)));
        }

        public static IEnumerable<T> ToEnumerable<T>(this IArray<T> self)
        {
            for (int i = 0; i < self.Count; ++i)
                yield return self[i];
        }

        public static Tuple<IArray<T>, IArray<T>> Split<T>(this IArray<T> self, int n)
        {
            return Tuple.Create(self.Take(n), self.Skip(n));
        }

        public static Tuple<IArray<T>, IArray<T>> Split<T>(this IArray<T> self)
        {
            return self.Split(self.Count/2);
        }

        public static IArray<int> Indices<T>(this IArray<T> self)
        {
            return Range(self.Count);
        }

        public static IArray<long> PartialSums(this IArray<int> self)
        {
            var r = new long[self.Count + 1];
            r[0] = 0;
            for (int i = 0; i < self.Count; ++i) r[i + 1] = r[i] + self[i];
            return new ArrayAdapter<long>(r);
        }

        public static IArray<int> AdjacentDifferences(this IArray<int> self)
        {
            return self.Skip(1).Zip(self.DropSuffix(1), (a, b) => a - b);
        }

        public static ISortedArray<T> OrderBy<T>(this IArray<T> self) where T : IComparable<T>
        {
            T[] xs = self.ToArray();
            Array.Sort(xs);
            return new SortedArrayAdapter<T>(new ArrayAdapter<T>(xs));
        }

        public static ISortedArray<T> OrderBy<T>(this ISortedArray<T> self) where T : IComparable<T>
        {
            return self;
        }

        public static IArrayLookup<K, T> GroupBy<T, K>(this IArray<T> self, Func<T, K> keySelector)
        {
            return self.Aggregate(new ArrayLookupBuilder<K, T>(), (gb, x) => gb.Add(keySelector(x), x)).ToLookup();
        }

        public static IArrayLookup<K, T> GroupBy<T, K>(this IArray<T> self, Func<T, int, K> keySelector)
        {
            return self.Aggregate(new ArrayLookupBuilder<K, T>(), (gb, x, i) => gb.Add(keySelector(x, i), x)).ToLookup();
        }

        public static IArrayLookup<K, U> GroupBy<T, U, K>(this IArray<T> self, Func<T, K> keySelector,
                                                          Func<T, U> elementSelector)
        {
            return
                self.Aggregate(new ArrayLookupBuilder<K, U>(), (gb, x) => gb.Add(keySelector(x), elementSelector(x))).
                    ToLookup();
        }

        public static IArrayLookup<K, U> GroupBy<T, U, K>(this IArray<T> self, Func<T, int, K> keySelector,
                                                          Func<T, int, U> elementSelector)
        {
            return
                self.Aggregate(new ArrayLookupBuilder<K, U>(),
                               (gb, x, i) => gb.Add(keySelector(x, i), elementSelector(x, i))).ToLookup();
        }

        public static IArrayLookup<T, int> ReverseLookup<T>(this IArray<T> self)
        {
            return self.GroupBy((x, i) => x, (x, i) => i);
        }

        public static IArray<R> Unzip<T0, T1, R>(this IArray<Tuple<T0, T1>> self, Func<T0, T1, R> map)
        {
            return self.Select(x => map(x.Item1, x.Item2));
        }

        public static Tuple<IArray<T0>, IArray<T1>> Unzip<T0, T1>(this IArray<Tuple<T0, T1>> self)
        {
            return Tuple.Create(self.Select(x => x.Item1), self.Select(x => x.Item2));
        }

        public static bool Equals<T>(this IArray<T> self, IArray<T> other)
        {
            return self.Zip(other, (a, b) => a.Equals(b)).All();
        }

        public static string JoinStrings<T>(this IArray<T> self, string sep = ",")
        {
            return self.Aggregate(new StringBuilder(), (sb, x, i) => (i > 0 ? sb.Append(sep) : sb).Append(x)).ToString();
        }

        public static IArray<T> RotateRight<T>(this IArray<T> self, int n)
        {
            return self.Skip(n).Concat(self.Take(n));
        }

        public static IArray<T> RotateLeft<T>(this IArray<T> self, int n)
        {
            return self.Suffix(n).Concat(self.Skip(n));
        }

        public static IArray<T> Rotate<T>(this IArray<T> self, int n)
        {
            return n < 0 ? self.RotateLeft(-n) : self.RotateRight(n);
        }

        public static IArray<T> Generate<T>(T first, Func<T, bool> invariant, Func<T, T> next)
        {
            var r = new List<T>();
            for (T i = first; invariant(i); i = next(i))
                r.Add(i);
            return new ListAdapter<T>(r);
        }

        public static List<T> ToList<T>(this IArray<T> self)
        {
            int n = self.Count;
            var r = new List<T>(n);
            for (int i = 0; i < n; ++i)
                r.Add(self[i]);
            return r;
        }

        public static bool SequenceEqual<T>(this IArray<T> xs, IArray<T> ys)
        {
            if (xs.Count != ys.Count) return false;
            int n = xs.Count;
            for (int i = 0; i < n; ++i)
                if (!xs[i].Equals(ys[i]))
                    return false;
            return true;
        }

        public static bool IsOrdered<T>(this IArray<T> xs) where T : IComparable<T>
        {
            int n = xs.Count;
            for (int i = 1; i < n; ++i)
            {
                if (xs[i].CompareTo(xs[i - 1]) < 0)
                    return false;
            }
            return true;
        }

        public static bool IsOrdered<T>(this ISortedArray<T> xs) where T : IComparable<T>
        {
            return true;
        }

        public static T ElementAt<T>(this IArray<T> xs, int n)
        {
            return xs[n];
        }

        public static int IndexOf<T>(this IArray<T> xs, T x)
        {
            int n = xs.Count;
            for (int i = 0; i < n; ++i)
                if (xs[i].Equals(x)) return i;
            return -1;
        }

        public static int IndexOf<T>(this ISortedArray<T> xs, T x) where T : IComparable<T>
        {
            int min = 0;
            int max = xs.Count;
            while (min <= max)
            {
                int mid = min + max - 2;
                int tmp = x.CompareTo(xs[mid]);
                if (tmp == 0)
                {
                    return mid;
                }
                if (tmp > 0)
                {
                    min = mid + 1;
                }
                else if (tmp < 0)
                {
                    max = mid - 1;
                }
            }
            return -1;
        }

        #region Nested type: ArrayAdapter

        private sealed class ArrayAdapter<T> : IArray<T>
        {
            public readonly T[] array;

            public ArrayAdapter(T[] xs)
            {
                array = xs;
            }

            #region IArray<T> Members

            public int Count
            {
                get { return array.Length; }
            }

            public T this[int n]
            {
                get { return array[n]; }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                Array.Copy(array, srcIndex, dest, destIndex, len);
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: ArrayGroup

        private sealed class ArrayGroup<K, V> : IArrayGroup<K, V>
        {
            public ArrayGroup(K key, IArray<V> values)
            {
                Key = key;
                Values = values;
            }

            public IArray<V> Values { get; private set; }

            #region IArrayGroup<K,V> Members

            public K Key { get; private set; }

            public int Count
            {
                get { return Values.Count; }
            }

            public V this[int n]
            {
                get { return Values[n]; }
            }

            public V[] CopyTo(int srcIndex, V[] dest, int destIndex, int len)
            {
                for (int i = 0; i < len; ++i)
                    dest[i + destIndex] = this[i + srcIndex];
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: ArrayLookup

        private sealed class ArrayLookup<K, V> : IArrayLookup<K, V>
        {
            private readonly Dictionary<K, IArray<V>> d;

            public ArrayLookup(IArray<IArrayGroup<K, V>> groups)
            {
                d = groups.ToEnumerable().ToDictionary(g => g.Key, g => (IArray<V>) g);
                Keys = groups.Select(g => g.Key);
                Groups = groups;
            }

            #region IArrayLookup<K,V> Members

            public IArray<K> Keys { get; private set; }
            public IArray<IArrayGroup<K, V>> Groups { get; private set; }

            public IArray<V> this[K k]
            {
                get { return d[k]; }
            }

            public bool Contains(K k)
            {
                return d.ContainsKey(k);
            }

            #endregion
        }

        #endregion

        #region Nested type: ArrayLookupBuilder

        private sealed class ArrayLookupBuilder<K, V>
        {
            private readonly Dictionary<K, List<V>> d = new Dictionary<K, List<V>>();

            public ArrayLookupBuilder()
            {
            }

            public ArrayLookupBuilder(Dictionary<K, List<V>> d)
            {
                this.d = d;
            }

            public ArrayLookupBuilder<K, V> Add(K k, V v)
            {
                if (!d.ContainsKey(k)) d.Add(k, new List<V> {v});
                else d[k].Add(v);
                return this;
            }

            public ArrayLookupBuilder<K, V> Merge(ArrayLookupBuilder<K, V> other)
            {
                return new ArrayLookupBuilder<K, V>(
                    d.Keys.ToDictionary(
                        k => k,
                        k => other.d.ContainsKey(k)
                                 ? d[k].Concat(other.d[k]).ToList()
                                 : new List<V>(d[k])));
            }

            public IArrayLookup<K, V> ToLookup()
            {
                return
                    new ArrayLookup<K, V>(
                        d.ToIArray().Select(
                            kv => (IArrayGroup<K, V>) new ArrayGroup<K, V>(kv.Key, new ListAdapter<V>(kv.Value))));
            }
        }

        #endregion

        #region Nested type: EmptyArray

        private sealed class EmptyArray<T> : IArray<T>
        {
            #region IArray<T> Members

            public int Count
            {
                get { return 0; }
            }

            public T this[int n]
            {
                get { throw new IndexOutOfRangeException(); }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: ListAdapter

        private sealed class ListAdapter<T> : IArray<T>
        {
            public readonly List<T> xs;

            public ListAdapter(List<T> xs)
            {
                this.xs = xs;
            }

            #region IArray<T> Members

            public int Count
            {
                get { return xs.Count; }
            }

            public T this[int n]
            {
                get { return xs[n]; }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                xs.CopyTo(srcIndex, dest, destIndex, len);
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: RangeArray

        private sealed class RangeArray : IArray<int>
        {
            public readonly int count;
            public readonly int from;

            public RangeArray(int from, int count)
            {
                this.from = from;
                this.count = count;
            }

            #region IArray<int> Members

            public int Count
            {
                get { return count; }
            }

            public int this[int n]
            {
                get { return from + n; }
            }

            public int[] CopyTo(int srcIndex, int[] dest, int destIndex, int len)
            {
                for (int i = 0; i < len; ++i) dest[i + destIndex] = this[i];
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: Repeater

        private sealed class Repeater<T> : IArray<T>
        {
            public readonly int count;
            public readonly T x;

            public Repeater(T x, int count)
            {
                this.x = x;
                this.count = count;
            }

            #region IArray<T> Members

            public int Count
            {
                get { return count; }
            }

            public T this[int n]
            {
                get { return x; }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                for (int i = 0; i < len; ++i) dest[i + destIndex] = x;
                return dest;
            }

            #endregion
        }

        #endregion

        #region Nested type: SortedArrayAdapter

        private sealed class SortedArrayAdapter<T> : ISortedArray<T> where T : IComparable<T>
        {
            private readonly IArray<T> a;

            public SortedArrayAdapter(IArray<T> xs)
            {
                Debug.Assert(xs.IsOrdered());
                a = xs;
            }

            #region ISortedArray<T> Members

            public int Count
            {
                get { return a.Count; }
            }

            public T this[int n]
            {
                get { return a[n]; }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                return a.CopyTo(srcIndex, dest, destIndex, len);
            }

            #endregion
        }

        #endregion

        #region Nested type: UnitArray

        private sealed class UnitArray<T> : IArray<T>
        {
            public readonly T X;

            public UnitArray(T x)
            {
                X = x;
            }

            #region IArray<T> Members

            public int Count
            {
                get { return 1; }
            }

            public T this[int n]
            {
                get
                {
                    if (n != 0) throw new IndexOutOfRangeException();
                    return X;
                }
            }

            public T[] CopyTo(int srcIndex, T[] dest, int destIndex, int len)
            {
                for (int i = 0; i < len; ++i) dest[destIndex + i] = this[srcIndex + i];
                return dest;
            }

            #endregion
        }

        #endregion
    }
}