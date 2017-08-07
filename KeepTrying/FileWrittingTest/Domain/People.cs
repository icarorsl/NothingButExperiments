using FileWrittingTest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace FileWrittingTest.Domain
{
    public enum MaritalStatus
    {
        Single,
        Married
    }

    public sealed class People : IBaseModel
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool? AllowRegistration { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

        public People Partner { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, Surname).Trim();
            }
        }
        public bool IsUnder18
        {
            get
            {

                return DateTime.Today.Age(DateOfBirth.Value) < 18;
            }
        }

        public bool IsUnder16
        {
            get
            {
                return DateTime.Today.Age(DateOfBirth.Value) < 16;
            }
        }
    }
}
