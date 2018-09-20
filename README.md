# HBD.EntityFrameworkCore.Extensions

## Introduction

If working with EntityFrameworkCore code fist We need:

1. Define the Entities
2. Apply the mapping of entity to DbContext.

So incase we have so many entities that may cause an issues that some entitis was not added the mapper propetly. So I develop this extention to auto mapp the Entities to a DbContext

Ex:
I have Uuser entity and User entity mapping as below

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

internal class UserMapper : EntityMapper<User>
{
    public override void Map(EntityTypeBuilder<User> builder)
    {
        base.Map(builder);
        builder.HasIndex(u => u.FirstName);
    }
}
```

When register to a DbContext, it will scan all EntityMapper types and map to DbContext accrodingtly.

```csharp
var db = new MyDbContext(new DbContextOptionsBuilder()
            .RegisterEntities(op=>op.FromAssemblies(typeof(MyDbContext).Assembly))
            .Options)
```

However if I define a Address entity without mapper. This entity still being mapped to the DbContext as library will scan all the Entities which is class of `IEntity<TKey>` and mapped to the DbContext by using default `EntityMapper<>` type.

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
                .WithFilter("THE Expression to filter the type scanning")
                .WithDefaultMapperType("YOUR custom EntityMapper type.")
                ).Options)
```

## NOTE

1. The entities MUST be a `IEntity`
2. The Mapper type MUST be a `IEntityMapper`
3. Your DbContext MUST be from `HBD.EntityFrameworkCore.Extensions.DbContext`
