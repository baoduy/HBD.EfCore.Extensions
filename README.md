# HBD.EntityFrameworkCore.Extensions
[![Build Status](https://steven2412.visualstudio.com/HBD/_apis/build/status/HBD.EntityFrameworkCore.Extensions-GitSync)](https://steven2412.visualstudio.com/HBD/_build/latest?definitionId=81)

## Introduction

If working with EntityFrameworkCore code fist We need:

1. Define the Entities
2. Apply the mapping of the entities to DbContext.

So in case, we have so many entities that may cause an issue that some entities has not added the mapper properly. So I develop this extension to automap the Entities to a DbContext

Ex:
I have User entity and User entity mapping as below

```Csharp
public class User : AuditEntity
{
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; } = new HashSet<Address>();
}

internal class UserMapper : EntityTypeConfiguration<User>
{
    public override void Map(EntityTypeBuilder<User> builder)
    {
        base.Map(builder);
        builder.HasIndex(u => u.FirstName);
    }
}
```

When register to a DbContext, it will scan all EntityTypeConfiguration types and map to DbContext accordingly.

```csharp
var db = new MyDbContext(new DbContextOptionsBuilder()
            .RegisterEntities(op=>op.FromAssemblies(typeof(MyDbContext).Assembly))
            .Options)
```

However, if I define an Address entity without provided a mapper. This entity still being mapped to the DbContext as the Extension will scan all the Entities which is a class of `IEntity<TKey>` and mapped to the DbContext by using the default `EntityTypeConfiguration<>` type.

```csharp
public class Address: Entity<int>
{
    [Required]
    [MaxLength(256)]
    public string Street { get; set; }

    [ForeignKey("Address_User")]
    public long UserId { get; set; }

    public virtual User User { get; set; }
}
```

If you want to customize the default EntityMapper you can overwrite it via the register option.

```csharp
var db = new MyDbContext(new DbContextOptionsBuilder()
            .RegisterEntities(op=>op.FromAssemblies("YOUR ENTITIES ASSEMBLIES")
                .WithFilter("The addition filter will be applied when scan the entities from  ASSEMBLIES")
                .WithDefaultMapperType("YOUR custom IEntityTypeConfiguration<> type.")
                ).Options)
```

## NOTES

1. The entities MUST be an `IEntity<>`
2. The Mapper type MUST be an `IEntityTypeConfiguration<>`
3. Your DbContext MUST be from `HBD.EntityFrameworkCore.Extensions.DbContext`

## CONCLUSIONS

By using this library you just need to define your Entities and register to DbContextOptionsBuilder they will be mapped automatically.
If some Db configuration is not possible to be done via DataAnnotations, just define an internal (private) `EntityTypeConfiguration` class for that entity they will be mapped either
