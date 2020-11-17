using System;
using System.Diagnostics.CodeAnalysis;

namespace OnPoint.UnitTests
{
    public class Person : IEquatable<Person>
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Salary { get; set; }

        public Person(int id, string firstName, string lastName, string emailAddress, DateTime birthDate, decimal salary)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            BirthDate = birthDate;
            Salary = salary;
        }

        public bool Equals([AllowNull] Person other) => ID == other.ID;
    }
}