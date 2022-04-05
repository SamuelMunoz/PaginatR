using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PaginatR.Contracts;
using PaginatR.Enums;
using PaginatR.Extensions;
using Xunit;

namespace PaginatR.Tests;

public class PaginationExtensionsTests
{
    private PaginationRequest? _request;
    private readonly IQueryable<TestModel> _data;
    private readonly CancellationTokenSource _cts;

    public PaginationExtensionsTests()
    {
        _data = TestingData.Data().Generate(20).AsQueryable();
        _cts = new();
    }

    [Theory]
    [InlineData(1, 10, 2, false, true)]
    [InlineData(2, 10, 2, true, false)]
    [InlineData(1, 5, 4, false, true)]
    public async Task ToPaginated_ShouldReturnThePaginatedResult(int page, int size, int expectedPages,
        bool hasPrevious, bool hasNext)
    {
        // arrange
        _request = new(page, size);

        // act
        var result = await _data.ToPaginatedAsync<TestModel, int, TestModel>(
            _request, 
            x => x.Id, 
            OrderDirection.Ascending, 
            _cts.Token);
        
        // assert
        result.Should().NotBeNull().And.BeOfType<PaginationResponse<TestModel>>();
        result.Data.Should().BeOfType<List<TestModel>>().And.HaveCount(size);
        result.As<PaginationResponse<TestModel>>().TotalPages.Should().Be(expectedPages);
        result.As<PaginationResponse<TestModel>>().PageSize.Should().Be(size);
        result.As<PaginationResponse<TestModel>>().PageNumber.Should().Be(page);
        result.As<PaginationResponse<TestModel>>().TotalRecords.Should().Be(20);
        result.As<PaginationResponse<TestModel>>().HasPrevious.Should().Be(hasPrevious);
        result.As<PaginationResponse<TestModel>>().HasNext.Should().Be(hasNext);
    }
    
    [Theory]
    [InlineData(1, 10, 2)]
    [InlineData(2, 10, 2)]
    [InlineData(1, 5, 4)]
    public async Task ToPaginatedDirectionDescending_ShouldReturnThePaginatedResult(int page, int size, int expectedPages)
    {
        // arrange
        _request = new(page, size);

        // act
        var result = await _data.ToPaginatedAsync<TestModel, int, TestModel>(_request, 
            x => x.Id, OrderDirection.Descending, _cts.Token);
        
        // assert
        result.Should().NotBeNull().And.BeOfType<PaginationResponse<TestModel>>();
        result.Data.Should().BeOfType<List<TestModel>>().And.HaveCount(size);
        result.As<PaginationResponse<TestModel>>().TotalPages.Should().Be(expectedPages);
        result.As<PaginationResponse<TestModel>>().PageNumber.Should().Be(page);
    }
    
    [Fact]
    public async Task ToPaginatedCancellation_ShouldCancelTheRequest()
    {
        // arrange
        _request = new(1, 15);

        // act
        _cts.Cancel();
        Func<Task> result = () => _data.ToPaginatedAsync<TestModel, int, TestModel>(_request, 
            x => x.Id, OrderDirection.Descending, _cts.Token);
        
        // assert
        await result.Should().ThrowAsync<TaskCanceledException>();
    }
}