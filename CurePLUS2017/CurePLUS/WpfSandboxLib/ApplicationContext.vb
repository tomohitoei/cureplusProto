Public Class ApplicationContext

    Private Sub New()
    End Sub

    Public Shared Function Instance() As ApplicationContext
        Static _i As New ApplicationContext
        Return _i
    End Function

    Private _settings As New Dictionary(Of String, Object)

    Public ReadOnly Property ContainsKey(key As String) As Boolean
        Get
            Return _settings.ContainsKey(key)
        End Get
    End Property

    Public Function GetValue(Of T)(key As String) As T
        Return DirectCast(_settings(key), T)
    End Function

    Public Sub SetValue(Of T)(key As String, value As T)
        If _settings.ContainsKey(key) Then
            _settings(key) = value
        Else
            _settings.Add(key, value)
        End If
    End Sub

    Public Sub SetCurrentTime(key As String)
        SetValue(Of DateTime)(key, Now)
    End Sub
End Class
