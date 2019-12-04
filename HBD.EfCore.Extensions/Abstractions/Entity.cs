using HBD.EfCore.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]

namespace HBD.EfCore.Extensions.Abstractions
{
    /// <summary>
    /// Abstract Entity with Key definition parameter.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<TKey> : IConcurrencyEntity<TKey>, IEquatable<Entity<TKey>>
    {
        #region Fields

        private readonly string _internalId;

        #endregion Fields

        #region Constructors

        protected Entity() => _internalId = Guid.NewGuid().ToString();

        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(TKey id)
        {
            Id = id;
            _internalId = Guid.NewGuid().ToString();
        }

        #endregion Constructors

        #region Properties

        public static EqualityComparer<TKey> KeyComparer => EqualityComparer<TKey>.Default;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreFromUpdate]
        public virtual TKey Id
        {
            get;
            private set;
        }

        /// <summary>
        /// The ConcurrencyCheck which using by EF
        /// </summary>
        [ConcurrencyCheck]
        [Timestamp]
        [Column(Order = 1000)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        public byte[] RowVersion { get; private set; }

        #endregion Properties

        #region Methods

        public static bool operator !=(Entity<TKey> a, Entity<TKey> b) => !(a == b);

        public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public override bool Equals(object obj) => Equals(obj as Entity<TKey>);

        public bool Equals(Entity<TKey> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (KeyComparer.Equals(Id, default) || KeyComparer.Equals(other.Id, default)) return false;

            return KeyComparer.Equals(Id, other.Id);
        }

        public override int GetHashCode() => (_internalId != null ? _internalId.GetHashCode(StringComparison.Ordinal) : 0);

        /// <summary>
        /// Set Id to the primary key
        /// </summary>
        /// <param name="id"></param>
        protected void SetId(TKey id) => Id = id;

        #endregion Methods
    }

    /// <summary>
    /// Default base Entity with Id is int.
    /// </summary>
    public abstract class Entity : Entity<int>, IConcurrencyEntity
    {
        #region Constructors

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity()
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(int id) : base(id)
        {
        }

        #endregion Constructors
    }

    /// <summary>
    /// The Base Entity with Id is Guid
    /// </summary>
    public abstract class EntityGuid : Entity<Guid>, IConcurrencyEntity<Guid>
    {
        #region Constructors

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected EntityGuid() : this(Guid.NewGuid())
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected EntityGuid(Guid id) : base(id)
        {
        }

        #endregion Constructors
    }
}