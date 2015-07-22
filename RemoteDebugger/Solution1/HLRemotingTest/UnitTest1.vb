Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class UnitTest1

    <TestMethod()> Public Sub TestMethod1()
        Dim emoji As New Dictionary(Of String, System.Drawing.Image)()
        emoji.Add("hare", System.Drawing.Image.FromFile("C:\Users\ei\Documents\GitHub\cureplusProto\jsrensyu\hare.png"))
        emoji.Add("ame", System.Drawing.Image.FromFile("C:\Users\ei\Documents\GitHub\cureplusProto\jsrensyu\ame.png"))
        emoji.Add("kumori", System.Drawing.Image.FromFile("C:\Users\ei\Documents\GitHub\cureplusProto\jsrensyu\kumori.png"))

        Dim tr = New HLRemoting.MyTextRenderer("メイリオ", 24)
        tr.EMoji = emoji
        Dim mi = tr.MakeImage(0, My.Settings.MailContent, 400, 1.5)
        mi.Save("d:\image.png", System.Drawing.Imaging.ImageFormat.Png)
        Process.Start("d:\image.png")
    End Sub

End Class