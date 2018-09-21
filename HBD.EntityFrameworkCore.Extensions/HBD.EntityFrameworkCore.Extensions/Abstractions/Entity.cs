using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public abstract class Entity : Entity<long>, IEntity
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity() { }
    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity()
        {
        }


        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; private set; }

        /// <summary>
        /// The ConcurrencyCheck which using by EF
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [ConcurrencyCheck]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public byte[] RowVersion { get; private set; }
    }
}
