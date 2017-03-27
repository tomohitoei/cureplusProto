Imports System.Xml.Serialization

Namespace Entity

    Public Class Mail
        Public Sender As Integer
        Public Title As String
        Public Content As String
        Public Stamp As String
        Public AdventurePart As String
        Public Script As String
        Public Note As String

        <XmlIgnore()> Public ID As String
        <XmlIgnore()> Public Manager As Object = Nothing
        <XmlIgnore()> Public Received As Boolean = False

        Public Replies As New List(Of Reply)
    End Class

    Public Class Reply
        Public Title As String
        Public Content As String
        Public Script As String

        <XmlIgnore()> Public Manager As Object = Nothing
        <XmlIgnore()> Public Parent As Mail = Nothing
    End Class


End Namespace
