
using System.Collections.Generic;

namespace DBRandomizer.Tests
{
    public class RandomizerTests
    {
        [Fact]
        public void RandomizeTest()
        {
            //Arrange
            var db = new DataBaseClient("test.db");

            var sut = new Randomizer(db, "table1");

            sut.DataSet.Add("f1", new List<string>());
            sut.DataSet.Add("f2", new List<string>());

            sut.DataSet["f1"].AddRange(new string[] { "string1", "string2", "string3", "string4" });
            sut.DataSet["f2"].AddRange(new string[] { "1", "2", "3", "4", "5" });

            //Act

            sut.Randomize(10);

            //Assert

            ((long)db.ExecuteScalar($"SELECT COUNT(*) FROM {sut.TableName}")).Should().Be(10);
        }

        [Fact]
        public void CombineTest()
        {
            //Arrange
            var db = new DataBaseClient("test.db");

            var sut = new Randomizer(db, "table1");

            sut.DataSet.Add("f1", new List<string>());
            sut.DataSet.Add("f2", new List<string>());

            sut.DataSet["f1"].AddRange(new string[] { "string1", "string2", "string3", "string4" });
            sut.DataSet["f2"].AddRange(new string[] { "1", "2", "3", "4", "5" });

            //Act

            sut.Combine();

            //Assert

            ((long)db.ExecuteScalar($"SELECT COUNT(*) FROM {sut.TableName}")).Should().Be(20);
        }
    }
}
