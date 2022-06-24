using AspNetCore_WebAPI_w_UnitTests.Controllers;
using AspNetCore_WebAPI_w_UnitTests.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace ControllerUnitTests_xUnit
{
    public class ValuesControllerTests
    {
        [Fact]
        public void TestMethod1()
        {
            // ARRANGE
            IMyModel mockRepository = new MockRepository();
            var controller = new ValuesController(mockRepository);

            // ACT
            var result = controller.Get();

            // ASSERT
            //var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(result.Value);
            var Values = model.GetEnumerator();
            Values.MoveNext();
            Assert.Equal("test one", Values.Current);
            Values.MoveNext();
            Assert.Equal("test two", Values.Current);
            Values.MoveNext();
            Assert.Equal("test three", Values.Current);
        }
    }

    internal class MockRepository : IMyModel
    {
        public IEnumerable<string> Get()
        {
            yield return "test one";
            yield return "test two";
            yield return "test three";
        }
    }
}
