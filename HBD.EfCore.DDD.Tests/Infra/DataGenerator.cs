using System.Collections.Generic;
using System.Threading.Tasks;
using AutoBogus;
using AutoMapper;
using HBD.EfCore.DDD.Tests.Infra;

internal static class DataGenerator
{
    public static async Task Generate(this ProfileContext @this, int number = 100, bool force = false)
    {
        var profiles = GenerateEntity<HBD.EfCore.DDD.Tests.Infra.Profile>(number);

        foreach (var p in profiles)
        {
            var accounts = GenerateEntity<Account>(number);
            p.AddAccounts(accounts);
        }

        @this.AddRange(profiles);
        await @this.SaveChangesAsync();
    }

    private static List<TEntity> GenerateEntity<TEntity>(int number) => AutoFaker.Generate<TEntity>(number);
}