Imports System.Runtime.Serialization
Imports System.Xml.Serialization

Namespace Entity

    <DataContract()> Public Class Reply
        <DataMember()> Public Property Title As String
        <DataMember()> Public Property Content As String

        <DataMember()> Public Property Manager As IReplyManager = Nothing
        <XmlIgnore()> Public Property Parent As Mail = Nothing
    End Class

End Namespace
