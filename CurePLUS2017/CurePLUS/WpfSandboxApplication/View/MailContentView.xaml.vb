Public Class MailContentView

    Public Shared ReadOnly Property TextProperty As DependencyProperty =
        DependencyProperty.Register("Text",
                                    GetType(String),
                                    GetType(MailContentView),
                                    New FrameworkPropertyMetadata("メール内容"))

    Private _text As String
    Public Property Text As String
        Get
            Return _text
        End Get
        ' TODO : 依存プロパティへのBindingで値が設定できない原因を調査
        Set(value As String)
            _text = value

            ' TODO : メールの内容を設定（絵文字等の対応）
            _view.Inlines.Clear()
            _view.Inlines.Add(New Label() With {.Content = value})
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        MyBase.OnMouseDown(e)
    End Sub

End Class
