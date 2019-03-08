using System;

namespace HBD.EfCore.Extensions.Internal
{
    internal class EntityMappingService
    {
        public EntityMappingExtension EntityMapping { get; }

        public EntityMappingService(EntityMappingExtension entityMapping) 
            => EntityMapping = entityMapping ?? throw new ArgumentNullException(nameof(entityMapping));
    }
}
