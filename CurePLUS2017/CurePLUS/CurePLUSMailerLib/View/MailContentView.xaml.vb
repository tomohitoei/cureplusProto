Public Class MailContentView

    Public Event KickScenario(scenarioName As String)

    Public Shared ReadOnly Property TextProperty As DependencyProperty =
        DependencyProperty.Register("Text",
                                    GetType(String),
                                    GetType(MailContentView),
                                    New FrameworkPropertyMetadata("メール内容"))

    Public Shared ReadOnly Property StampProperty As DependencyProperty =
        DependencyProperty.Register("Stamp",
                                    GetType(String),
                                    GetType(MailContentView),
                                    New FrameworkPropertyMetadata("スタンプ"))

    Public Shared ReadOnly Property AdventurePartProperty As DependencyProperty =
        DependencyProperty.Register("AdventurePart",
                                    GetType(String),
                                    GetType(MailContentView),
                                    New FrameworkPropertyMetadata("アドベンチャーパート"))

    Private _context As ApplicationContext = ApplicationContext.Instance()

    Private _text As String
    Public Property Text As String
        Get
            Return _text
        End Get
        ' TODO : 依存プロパティへのBindingで値が設定できない原因を調査
        Set(value As String)
            _text = value
            'LayoutParts()
        End Set
    End Property

    Private _stamp As String

    Public Property Stamp As String
        Get
            Return _stamp
        End Get
        Set(value As String)
            _stamp = value
            'If String.IsNullOrEmpty(Stamp) Then LayoutParts()
        End Set
    End Property

    Private _adventurePart As String

    Public Property AdventurePart As String
        Get
            Return _adventurePart
        End Get
        Set(value As String)
            _adventurePart = value
            'If String.IsNullOrEmpty(AdventurePart) Then LayoutParts()
        End Set
    End Property

    Private _doneAdventurePart As Boolean = False
    Public Property DoneAdventurePart As Boolean
        Get
            Return _doneAdventurePart
        End Get
        Set(value As Boolean)
            _doneAdventurePart = value
            If Not IsNothing(_button) Then _button.IsEnabled = Not value
        End Set
    End Property

    Private _button As Button = Nothing

    Public Sub LayoutParts()
        ' TODO : 処理に少し時間がかかるので改善方法を検討

        _view.Inlines.Clear()

        Dim sr As New IO.StringReader(Text)
        Do
            Dim buf = sr.ReadLine
            If IsNothing(buf) Then Exit Do
            Dim p As New MailContent.LineParser(buf)
            Do
                Dim t = p.Read()
                If IsNothing(t) Then Exit Do
                Select Case t.Type
                    Case MailContent.LineParser.Token.TokenType.Text
                        _view.Inlines.Add(New TextBlock() With {.Text = t.Value, .TextWrapping = TextWrapping.Wrap})
                    Case MailContent.LineParser.Token.TokenType.Emoji
                        _view.Inlines.Add(New Image() With {.Source = Util.CreateImageSource(t.Value), .Width = 24, .Height = 24})
                    Case MailContent.LineParser.Token.TokenType.EmbeddedValue
                        If _context.ContainsKey(t.Value) Then
                            _view.Inlines.Add(New TextBlock() With {.Text = _context.GetString(t.Value), .TextWrapping = TextWrapping.Wrap})
                        Else
                            _view.Inlines.Add(New TextBlock() With {.Text = String.Format("埋め込み文字列が見つかりません:\[{0}]", t.Value), .TextWrapping = TextWrapping.Wrap})
                        End If
                End Select
            Loop
            _view.Inlines.Add(New LineBreak())
        Loop

        Dim sp As New StackPanel()
        sp.Orientation = Orientation.Vertical
        sp.HorizontalAlignment = HorizontalAlignment.Center
        If Not String.IsNullOrEmpty(Stamp) Then
            sp.Children.Add(New Image() With {.Source = Util.CreateImageSource(Stamp), .Width = 256, .HorizontalAlignment = HorizontalAlignment.Center})
        End If
        If Not String.IsNullOrEmpty(AdventurePart) Then
            Dim b = New Button() With {.Content = "デートに行く", .Width = 128, .Height = 24, .HorizontalAlignment = HorizontalAlignment.Center}
            AddHandler b.Click,
                Sub(sender, e)
                    RaiseEvent KickScenario(AdventurePart)
                End Sub
            _button = b
            sp.Children.Add(b)
        End If
        If 0 < sp.Children.Count Then
            _view.Inlines.Add(sp)
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        MyBase.OnMouseDown(e)
    End Sub

End Class
