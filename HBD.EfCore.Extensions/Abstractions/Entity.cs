using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HBD.EfCore.Extensions.Tests")]

namespace HBD.EfCore.Extensions.Abstractions
{
    public abstract class Entity<TKey> : IConcurrencyEntity<TKey>, IEquatable<Entity<TKey>>
    {
        #region Private Fields

        private readonly string _internalId;

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(TKey id)
        {
            Id = id;
            _internalId = Guid.NewGuid().ToString();
        }

        #endregion Protected Constructors

        #region Public Properties

        public static EqualityComparer<TKey> KeyComparer => EqualityComparer<TKey>.Default;

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; private set; }

        /// <summary>
        /// The ConcurrencyCheck which using by EF
        /// </summary>
        [ConcurrencyCheck]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public byte[] RowVersion { get; private set; }

        #endregion Public Properties

        #region Public Methods

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

        public override int GetHashCode() => (_internalId != null ? _internalId.GetHashCode() : 0);

        #endregion Public Methods

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        return (KeyComparer.GetHashCode(Id) * 397) ^ GetType().Name.GetHashCode();
        //    }
        //}
    }

    public abstract class Entity : Entity<int>, IConcurrencyEntity
    {
        #region Protected Constructors

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core
        /// </summary>
        protected Entity() : this(0)
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Constructor for EF Core using for Data Seeding
        /// </summary>
        protected Entity(int id) : base(id < 0 ? 0 : id)
        {
        }

        #endregion Protected Constructors
    }
}