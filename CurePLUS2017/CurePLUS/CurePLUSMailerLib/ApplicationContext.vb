Public Class ApplicationContext

    Private Sub New()
    End Sub

    Public Shared Function Instance() As ApplicationContext
        Static _i As New ApplicationContext
        Return _i
    End Function

    Friend _gameParameters As New Dictionary(Of String, Object)


    Public Function ContainsKey(key As String) As Boolean
        Return _gameParameters.ContainsKey(key)
    End Function

    Public Function GetValue(Of T)(key As String) As T
        Return DirectCast(_gameParameters(key), T)
    End Function

    Public Function GetString(key As String) As String
        Return CStr(_gameParameters(key))
    End Function

    Public Sub SetValue(Of T)(key As String, value As T)
        If _gameParameters.ContainsKey(key) Then
            _gameParameters(key) = value
        Else
            _gameParameters.Add(key, value)
        End If
    End Sub

    Public Sub SetCurrentTime(key As String)
        SetValue(Of DateTime)(key, Now)
    End Sub

    Public Function 初回起動からの経過秒() As Integer
        Return 経過秒("初回起動日時")
    End Function

    Public Function 経過秒(key As String) As Integer
        Return (Now - GetValue(Of DateTime)(key)).TotalSeconds
    End Function
End Class
