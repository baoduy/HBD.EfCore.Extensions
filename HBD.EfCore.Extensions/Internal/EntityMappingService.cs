using System;

namespace HBD.EfCore.Extensions.Internal
{
    internal class EntityMappingService
    {
        #region Constructors

        public EntityMappingService(EntityMappingExtension entityMapping)
            => EntityMapping = entityMapping ?? throw new ArgumentNullException(nameof(entityMapping));

        #endregion Constructors

        #region Properties

        public EntityMappingExtension EntityMapping { get; }

        #endregion Properties
    }
}