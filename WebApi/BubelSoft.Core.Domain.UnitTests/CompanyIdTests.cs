using System;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Core.Domain.UnitTests
{
    [TestFixture]
    public class CompanyIdTests
    {
        [TestCase(-4)]
        [TestCase(-100)]
        public void Create_ThrowException_IfValueIsNegative(int value)
        {
            Assert.Throws<ArgumentException>(() => { new CompanyId(value);});
        }

        [TestCase(0)]
        [TestCase(100)]
        public void Create_WithGiveValue_IfValueIsNonNegative(int value)
        {
            var created = new CompanyId(value);
            Assert.That(created.Value, Is.EqualTo(value));
        }
    }
}
