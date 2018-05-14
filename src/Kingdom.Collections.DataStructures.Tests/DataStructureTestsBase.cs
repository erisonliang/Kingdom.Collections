﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Collections
{
    using NUnit.Framework;
    using static String;

    [TestFixture]
    public abstract class DataStructureTestsBase<T, TList>
        where TList : class, IList<T>, new()
    {
        public class ItemList : IList<T>
        {
            private readonly IList<T> _list;

            internal ItemList(params T[] items)
            {
                _list = items.ToList();
            }

            private void ListAction(Action<IList<T>> action) => action(_list);

            private TResult ListFunc<TResult>(Func<IList<T>, TResult> func) => func(_list);

            public IEnumerator GetEnumerator() => ListFunc(x => x.GetEnumerator());

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => ListFunc(x => x.GetEnumerator());

            public void Add(T item) => ListAction(x => x.Add(item));

            public void Clear() => ListAction(x => x.Clear());

            public bool Contains(T item) => ListFunc(x => x.Contains(item));

            public void CopyTo(T[] array, int arrayIndex) => ListAction(x => x.CopyTo(array, arrayIndex));

            public bool Remove(T item) => ListFunc(x => x.Remove(item));

            public int Count => ListFunc(x => x.Count);

            public bool IsReadOnly => false;

            public int IndexOf(T item) => ListFunc(x => x.IndexOf(item));

            public void Insert(int index, T item) => ListAction(x => x.Insert(index, item));

            public void RemoveAt(int index) => ListAction(x => x.RemoveAt(index));

            public T this[int index]
            {
                get { return ListFunc(x => x[index]); }
                set { ListAction(x => x[index] = value); }
            }

            /// <summary>
            /// Everything else is plumbing with the inner <see cref="IList{T}"/>. What this
            /// actually does it to inform the unit test runner what the discovered test
            /// signature actually looks like.
            /// </summary>
            /// <returns></returns>
            public override string ToString() => Join(Join(", ", from x in _list select $"{x}"), "{", "}");
        }

        protected TList Subject { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            Subject = new TList();
        }

        [TearDown]
        public virtual void TearDown()
        {
            (Subject as IDisposable)?.Dispose();
            Subject = null;
        }

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }

        protected static TList Verify(TList subject, Action<TList> verify = null)
        {
            verify = verify ?? (x => { });
            Assert.NotNull(subject);
            verify(subject);
            return subject;
        }
    }
}