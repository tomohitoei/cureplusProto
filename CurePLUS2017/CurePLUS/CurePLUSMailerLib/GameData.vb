Public Class GameData
    Public UserSettings As New UserSettings

    Public GameParameters As New Dictionary(Of String, Object)

    Public MailTriggerList As New Dictionary(Of String, Entity.Mail)

    Public ReceivedMailList As New List(Of Entity.ReceivedMail)

    Public Sub Clear()
        GameParameters.Clear()
        MailTriggerList.Clear()
        ReceivedMailList.Clear()
    End Sub
End Class
