using DavesUtilities.Reflection.Tests.TypeConverters;
using NUnit.Framework;

namespace DavesUtilities.Reflection.Tests
{
    public class CopyBetweenTypes
    {
        private class DataObject1
        {
            public string Hello { get; set; }

            public string Value1 { get; set; }

            public string Value2 { get; set; }
        }

        private class DataObject2
        {
            public string Hello { get; set; }

            public string Value1 { get; set; }

            public string Value2 { get; set; }
        }


        private class DataObject3
        {
            public string Hello { get; set; }

            public string Value1 { get; set; }
        }


        private class DataObject4
        {
            public int Hello { get; set; }

            public int Value1 { get; set; }

            public int Value2 { get; set; }
        }


        [Test]
        public void SameProperties()
        {
            var obj1 = new DataObject1
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject2();

            ReflectionUtilities.CopyProperties(obj1, obj2);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
            Assert.AreEqual(obj1.Value2, obj2.Value2);
        }

        [Test]
        public void MissingSourceProperties()
        {
            var obj1 = new DataObject3
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
            };

            var obj2 = new DataObject2();

            ReflectionUtilities.CopyProperties(obj1, obj2);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
            Assert.AreEqual(default(string), obj2.Value2);
        }

        [Test]
        public void MissingTargetProperties()
        {
            var obj1 = new DataObject1
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject3();

            ReflectionUtilities.CopyProperties(obj1, obj2);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
        }


        [Test]
        public void IgnoredProperties()
        {
            var obj1 = new DataObject1
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject2();

            var settings = new CopySettings();
            settings.IgnoredPropertyNames.Add(nameof(obj1.Value2));

            ReflectionUtilities.CopyProperties(obj1, obj2, settings);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
            Assert.AreEqual(default(string), obj2.Value2);
        }

        [Test]
        public void TypeConverterIsUsed()
        {
            var obj1 = new DataObject1
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject4();

            var settings = new CopySettings();
            settings.TypeConverters.Add(new StringToLengthConverter());

            ReflectionUtilities.CopyProperties(obj1, obj2, settings);

            Assert.AreEqual(obj1.Hello.Length, obj2.Hello);
            Assert.AreEqual(obj1.Value1.Length, obj2.Value1);
            Assert.AreEqual(obj1.Value1.Length, obj2.Value2);
        }

        [Test]
        public void PropertyMapping()
        {
            var obj1 = new DataObject1
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject2();
            var settings = new CopySettings();
            settings.PropertyMapping.Add(nameof(DataObject1.Value1), nameof(DataObject2.Value2));
            settings.PropertyMapping.Add(nameof(DataObject1.Value2), nameof(DataObject2.Value1));

            ReflectionUtilities.CopyProperties(obj1, obj2, settings);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value2, obj2.Value1);
            Assert.AreEqual(obj1.Value1, obj2.Value2);
        }
    }
}