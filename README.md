# HBD.EntityFrameworkCore.Extensions
[![Build Status](https://steven2412.visualstudio.com/HBD/_apis/build/status/HBD.EntityFrameworkCore.Extensions-GitSync)](https://steven2412.visualstudio.com/HBD/_build/latest?definitionId=81)

## Introduction

As you know Entity Framework (EF) Core is a lightweight, extensible, and cross-platform version of the popular Entity Framework data access technology. 
However, in order to make the EF work, We need to define, config a few things below:

1. Define the Entities
```csharp
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{get;set;}

    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [MaxLength(256)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(256)]
    public string LastName { get; set; }
}
```
2. Define Mappers
```csharp
internal class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.FirstName);
    }
}
```
3. Define DbContext and add the Mapper in.
```csharp
public partial class MyContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapper());
    }
}
```

Let see, if we have a hundred Entities, we need to do all steps above a hundred times and the third step is an important step as without applying the configuration to the DbContext we are not able to use those entities.
To speed up the development process and cut down the manual works, I developed this extension for EF Core which allows to scan and apply the IEntityTypeConfiguration from assemblies to DbContext automatically.

## How HBD.EntityFrameworkCore.Extensions Works.

### 1. Generic Entity Type Configuration
Let see, if we are 2 numbers of entities which have the same Entity Type Configuration as almost of configuration can be done via DataAnnotations. However, we still need to define a class from `IEntityTypeConfiguration` for every entity and add into `OnModelCreating` of DbContext. 
However, if using this extension you can define a generic EntityTypeConfiguration that define the configuration for all basic entities which extension will scan it from the Assembly and add into `OnModelCreating` automatically.

Sample code: Define a generic Entity Type Configuration

```csharp
//1. Define BaseEntity
//The recommendation, you should define a BaseEntity on your project which includes all basic properties which will be applied for all entities on the project.
//The Extension provides a few generic class which help you to do the things faster.
public abstract class BaseEntity: Entity {}

//2. Define Generic Entity Type Configuration
internal class MyEntityTypeConfiguration<TEntity> : EntityTypeConfiguration<TEntity> where TEntity: BaseEntity
{
    public void Map(EntityTypeBuilder<TEntity> builder){ /*Apply the configuration here for generic entities*/ }
}

//3. Switch from Microsoft.EntityFrameworkCore.DbContext to  HBD.EntityFrameworkCore.Extensions.DbContext
public partial class MyContext : Microsoft.EntityFrameworkCore.DbContext
{
   //No need to override the OnModelCreating(ModelBuilder modelBuilder)
}

//4. Apply the configuration when App Start
//Normally, the DbContext will be registered with Dependency Injection when App start. So below is a sample code to register the assembles to the extensions as well
var db = new MyDbContext(new DbContextOptionsBuilder().RegisterEntities(op=>op.FromAssemblies(typeof(MyDbContext).Assembly)).Options)
```

Run the application and show the result, All the entities had been mapped to the DbContext automatically with any further codes.
Now, you want to add new entities? Only entity classes need to be added no more Entity Type Configuration, no more OnModelCreating mapping both had been automated.

### 2. Specific Entity Type Configuration
But, The life is not easy and for some special entities, the generic my not able to adopt all the requirement of the entities, in this case, it requires a specific Entity Type Configuration to be defined.

```csharp
//1. Specific Configuration for User Entity
internal class UserTypeConfiguration : EntityTypeConfiguration<User>
{
    public void Map(EntityTypeBuilder<User> builder){ /*Apply the configuration here for the User entity*/ }
}
```
That's all, You just need to define the specific EntityTypeConfiguration for those special entities. The extension is smart enough to scan and all them to the DbContext and obviously, it will apply the Generic Entity Type Configuration to those others entities.

### 3. Static Data Loading
Usually, In the application, We also have some static data tables which shall be initialized when creating the database. EF Core proved a `HasData` method allow us to define the static data for particular entities. But, again the static data need to be provided during `OnModelCreating`.
Instead of combined to together with  Entity Type Configuration, This extension provides a new generic interface named `IDataSeedingConfiguration<>` allow you to define the Static Data for an entity and this definition will be scan and applied to the DbContext at runtime automatically.

```csharp
//1. Define the Data Seeding
internal class AccountDataSeeding: IDataSeedingConfiguration<Account>{
    public TEntity[] Data => new [
        new Account {UserName = "admin", Password="123"}
    ];
}
```

## RECOMMENDATION

1. The entities SHOULD be an implemente of `IEntity<>`
2. The Mapper type SHOULD be an implemente of `IEntityTypeConfiguration<>`
3. Your DbContext SHOULD be from implemente of `HBD.EntityFrameworkCore.Extensions.DbContext`

Hope the library useful.
[drunkcoding.net](http://drunkcoding.net)
