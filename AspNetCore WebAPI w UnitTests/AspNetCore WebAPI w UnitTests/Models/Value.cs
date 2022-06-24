using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_WebAPI_w_UnitTests.Models
{
    public class Value
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Value(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
