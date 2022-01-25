using System.Collections.Generic;
using FluentAssertions;
using PaginatR.Responses;
using Xunit;

namespace PaginatR.Tests;

public class PaginationResponseTests
{
    private PaginationResponse<TestData>? _sut;
    private readonly List<TestData> _list;

    public PaginationResponseTests()
    {
        _list = TestingData.Data().Generate(50);
    }

    [Fact]
    public void Response_DataShouldBeTheSame_WhenInitialized()
    {
        // act
        _sut = new(_list);
        
        // assert
        _sut.Data.Should().NotBeNull().And.BeOfType<List<TestData>>().And.HaveCount(50);
    }
}
