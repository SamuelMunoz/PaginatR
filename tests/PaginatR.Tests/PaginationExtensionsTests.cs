using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PaginatR.Extensions;
using PaginatR.Requests;
using PaginatR.Responses;
using Xunit;

namespace PaginatR.Tests;

public class PaginationExtensionsTests
{
    private PaginationRequest? _request;
    private readonly IQueryable<TestData> _data;
    private readonly CancellationTokenSource _cts;

    public PaginationExtensionsTests()
    {
        _data = TestingData.Data().Generate(20).AsQueryable();
        _cts = new();
    }

    [Theory]
    [InlineData(1, 10, 2)]
    [InlineData(2, 10, 2)]
    [InlineData(1, 5, 4)]
    public async Task ToPaginated_ShouldReturnThePaginatedResult(int page, int size, int expectedPages)
    {
        // arrange
        _request = new(page, size);

        // act
        var result = await _data.ToPaginatedAsync<TestData, int, TestData>(_request, x => x.Id, _cts.Token);
        
        // assert
        result.Should().NotBeNull().And.BeOfType<PaginationResponse<TestData>>();
        result.Data.Should().BeOfType<List<TestData>>().And.HaveCount(size);
        result.As<PaginationResponse<TestData>>().TotalPages.Should().Be(expectedPages);
    }
}