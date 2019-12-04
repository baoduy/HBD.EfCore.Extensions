using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domains.Owned
{
    [Owned]
    public class Company
    {
        #region Constructors

        public Company(string name, string uen, string abn, string arbn, string can, LegalType legalType)
        {
            Name = name;
            UEN = uen;
            ABN = abn;
            ARBN = arbn;
            CAN = can;
            LegalType = legalType;
        }

        internal Company()
        {
        }

        #endregion Constructors

        #region Properties

        [MaxLength(50)]
        public string ABN { get; private set; }

        [MaxLength(50)]
        public string ARBN { get; private set; }

        [MaxLength(50)]
        public string CAN { get; private set; }

        public LegalType LegalType { get; private set; }

        [MaxLength(100)]
        public string Name { get; private set; }

        [MaxLength(100)]
        public string UEN { get; private set; }

        #endregion Properties
    }
}