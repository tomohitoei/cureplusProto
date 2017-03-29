Public Class UserSettings
    Implements ICloneable

    Public Property Username As String = "けんじ"
    Public Property Nickname1 As String = "けんちゃん"
    Public Property Nickname2 As String = "けんけん"

    Private _birthMonth As Integer = 1
    Public Property BirthMonth As Integer
        Get
            Return _birthMonth
        End Get
        Set(value As Integer)
            If value < 1 Or 12 < value Then Throw New ArgumentException("誕生日の月は1～12の範囲で設定できます")
            _birthMonth = value
            BirthDay = BirthDay
        End Set
    End Property

    Private _birthDay As Integer = 1
    Public Property BirthDay As Integer
        Get
            Return _birthDay
        End Get
        Set(value As Integer)
            Dim last = 31
            Select Case BirthMonth
                Case 1, 3, 5, 7, 8, 10, 12 : last = 31
                Case 4, 6, 9, 11 : last = 30
                Case 2 : last = 29
                Case BirthDay : BirthMonth = 1
            End Select
            _birthDay = value
            If _birthDay < 1 Then _birthDay = 1
            If last < _birthDay Then _birthDay = last
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim c As New UserSettings
        For Each prop In GetType(UserSettings).GetProperties
            prop.SetValue(c, prop.GetValue(Me))
        Next
        Return c
    End Function
End Class
