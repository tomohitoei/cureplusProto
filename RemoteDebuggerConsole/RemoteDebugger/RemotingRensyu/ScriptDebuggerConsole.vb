Imports System.Runtime.Remoting

Public Class ScriptDebuggerConsole

    Public Shared Instance As ScriptDebuggerConsole

    Protected Overrides Sub OnLoad(e As EventArgs)
        Instance = Me
        Dim chnl As New Channels.Http.HttpServerChannel(3142)
        Channels.ChannelServices.RegisterChannel(chnl, False)
        RemotingConfiguration.RegisterWellKnownServiceType(GetType(MyHoge), "tekitou", WellKnownObjectMode.SingleCall)
        RemotingConfiguration.RegisterWellKnownServiceType(GetType(MySelecter), "SelectJumpTarget", WellKnownObjectMode.SingleCall)
        MyBase.OnLoad(e)
    End Sub

    Private Class MyHoge
        Inherits MarshalByRefObject
        Implements HLRemoting.IHoge

        Private Sub New()

        End Sub

        Public Function Func(message As String, count As Integer) As HLRemoting.ReturnValues Implements HLRemoting.IHoge.Func
            Dim r = New HLRemoting.ReturnValues()
            Debug.WriteLine(message)
            r.param1 = Now
            r.abort = ScriptDebuggerConsole.Instance.Abort
            ScriptDebuggerConsole.Instance.Abort = False
            ScriptDebuggerConsole.Instance.Invoke(Sub() ScriptDebuggerConsole.Instance.TextBox1.AppendText(message))
            Return r
        End Function
    End Class

    Private Class MySelecter
        Inherits MarshalByRefObject
        Implements HLRemoting.JumpTargetSelecter

        Public Function SelectTarget(labels As List(Of String)) As String Implements HLRemoting.JumpTargetSelecter.SelectTarget
            Return String.Empty
            'Dim f As New JumpTargetSelecterForm()
            'f.ComboBox1.Items.Add("先頭から開始")
            'For Each Label In labels
            '    Debug.WriteLine(Label)
            '    f.ComboBox1.Items.Add(Label.Substring(13, Label.Length - 13 - 3))
            'Next
            'f.ComboBox1.SelectedIndex = 0
            'f.ShowDialog()

            'Dim selected = f.ComboBox1.Items(f.ComboBox1.SelectedIndex)
            'If f.ComboBox1.SelectedIndex = 0 Then selected = String.Empty
            'Return selected
        End Function
    End Class

    Public Abort As Boolean = False

    Private Sub AbortButton_Click(sender As Object, e As EventArgs) Handles AbortButton.Click
        Abort = True
    End Sub
End Class
