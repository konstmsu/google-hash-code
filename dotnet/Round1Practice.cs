using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Round1Practice
{

    static class Extensions
    {
        public static IEnumerable<T> At<T>(this IList<T> values, IEnumerable<int> indexes)
            => indexes.Select(i => values[i]);

        public static IEnumerable<T> At<T>(this IList<T> values, params int[] indexes)
            => values.At(indexes.AsEnumerable());

        public static IEnumerable<int> ToIndexes(this Range range, int count)
        {
            var (offset, length) = range.GetOffsetAndLength(count);
            return Enumerable.Range(offset, length);
        }

        public static IEnumerable<T> At<T>(this IList<T> values, Range range)
            => values.At(range.ToIndexes(values.Count));

        public static T[] Index<T>(this IList<T> values, params Either<int, Range>[] parts)
            => parts.SelectMany(part => part.Map(
                  index => values.At(index),
                  range => values.At(range)
              )).ToArray();
    }

    public class Either<T0, T1>
    {
        readonly T0 v0;
        readonly T1 v1;
        readonly bool is0;

        public Either(T0 v0) { this.v0 = v0; is0 = true; }
        public Either(T1 v1) { this.v1 = v1; }

        public static implicit operator Either<T0, T1>(T0 v0) => new Either<T0, T1>(v0);
        public static implicit operator Either<T0, T1>(T1 v1) => new Either<T0, T1>(v1);

        public T Map<T>(Func<T0, T> map0, Func<T1, T> map1) => is0 ? map0(v0) : map1(v1);
    }

    [TestClass]
    public class NumPyStyleTests
    {
        [TestMethod]
        public void ShouldIndex()
        {
            int[] values = new[] { 10, 11, 12, 13, 14, 15 };
            values.At(0, 2).Should().Equal(10, 12);
            values.At(1..4).Should().Equal(11, 12, 13);
        }

        [TestMethod]
        public void ShouldAllowMixingIndexesAndRanges()
        {
            int[] values = new[] { 10, 11, 12, 13, 14, 15, 16 };
            values.Index(0, 2..4, 6).Should().Equal(10, 12, 13, 16);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        IEnumerable<IEnumerable<int>> GenerateAll(int count)
        {
            var values = ImmutableStack<int>.Empty.Push(0);

            for (var i = 0; i < 10; i++)
            {
                yield return values;

                if (values.IsEmpty)
                    break;

                if (values.Peek() < count - 1)
                    values = values.Push(values.Peek() + 1);
                else
                {
                    values = values.Pop();
                    if (!values.IsEmpty)
                        values = values.Pop(out var v).Push(v + 1);
                }
            }
        }

        [TestMethod]
        public void ShouldGenerateAll()
        {
            GenerateAll(2).Should().BeEquivalentTo(
                new int[0],
                new[] { 0 },
                new[] { 1 },
                new[] { 0, 1 }
            );
        }

        static int[] Solve(int M, int[] p)
        {
            var bestSum = 0;
            var bestResult = new int[0];

            void Gen(IEnumerable<int> head, int startAt)
            {
                var vv = head.ToArray();
                var sum = vv.Sum();
                if (bestSum < sum && sum <= M)
                {
                    bestResult = vv;
                    bestSum = sum;
                }
                for (var i = startAt; i < p.Length; i++)
                    Gen(head.Append(i), startAt + 1);
            }
            Gen(new int[0], 0);

            return bestResult;
        }

        [TestMethod]
        public void ShouldSolve()
        {
            Solve(1, new[] { 1 }).Should().Equal(0);
            Solve(15, new[] { 1, 3, 5, 7, 9 }).Should().Equal(1, 2, 3);
            Solve(17, new[] { 2, 5, 6, 8 }).Should().Equal(0, 2, 3);
        }
    }
}
