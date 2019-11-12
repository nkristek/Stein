using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Stein.Utility.Tests
{
    public class IEnumerableExtensionsTests
    {
        [Theory]
        [InlineData(new[] { "a", "b", "c" }, 3)]
        [InlineData(new object[0], 0)]
        public void ForEach_generic(IEnumerable<object> input, int expectedResult)
        {
            var result = 0;
            input.ForEach(i => result++);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, 3)]
        [InlineData(new object[0], 0)]
        public void ForEach(IEnumerable input, int expectedResult)
        {
            var result = 0;
            input.ForEach(i => result++);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ForEach_generic_throws_ArgumentNullException()
        {
            IEnumerable<object> enumeration = new[] { "a", "b", "c" };
            void action(object _) { }
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach<object>((IEnumerable<object>)null, action));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach<object>(enumeration, (Action<object>)null));
        }

        [Fact]
        public void ForEach_throws_ArgumentNullException()
        {
            IEnumerable enumeration = new[] { "a", "b", "c" };
            void action(object _) { }
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach((IEnumerable)null, action));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.ForEach(enumeration, (Action<object>)null));
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, 3)]
        [InlineData(new object[0], 0)]
        public void Apply_generic(IEnumerable<object> input, int expectedResult)
        {
            var result = 0;
            Assert.Equal(input, input.Apply(value => result++));
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, 3)]
        [InlineData(new object[0], 0)]
        public void Apply(IEnumerable input, int expectedResult)
        {
            var result = 0;
            Assert.Equal(input, input.Apply(value => result++));
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Apply_generic_throws_ArgumentNullException()
        {
            IEnumerable<object> enumeration = new[] { "a", "b", "c" };
            void action(object _) { }
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Apply<object>((IEnumerable<object>)null, action).Iterate());
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Apply<object>(enumeration, (Action<object>)null).Iterate());
        }

        [Fact]
        public void Apply_throws_ArgumentNullException()
        {
            IEnumerable enumeration = new[] { "a", "b", "c" };
            void action(object _) { }
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Apply((IEnumerable)null, action).Iterate());
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Apply(enumeration, (Action<object>)null).Iterate());
        }
        
        [Fact]
        public void DistinctBy()
        {
            var first = new KeyValuePair<int, string>(1, "test");
            var second = new KeyValuePair<int, string>(2, "test");
            var third = new KeyValuePair<int, string>(3, "test2");
            var sequence = new List<KeyValuePair<int, string>> { first, second, third };
            var expectedDistinctValues = new List<KeyValuePair<int, string>> { first, third };
            var actualDistinctValues = sequence.DistinctBy(kvp => kvp.Value);
            Assert.Equal(expectedDistinctValues, actualDistinctValues);
        }

        [Fact]
        public void DistinctBy_throws_ArgumentNullException()
        {
            IEnumerable<object> enumeration = new[] { "a", "b", "c" };
            object action(object value) => value;
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.DistinctBy((IEnumerable<object>)null, action).Iterate());
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.DistinctBy(enumeration, (Func<object, object>)null).Iterate());
        }

        [Theory]
        [InlineData(new string[0], new string[0], true)]
        [InlineData(new[] { "1" }, new[] { "1" }, true)]
        [InlineData(new[] { "1" }, new[] { "2" }, false)]
        public void SequenceEqual_generic_action(IEnumerable<string> first, IEnumerable<string> second, bool expectedResult)
        {
            bool comparer(string f, string s) => f == s;
            Assert.Equal(expectedResult, first.SequenceEqual<string, string>(second, comparer));
        }

        [Theory]
        [InlineData(new string[0], new string[0], true)]
        [InlineData(new string[] { "1" }, new string[] { "1" }, true)]
        [InlineData(new string[] { "1" }, new string[] { "2" }, false)]
        public void SequenceEqual_action(IEnumerable first, IEnumerable second, bool expectedResult)
        {
            bool comparer(object f, object s) => (string)f == (string)s;
            Assert.Equal(expectedResult, first.SequenceEqual(second, comparer));
        }

        [Fact]
        public void SequenceEqual_generic_action_throws_ArgumentNullException()
        {
            IEnumerable<string> enumeration = new[] { "a", "b", "c" };
            bool comparer(string f, string s) => f == s;
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual<string, string>((IEnumerable<string>)null, enumeration, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual<string, string>(enumeration, (IEnumerable<string>)null, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual<string, string>(enumeration, enumeration, (Func<string, string, bool>)null));
        }

        [Fact]
        public void SequenceEqual_action_throws_ArgumentNullException()
        {
            IEnumerable enumeration = new[] { "a", "b", "c" };
            bool comparer(object f, object s) => (string)f == (string)s;
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual(null, enumeration, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual(enumeration, null, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual(enumeration, enumeration, (Func<object, object, bool>)null));
        }

        private class StringComparer : IEqualityComparer, IEqualityComparer<string>
        {
            public new bool Equals(object x, object y)
            {
                return (string)x == (string)y;
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }

            public bool Equals(string x, string y)
            {
                return x == y;
            }

            public int GetHashCode(string obj)
            {
                throw new NotImplementedException();
            }
        }

        [Theory]
        [InlineData(new string[0], new string[0], true)]
        [InlineData(new[] { "1" }, new[] { "1" }, true)]
        [InlineData(new[] { "1" }, new[] { "2" }, false)]
        public void SequenceEqual_generic_equalitycomparer(IEnumerable<string> first, IEnumerable<string> second, bool expectedResult)
        {
            var comparer = new StringComparer();
            Assert.Equal(expectedResult, first.SequenceEqual<string>(second, comparer));
        }

        [Theory]
        [InlineData(new string[0], new string[0], true)]
        [InlineData(new string[] { "1" }, new string[] { "1" }, true)]
        [InlineData(new string[] { "1" }, new string[] { "2" }, false)]
        public void SequenceEqual_equalitycomparer(IEnumerable first, IEnumerable second, bool expectedResult)
        {
            var comparer = new StringComparer();
            Assert.Equal(expectedResult, first.SequenceEqual(second, comparer));
        }

        [Fact]
        public void SequenceEqual_generic_equalitycomparer_throws_ArgumentNullException()
        {
            IEnumerable<string> enumeration = new[] { "a", "b", "c" };
            var comparer = new StringComparer();
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual<string>((IEnumerable<string>)null, enumeration, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual<string>(enumeration, (IEnumerable<string>)null, comparer));
            IEnumerableExtensions.SequenceEqual<string>(enumeration, enumeration, (IEqualityComparer<string>)null);
        }

        [Fact]
        public void SequenceEqual_equalitycomparer_throws_ArgumentNullException()
        {
            IEnumerable enumeration = new[] { "a", "b", "c" };
            var comparer = new StringComparer();
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual(null, enumeration, comparer));
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.SequenceEqual(enumeration, null, comparer));
            IEnumerableExtensions.SequenceEqual(enumeration, enumeration, (IEqualityComparer)null);
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, 3)]
        [InlineData(new object[0], 0)]
        public void Iterate(IEnumerable input, int expectedResult)
        {
            var result = 0;
            var applied = input.Apply(value => result++);
            Assert.Equal(0, result);
            applied.Iterate();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Iterate_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Iterate(null));
        }

        [Fact]
        public void Loop_generic_empty_returns_empty()
        {
            Assert.Empty(Enumerable.Empty<object>().Loop());
        }

        [Fact]
        public void Loop_empty_returns_empty()
        {
            Assert.Empty(((IEnumerable)Enumerable.Empty<object>()).Loop());
        }

        [Fact]
        public void Loop_generic_not_empty_returns_looping_enumeration()
        {
            var obj = new object();
            var enumeration = Enumerable.Repeat(obj, 1).Loop();
            using (var enumerator = enumeration.GetEnumerator())
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(obj, enumerator.Current);
                Assert.True(enumerator.MoveNext());
                Assert.Equal(obj, enumerator.Current);
            }
        }

        [Fact]
        public void Loop_not_empty_returns_looping_enumeration()
        {
            var obj = new object();
            var enumeration = ((IEnumerable)Enumerable.Repeat(obj, 1)).Loop();
            var enumerator = enumeration.GetEnumerator();
            try
            {
                Assert.True(enumerator.MoveNext());
                Assert.Equal(obj, enumerator.Current);
                Assert.True(enumerator.MoveNext());
                Assert.Equal(obj, enumerator.Current);
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        [Fact]
        public void Loop_generic_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Loop((IEnumerable)null).Iterate());
        }

        [Fact]
        public void Loop_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Loop((IEnumerable<object>)null).Iterate());
        }

        [Theory]
        [InlineData(new[] { "a" }, true)]
        [InlineData(new object[0], false)]
        public void Any(IEnumerable input, bool expectedResult)
        {
            Assert.Equal(expectedResult, input.Any());
        }

        [Fact]
        public void Any_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IEnumerableExtensions.Any(null));
        }
    }
}
