using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PaginatR.Contracts;
using PaginatR.Enums;
using PaginatR.Process;
using Xunit;

namespace PaginatR.Tests;

public class PaginationBuilderTests
{
    private PaginationRequest? _request;
    private readonly IQueryable<TestModel> _data;
    private readonly CancellationTokenSource _cts;

    public PaginationBuilderTests()
    {
        _data = TestingData.Data().Generate(20).AsQueryable();
        _cts = new();
    }

    [Fact]
    public async Task GeneratePaginationObject_UsingFluentBuilderApi()
    {
        // arrange
        _request = new(1, 10);
        
        // act
        var result = await PaginationBuilder<TestModel, int, TestModel>
            .CreatePagination()
            .ForQueryable(_data)
            .WithRequest(_request)
            .OrderedBy(x => x.Id)
            .WithDirection(OrderDirection.Ascending)
            .Generate(_cts.Token);

        // assert
        result.Should().NotBeNull().And.BeOfType<PaginationResponse<TestModel>>();
        result.Data.Should().BeOfType<List<TestModel>>().And.HaveCount(10);
        result.As<PaginationResponse<TestModel>>().TotalPages.Should().Be(2);
        result.As<PaginationResponse<TestModel>>().PageSize.Should().Be(10);
        result.As<PaginationResponse<TestModel>>().PageNumber.Should().Be(1);
        result.As<PaginationResponse<TestModel>>().TotalRecords.Should().Be(20);
        result.As<PaginationResponse<TestModel>>().HasPrevious.Should().Be(false);
        result.As<PaginationResponse<TestModel>>().HasNext.Should().Be(true);
    }
}