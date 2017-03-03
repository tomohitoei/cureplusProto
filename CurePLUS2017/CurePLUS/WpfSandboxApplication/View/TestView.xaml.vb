<Autofac.AttributedComponent.Component()> Public Class TestView

    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()
    End Sub

    Private Function CreateImageSource(source As System.Drawing.Bitmap) As ImageSource
        Dim ms As IO.Stream = New IO.MemoryStream()
        ms.Seek(0, IO.SeekOrigin.Begin)
        source.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Dim [is] = New WriteableBitmap(BitmapFrame.Create(ms))
        ms.Close()
        Return [is]
    End Function

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        changeimage.Source = CreateImageSource(My.Resources.切り替えアイコン)
        newmailimage.Source = CreateImageSource(My.Resources.新規メールアイコン)
        favoriteimage.Source = CreateImageSource(My.Resources.お気に入りアイコン)
        mementoimage.Source = CreateImageSource(My.Resources.想い出アイコン)
        settingsimage.Source = CreateImageSource(My.Resources.設定アイコン)
        minimizeimage.Source = CreateImageSource(My.Resources.最小化アイコン)

        DataContext = New With {.AddFavoriteIcon = CreateImageSource(My.Resources.お気に入り追加アイコン),
                                .ReplyIcon = CreateImageSource(My.Resources.返信メールアイコン)}

        For i = 0 To 10
            Dim mi1 As New MailItem
            mi1.CharacterIcon = CreateImageSource(My.Resources._02_021_ひめ_アイコン)
            mi1.CharacterName = "ひめ"
            mi1.Title = "聞いて聞いて！"
            mi1.Content = "聞いて聞いて！" & vbCrLf & "めぐみがね" & vbCrLf & vbCrLf & "aaaaa"
            Dim mli1 = New MailListItem() With {.DataContext = mi1}
            Dim mi2 As New MailItem
            mi2.CharacterIcon = CreateImageSource(My.Resources._01_011_響_アイコン)
            mi2.CharacterName = "響"
            mi2.Title = "今日さ、学校で"
            mi2.Content = "AAAAAあああああ" & vbCrLf & "カップケーキいっぱい" & vbCrLf & "aaaaa"
            For j = 0 To 30
                mi2.Content &= vbCrLf & j & "長いメール"
            Next
            mi2.Content &= vbCrLf & "長い行　あああああああああああああああああああああああああああああああああああ"

            Dim mli2 = New MailListItem() With {.DataContext = mi2}
            listBox.Items.Add(mli1)
            listBox.Items.Add(mli2)
        Next
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        DragMove()
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Close()
    End Sub

    Private Sub minimize_Click(sender As Object, e As RoutedEventArgs) Handles minimize.Click
        Debug.WriteLine("1")
        Me.Visibility = Visibility.Hidden
        Debug.WriteLine("2")
        Dim mv As New MascotWindow()
        Debug.WriteLine("3")
        mv.Left = Left - (mv.Width - Width) / 2
        mv.Top = Top - (mv.Height - Height) / 2
        Debug.WriteLine("4")
        mv.ShowDialog()
        Me.Visibility = Visibility.Visible
    End Sub

    Private Sub listBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim i = listBox.SelectedIndex
        If i < 0 Then Return
        mailContent.DataContext = DirectCast(listBox.Items(i), MailListItem).DataContext
    End Sub
End Class
