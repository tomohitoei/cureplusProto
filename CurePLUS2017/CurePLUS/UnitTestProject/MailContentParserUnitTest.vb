Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports WpfSandboxLib.MailContent

<TestClass()> Public Class MailContentParserUnitTest

    <TestMethod()> Public Sub TestMethod1()
        Dim hoge As New LineParser("ほげほげふがふが\{icon_name}ぴよぴよ")

        Dim t1 = hoge.Read
        Assert.AreEqual("ほげほげふがふが", t1.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Text, t1.Type)

        Dim t2 = hoge.Read
        Assert.AreEqual("icon_name", t2.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Emoji, t2.Type)

        Dim t3 = hoge.Read
        Assert.AreEqual("ぴよぴよ", t3.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Text, t3.Type)

        Dim t4 = hoge.Read
        Assert.AreEqual(Nothing, t4)
    End Sub

    <TestMethod()> Public Sub TestMethod2()
        Dim hoge As New LineParser("ほげほげふがふが\{icon_name")
        Dim t1 = hoge.Read
        Assert.AreEqual("ほげほげふがふが", t1.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Text, t1.Type)

        Dim t2 = hoge.Read
        Assert.AreEqual("icon_name", t2.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Emoji, t2.Type)

        Dim t4 = hoge.Read
        Assert.AreEqual(Nothing, t4)
    End Sub

    <TestMethod()> Public Sub TestMethod3()
        Dim hoge As New LineParser("ほげほげふがふが\{")
        Dim t1 = hoge.Read
        Assert.AreEqual("ほげほげふがふが", t1.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Text, t1.Type)

        Dim t2 = hoge.Read
        Assert.AreEqual("", t2.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Emoji, t2.Type)

        Dim t4 = hoge.Read
        Assert.AreEqual(Nothing, t4)
    End Sub

    <TestMethod()> Public Sub TestMethod4()
        Dim hoge As New LineParser("")

        Dim t4 = hoge.Read
        Assert.AreEqual(Nothing, t4)
    End Sub

    <TestMethod()> Public Sub TestMethod5()
        Dim hoge As New LineParser("\{icon_name}")

        Dim t2 = hoge.Read
        Assert.AreEqual("icon_name", t2.Value)
        Assert.AreEqual(LineParser.Token.TokenType.Emoji, t2.Type)

        Dim t4 = hoge.Read
        Assert.AreEqual(Nothing, t4)
    End Sub
End Class