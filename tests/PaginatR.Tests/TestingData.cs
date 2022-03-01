using Bogus;

namespace PaginatR.Tests;

public class TestingData
{
    public static Faker<TestModel> Data()
    {
        var data = new Faker<TestModel>()
            .CustomInstantiator(f => 
                new TestModel(f.Random.Int(1, 100), f.Name.FullName())
            );

        return data;
    }
}