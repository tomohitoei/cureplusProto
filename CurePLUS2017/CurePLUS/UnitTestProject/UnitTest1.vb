Option Strict On

Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Microsoft.CSharp

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub TestMethod1()
        Dim provider As New CSharpCodeProvider()
        Dim cp As New System.CodeDom.Compiler.CompilerParameters()
        'cp.ReferencedAssemblies.Add(IO.Path.Combine("c:\hoge", "YaneGameSdk.dll"))
        Dim source = "
using System;
using System.Collections.Generic;

// コメント
/*
 コメント
*/
public class hoge
{
    public void huga(Dictionary<string,object> param)
    {
        Console.WriteLine(param[""name""]);
        param[""p1""] = (int)param[""p1""] + 1;
    }
}
"
        Dim result = provider.CompileAssemblyFromSource(cp, source)
        'System.Diagnostics.Debug.WriteLine("")

        Dim asm = result.CompiledAssembly
        Dim obj = asm.CreateInstance("hoge")
        Dim mi = obj.GetType().GetMethod("huga")
        Dim param As New Dictionary(Of String, Object)
        param.Add("name", "nya-")
        param.Add("p1", 99)
        Dim rr = mi.Invoke(obj, New Object() {param})

        'Dim d1 = New DateTime(2017, 3, 27, 14, 10, 0)
        'Dim d2 = Now
        'Dim dd = d2 - d1
        'dd.TotalSeconds

    End Sub


End Class