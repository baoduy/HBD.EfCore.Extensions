using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domains.Owned
{
    [Owned]
    public class PersionName
    {
        #region Constructors

        public PersionName(string title, string firstName, string lastName)
        {
            Title = title;
            Firstname = firstName;
            LastName = lastName;
        }

        internal PersionName()
        {
        }

        #endregion Constructors

        #region Properties

        [MaxLength(100)]
        [Required]
        public string Firstname { get; private set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; private set; }

        [MaxLength(10)]
        [Required]
        public string Title { get; private set; }

        #endregion Properties
    }
}