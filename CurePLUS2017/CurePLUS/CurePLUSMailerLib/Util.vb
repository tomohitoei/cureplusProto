Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization

Friend Class Util

    Public Shared Sub SetMember(source As CustomAttributeData, target As Object, ignore() As String)
        Dim props As New Dictionary(Of String, PropertyInfo)
        For Each pi In target.GetType().GetProperties
            props.Add(pi.Name, pi)
        Next

        For Each na In source.NamedArguments
            If ignore.Contains(na.MemberName) Then Continue For
            If props.ContainsKey(na.MemberName) Then
                Dim tp = props(na.MemberName)
                tp.SetValue(target, na.TypedValue.Value)
            End If
        Next
    End Sub

    Public Shared Sub setCharacter(id As Integer, mi As MailItem)
        Select Case id
            Case 1
                mi.CharacterIcon = CreateImageSource(My.Resources._02_021_ひめ_アイコン)
                mi.CharacterName = "ひめ"
            Case 2
                mi.CharacterIcon = CreateImageSource(My.Resources._01_011_響_アイコン)
                mi.CharacterName = "響"
        End Select
    End Sub

    Public Shared Function CreateImageSource(source As System.Drawing.Bitmap) As ImageSource
        Dim ms As IO.Stream = New IO.MemoryStream()
        ms.Seek(0, IO.SeekOrigin.Begin)
        source.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Dim [is] = New WriteableBitmap(BitmapFrame.Create(ms))
        ms.Close()
        Return [is]
    End Function

    Public Shared Function CreateImageSource(source As String) As ImageSource
        Dim bmp = DirectCast(My.Resources.ResourceManager.GetObject(source), System.Drawing.Bitmap)
        If IsNothing(bmp) Then Return Nothing
        Return CreateImageSource(bmp)
    End Function

End Class
