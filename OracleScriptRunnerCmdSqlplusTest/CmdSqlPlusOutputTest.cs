using NUnit.Framework;
using OracleScriptRunnerCmdSqlplus;
using System;
using System.Collections.Generic;
using System.Text;

namespace OracleScriptRunnerCmdSqlplusTest
{
    internal class CmdSqlPlusOutputTest
    {
        [Test]
        public void HasNoErrorsTest()
        {
            string output = @"execute bla ORA-01564
foo bar PLS-123
done";

            CmdSqlPlusOutput o = new CmdSqlPlusOutput() { Output = output };

            Assert.IsFalse(o.HasError);
        }

        [Test]
        public void HasErrorsTest()
        {
            string output1 = @"execute bla 
ORA-01564
foo bar PLS-123
done";

            var o1 = new CmdSqlPlusOutput() { Output = output1 };

            string output2 = @"execute bla ORA-01564
PLS-123
done";

            var o2 = new CmdSqlPlusOutput() { Output = output2 };

            Assert.IsTrue(o1.HasError);
            Assert.IsTrue(o2.HasError);
        }
    }
}
