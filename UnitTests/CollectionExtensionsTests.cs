using NUnit.Framework;
using System.Collections.Generic;
using OnPoint.Universal;
using System.Linq;
using System;

namespace OnPoint.UnitTests
{
    public class CollectionExtensionsTests
    {
        private List<string> _NullList = null;
        private List<string> _Hello = new List<string>();
        private List<string> _Space = new List<string>();
        private List<string> _Null = new List<string>();
        private List<string> _Nothing = new List<string>();
        private List<Person> _People = new List<Person>();
        private Person _Bob = new Person(1, "Bob", "Smith", "bob@smith.com", DateTime.Now, 100);
        private Person _Jane = new Person(2, "Jane", "Doe", "jane@doe.com", DateTime.Now, 200);

        [SetUp]
        public void Setup()
        {
            _Hello = new List<string>();
            _Space = new List<string>();
            _Hello.Add("Hello World");
            _Null = new List<string>();
            _Space.Add(" ");
            _Null.Add(null);
        }

        [Test]
        public void AnyNonNulls()
        {
            Assert.IsFalse(_NullList.AnyNonNulls());
            Assert.IsTrue(_Hello.AnyNonNulls());
            Assert.IsTrue(_Space.AnyNonNulls());
            Assert.IsFalse(_Null.AnyNonNulls());
            Assert.IsFalse(_Nothing.AnyNonNulls());
        }

        [Test]
        public void GetNonNulls()
        {
            Assert.AreEqual(_NullList.GetNonNulls()?.FirstOrDefault(), null);
            Assert.AreEqual(_Hello.GetNonNulls().FirstOrDefault(), "Hello World");
            Assert.AreEqual(_Space.GetNonNulls().FirstOrDefault(), " ");
            Assert.AreEqual(_Null.GetNonNulls().FirstOrDefault(), null);
            Assert.AreEqual(_Nothing.GetNonNulls().FirstOrDefault(), null);
        }

        [Test]
        public void AddRemove()
        {
            _Hello.AddIfNotContains("Hello World");
            Assert.AreEqual(_Hello.Count, 1);
            _Hello.AddIfNotContains("FOO");
            Assert.AreEqual(_Hello.Count, 2);
            _Hello.RemoveIfContains("BAR");
            Assert.AreEqual(_Hello.Count, 2);
            _Hello.RemoveIfContains("FOO");
            Assert.AreEqual(_Hello.Count, 1);

            _Hello.AddAll();
            Assert.AreEqual(_Hello.Count, 1);
            _Hello.AddAll("FOO", "BAR");
            Assert.AreEqual(_Hello.Count, 3);
            _Hello.RemoveItems(x => !x.StartsWith("H"));
            Assert.AreEqual(_Hello.Count, 1);
        }

        [Test]
        public void Replace()
        {
            Assert.AreEqual(_People.Count, 0);
            _People.Replace(_Bob, _Jane);
            Assert.AreEqual(_People.Count, 1);
            Assert.AreEqual(_People[0].ID, 2);
            _People.Replace(_Jane, _Bob);
            Assert.AreEqual(_People.Count, 1);
            Assert.AreEqual(_People[0].ID, 1);
        }

        [Test]
        public void MakeArray()
        {
            Person[] people = new Person[] { _Bob };
            Assert.AreEqual(_Bob.MakeArray(), people);
        }

        [Test]
        public void AddToArray()
        {
            Person[] people = new Person[] { _Bob };
            var newPeople = _Jane.AddToArray(ref people);
            Assert.AreEqual(people.Length, 2);
            Assert.AreEqual(people, newPeople);
            Assert.AreSame(people, newPeople);
            Person nobody = null;
            nobody.AddToArray(ref people);
            Assert.AreEqual(people.Length, 3);
        }
    }
}