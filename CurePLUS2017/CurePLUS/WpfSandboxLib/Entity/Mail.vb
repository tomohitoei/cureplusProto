﻿Imports System.Xml.Serialization

Namespace Entity

    Public Class Mail
        Public Property Sender As Character.CharacterID
        Public Property Title As String
        Public Property Content As String
        Public Property Stamp As String
        Public Property AdventurePart As String

        Public Property ID As String
        Public Property Manager As IMailManager = Nothing
        Public Property Received As Boolean = False

        Public Replies As New List(Of Reply)
        Public SelectedReply As Reply = Nothing
    End Class

    Public Class Reply
        Public Property Title As String
        Public Property Content As String

        Public Property Manager As IReplyManager = Nothing
        Public Property Parent As Mail = Nothing
    End Class

End Namespace
