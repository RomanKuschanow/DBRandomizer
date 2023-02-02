namespace DBRandomizer.Tests;

public class DataBaseTests
{
    [Fact]
    public void GetDBInfoNotNullTest()
    {
        //Arrange
        var sut = new DataBaseClient("test.db");

        //Act
        var info = sut.DBInfo;

        //Assert
        info.Should().NotBeNull();
    }
}
