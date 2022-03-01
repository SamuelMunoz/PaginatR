using System.Collections.Generic;
using FluentAssertions;
using PaginatR.Contracts;
using Xunit;

namespace PaginatR.Tests;

public class PaginationResponseTests
{
    private PaginationResponse<TestModel>? _sut;
    private readonly List<TestModel> _list;

    public PaginationResponseTests()
    {
        _list = TestingData.Data().Generate(50);
    }

    [Fact]
    public void Response_DataShouldBeTheSame_WhenInitialized()
    {
        // act
        _sut = new(_list, 1, 50, 1, false, false);
        
        // assert
        _sut.Data.Should().NotBeNull().And.BeOfType<List<TestModel>>().And.HaveCount(50);
    }
}
