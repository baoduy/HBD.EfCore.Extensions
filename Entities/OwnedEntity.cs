using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    [Owned]
    public class OwnedEntity
    {
        #region Public Properties

        public string FullName => $"{nameof(OwnedEntity)} {Name}";
        public string Name { get; set; }

        [ReadOnly(false)]
        public string NotReadOnly { get; set; }

        [ReadOnly(true)]
        public string ReadOnly { get; set; }

        #endregion Public Properties
    }
}