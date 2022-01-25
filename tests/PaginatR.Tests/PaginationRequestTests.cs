using FluentAssertions;
using PaginatR.Requests;
using Xunit;

namespace PaginatR.Tests;

public class PaginationRequestTests
{
    private PaginationRequest? _sut;

    [Fact]
    public void Request_ShouldInitializeWithDefaultValues_WhenInstantiated()
    {
        // arrange
        
        // act
        _sut = new();
        
        // assert
        _sut.PageNumber.Should().Be(1);
        _sut.PageSize.Should().Be(15);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 10)]
    [InlineData(5, 20)]
    [InlineData(15, 20)]
    public void RequestProperties_ShouldBeTheSameAsTheConstructorValues_WhenInitializedWithParams(int page, int size)
    {
        // arrange
        
        // act
        _sut = new(page, size);
        
        // assert
        _sut.PageNumber.Should().Be(page);
        _sut.PageSize.Should().Be(size);
    }
}