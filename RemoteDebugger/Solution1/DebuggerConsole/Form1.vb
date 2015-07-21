Imports System.Runtime.Remoting

Public Class Form1

    Private hoge As HLRemoting.IHoge = Nothing

    Protected Overrides Sub OnLoad(e As EventArgs)
        'Dim chnl As New System.Runtime.Remoting.Channels.Ipc.IpcClientChannel
        Dim chnl As New System.Runtime.Remoting.Channels.Http.HttpChannel()
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(chnl, True)
        'Dim obj As Object = Activator.GetObject(GetType(HLRemoting.IHoge), "ipc://rtest/tekitou")
        Dim obj As Object = Activator.GetObject(GetType(HLRemoting.IHoge), "http://localhost:8080/tekitou")
        Dim refObj As HLRemoting.IHoge = CType(obj, HLRemoting.IHoge)
        Dim rv = refObj.Func("dorya" & Now.ToString(), 1)

        MyBase.OnLoad(e)
    End Sub
End Class
