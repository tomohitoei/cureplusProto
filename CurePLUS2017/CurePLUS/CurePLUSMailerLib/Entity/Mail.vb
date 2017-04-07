Imports System.Xml.Serialization

Namespace Entity

    Public Class Mail
        Public Property Sender As Character.CharacterID
        Public Property Title As String
        Public Property Content As String
        Public Property Stamp As String
        Public Property AdventurePart As String
        Public Property DoneAdventurePart As Boolean = False

        Public Property ID As String
        Public Property Manager As IMailManager = Nothing
        Public Property Received As Boolean = False

        Public Replies As New List(Of Reply)
        Public SelectedReply As Reply = Nothing
    End Class

End Namespace
