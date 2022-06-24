using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_WebAPI_w_UnitTests.Models
{
    public class MyModel : IMyModel
    {
        public IEnumerable<string> Get()
        {
            yield return "one";
            yield return "two";
            yield return "three";
        }
    }
}
