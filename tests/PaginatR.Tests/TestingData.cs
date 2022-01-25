using Bogus;

namespace PaginatR.Tests;

public class TestingData
{
    public static Faker<TestData> Data()
    {
        var data = new Faker<TestData>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
            .RuleFor(x => x.Name, f => f.Name.FullName());

        return data;
    }
}