using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BackendTests
{
    public class CalcTests
    {
         [Fact]
         public void Add_When2Integers_ShouldReturnCorrectInteger()
        {
            var result = Calc.Add(1, 1);
            Assert.Equal(result, 2);
        }
    }
}
