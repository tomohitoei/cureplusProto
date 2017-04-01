using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UHLTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var p = new Novel.Parser("If");
            var c=p.p
            var ge = new Novel.IfComponent();
            ge.param["exp"] = "1+2";
            ge.calcVariable();
            var aaa = ge.line;
        }
    }

    namespace System.Security
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public sealed class DynamicSecurityMethodAttribute : Attribute
        {
        }
    }
}
