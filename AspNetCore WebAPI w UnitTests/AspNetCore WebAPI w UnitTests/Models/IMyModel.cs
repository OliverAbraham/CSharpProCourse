using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_WebAPI_w_UnitTests.Models
{
    public interface IMyModel
    {
        IEnumerable<string> Get();
    }
}
