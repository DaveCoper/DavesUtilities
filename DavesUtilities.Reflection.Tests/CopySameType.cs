using System.Linq;
using NUnit.Framework;

namespace DavesUtilities.Reflection.Tests
{
    public class CopySameType
    {
        private class DataObject
        {
            public string Hello { get; set; }
            
            public string Value1 { get; set; }
         
            public string Value2 { get; set; }
        }


        [Test]
        public void SimpleCopy()
        {
            var obj1 = new DataObject
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject();

            ReflectionUtilities.CopyProperties(obj1, obj2);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
            Assert.AreEqual(obj1.Value2, obj2.Value2);
        }

        [Test]
        public void IgnoredProperties()
        {
            var obj1 = new DataObject
            {
                Hello = "Hello",
                Value1 = TestContext.CurrentContext.Random.GetString(50),
                Value2 = TestContext.CurrentContext.Random.GetString(50)
            };

            var obj2 = new DataObject();

            var settings = new CopySettings();
            settings.IgnoredPropertyNames.Add(nameof(obj1.Value2));

            ReflectionUtilities.CopyProperties(obj1, obj2, settings);

            Assert.AreEqual(obj1.Hello, obj2.Hello);
            Assert.AreEqual(obj1.Value1, obj2.Value1);
            Assert.AreEqual(default(string), obj2.Value2);
        }
    }
}