using Bongo.Models.ModelValidations;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Bango.Models
{
    [TestFixture]
    public class DateInFutureAttributeTests
    {
        [Test]
        public void DateValidator_InputExpectdDateRange_DateValidity()
        {
            DateInFutureAttribute dateInFutureAttribute = new(() => DateTime.Now);
            var result = dateInFutureAttribute.IsValid(DateTime.Now.AddSeconds(100));

            ClassicAssert.AreEqual(true, result);

        }
        [TestCase(100, ExpectedResult = true)]
        [TestCase(-100, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = false)]
        public bool DateValidator_InputExpectedDateRange_DateValidity_withTestCase(int addTime)
        {
            DateInFutureAttribute dateInfuture =new(() => DateTime.Now);
            return dateInfuture.IsValid(DateTime.Now.AddSeconds(addTime));

        }

        [Test]
        public void DateValidator_NotValidDate_ReturnErrorMessage()
        {
            var result = new DateInFutureAttribute();
            ClassicAssert.AreEqual("Date must be in the future", result.ErrorMessage);

        }


    }
}
