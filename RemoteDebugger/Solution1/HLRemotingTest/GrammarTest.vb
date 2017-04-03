Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class GrammarTest

    <TestMethod()> Public Sub TestMethod1()
        Dim mg As New HLRemoting.Scripting.MailScriptGrammar()
        Dim p = New Irony.Parsing.Parser(mg)
        Dim pt = p.Parse("a=1+(b-2);")

        ' 数値
        Dim e As New HLRemoting.Scripting.MailScriptExecuter()
        Dim results = e.Execute("2;")
        Assert.AreEqual(1, results.Count)
        Assert.AreEqual(2, results(0))

        results = e.Execute("-2;")
        Assert.AreEqual(1, results.Count)
        Assert.AreEqual(-2, results(0))

        ' 変数
        Dim variables As New Dictionary(Of String, Object)
        variables.Add("hoge.huga", 100)
        e.Variables = variables
        results = e.Execute("hoge.huga;")
        Assert.AreEqual(1, results.Count)
        Assert.AreEqual(100, results(0))

        ' 四則演算
        results = e.Execute("12 + 23; 12-5; 11/5; 7*4;")
        Assert.AreEqual(4, results.Count)
        Assert.AreEqual(35, results(0))
        Assert.AreEqual(7, results(1))
        Assert.AreEqual(2, results(2))
        Assert.AreEqual(28, results(3))

        results = e.Execute(" 3 * (5-2);(3+5)*2;")
        Assert.AreEqual(9, results(0))
        Assert.AreEqual(16, results(1))

        ' 代入
        results = e.Execute("hoge.huga = 101;")
        Assert.AreEqual(1, results.Count)
        Assert.AreEqual(101, e.ModifiedVariables("hoge.huga"))

        results = e.Execute("hoge.huga = 123-99;")
        Assert.AreEqual(1, results.Count)
        Assert.AreEqual(24, e.ModifiedVariables("hoge.huga"))

        results = e.Execute("hoge.huga = 1; hoge.huga += 1+5;")
        Assert.AreEqual(2, results.Count)
        Assert.AreEqual(7, e.ModifiedVariables("hoge.huga"))

        results = e.Execute("hoge.huga = 1; hoge.huga -= 2;")
        Assert.AreEqual(-1, e.ModifiedVariables("hoge.huga"))

        results = e.Execute("hoge.huga = 1; hoge.huga *= 2;")
        Assert.AreEqual(2, e.ModifiedVariables("hoge.huga"))

        results = e.Execute("hoge.huga = 10; hoge.huga /= -2;")
        Assert.AreEqual(-5, e.ModifiedVariables("hoge.huga"))

        ' 論理否定
        results = e.Execute("!1;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("!0;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("!(1+2);")
        Assert.AreEqual(0, results(0))
        results = e.Execute("!(1+2-3);")
        Assert.AreEqual(1, results(0))

        ' 比較・論理演算
        results = e.Execute("1<0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("1>0;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1<=0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("1<=3-2;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1<=3-3;")
        Assert.AreEqual(0, results(0))

        results = e.Execute("(5*6)==(300/10);")
        Assert.AreEqual(1, results(0))
        results = e.Execute("(5*6)!=(300/10);")
        Assert.AreEqual(0, results(0))

        results = e.Execute("0&&0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("0&&1;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("1&&0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("1&&1;")
        Assert.AreEqual(1, results(0))

        results = e.Execute("0||0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("0||1;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1||0;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1||1;")
        Assert.AreEqual(1, results(0))

        results = e.Execute("0^^0;")
        Assert.AreEqual(0, results(0))
        results = e.Execute("0^^1;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1^^0;")
        Assert.AreEqual(1, results(0))
        results = e.Execute("1^^1;")
        Assert.AreEqual(0, results(0))
    End Sub


End Class