using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sitecore.Strategy.Sanitizer.Readers;

namespace Sitecore.Strategy.Sanitizer.Tests
{
    [TestClass]
    public class DelimitedFileReaderTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void NullPathTest()
        {
            var reader = new DelimitedFileReader(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptyPathTest()
        {
            var reader = new DelimitedFileReader(string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void NoDelimitersPathTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("xxx");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile))
            {
            }
        }

        [TestMethod]
        public void OneDelimiterPathTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("col1,col2");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile, ','))
            {
                Assert.IsNotNull(reader.ColumnPositions);
                Assert.AreEqual(2, reader.ColumnPositions.Length);
                Assert.AreEqual("col1", reader.ColumnPositions[0]);
                Assert.AreEqual("col2", reader.ColumnPositions[1]);
            }
            File.Delete(tempFile);
        }

        [TestMethod]
        public void TwoDelimiterPathTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("col1,col2-col3");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile, ',', '-'))
            {
                Assert.IsNotNull(reader.ColumnPositions);
                Assert.AreEqual(3, reader.ColumnPositions.Length);
                Assert.AreEqual("col1", reader.ColumnPositions[0]);
                Assert.AreEqual("col2", reader.ColumnPositions[1]);
                Assert.AreEqual("col3", reader.ColumnPositions[2]);
            }
            File.Delete(tempFile);
        }

        [TestMethod]
        public void TwoDataRowsTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("col1,col2,col3");
                writer.WriteLine("row1-val1,row1-val2,row1-val3");
                writer.WriteLine("row2-val1,row2-val2,row2-val3");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile, ','))
            {
                var values = reader.ReadValues();
                Assert.IsNotNull(values);
                Assert.AreEqual(3, values.Count);
                Assert.AreEqual("row1-val1", values["col1"]);
                Assert.AreEqual("row1-val2", values["col2"]);
                Assert.AreEqual("row1-val3", values["col3"]);
                //
                values = reader.ReadValues();
                Assert.IsNotNull(values);
                Assert.AreEqual(3, values.Count);
                Assert.AreEqual("row2-val1", values["col1"]);
                Assert.AreEqual("row2-val2", values["col2"]);
                Assert.AreEqual("row2-val3", values["col3"]);
                //
                values = reader.ReadValues();
                Assert.IsNull(values);
            }
            File.Delete(tempFile);
        }

        [TestMethod]
        public void DataRowWithExtraColumnsTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("col1,col2");
                writer.WriteLine("val1,val2,val3");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile, ','))
            {
                var values = reader.ReadValues();
                Assert.IsNotNull(values);
                Assert.AreEqual(2, values.Count);
                Assert.AreEqual("val1", values["col1"]);
                Assert.AreEqual("val2", values["col2"]);
                Assert.IsFalse(values.ContainsKey("col3"));
            }
            File.Delete(tempFile);
        }

        [TestMethod]
        public void DataRowWithMissingColumnTest()
        {
            var tempFile = Path.GetTempFileName();
            using (var writer = new StreamWriter(File.OpenWrite(tempFile)))
            {
                writer.WriteLine("col1,col2,col3");
                writer.WriteLine("val1,val2");
                writer.Close();
            }
            using (var reader = new DelimitedFileReader(tempFile, ','))
            {
                var values = reader.ReadValues();
                Assert.IsNotNull(values);
                Assert.AreEqual(3, values.Count);
                Assert.AreEqual("val1", values["col1"]);
                Assert.AreEqual("val2", values["col2"]);
                Assert.AreEqual(null, values["col3"]);
            }
            File.Delete(tempFile);
        }

    }
}
