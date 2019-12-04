using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domains.Owned
{
    [Owned]
    public class Address
    {
        #region Constructors

        public Address(string line, string state, string city, string country, string postal)
        {
            Line = line;
            State = state;
            City = city;
            Country = country;
            Postal = postal;
        }

        internal Address()
        {
        }

        #endregion Constructors

        #region Properties

        [MaxLength(50)]
        public string City { get; private set; }

        [MaxLength(50)]
        public string Country { get; private set; }

        [MaxLength(50)]
        public string Line { get; private set; }

        [MaxLength(50)]
        public string Postal { get; private set; }

        [MaxLength(50)]
        public string State { get; private set; }

        #endregion Properties
    }
}