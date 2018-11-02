using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.EntityFrameworkCore.Extensions.Tests")]
namespace HBD.EntityFrameworkCore.Extensions.Abstractions
{
    public abstract class Entity<TKey> : IConcurrencyEntity<TKey>
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(TKey id) => Id = id;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get;private set; }

        /// <summary>
        /// The ConcurrencyCheck which using by EF
        /// </summary>
        [ConcurrencyCheck]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public byte[] RowVersion { get; private set; }
    }

    public abstract class Entity : Entity<long>, IConcurrencyEntity
    {
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity() { }

        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(long id) : base(id)
        {
        }
    }

}
