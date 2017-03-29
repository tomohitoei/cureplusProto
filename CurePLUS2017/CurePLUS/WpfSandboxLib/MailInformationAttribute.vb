<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False)>
Public Class MailInformationAttribute
    Inherits Attribute

    Public Property Sender As Entity.Character.CharacterID
    Public Property Title As String
    Public Property Content As String
    Public Property Stamp As String
    Public Property AdventurePart As String

End Class

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True)>
Public Class ReplyInformationAttribute
    Inherits Attribute
    Public Property Title As String
    Public Property Content As String
    Public Property Parent As Type
End Class
