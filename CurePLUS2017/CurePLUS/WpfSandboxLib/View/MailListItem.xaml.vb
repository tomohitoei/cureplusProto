Public Class MailListItem
    Public Property State As Object
        Get
            Return _state.Content
        End Get
        Set(value As Object)
            _state.Content = value
        End Set
    End Property
End Class
