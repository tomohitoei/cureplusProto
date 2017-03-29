Public Class ReplyWindow

    'Public Property Mail As Entity.Mail = Nothing

    Public Sub New(replies As List(Of Entity.Reply), selected As Entity.Reply)
        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        If IsNothing(replies) Then
            Debug.Assert(Not IsNothing(selected))
            SelectedReply = selected
            rep1.Content = SelectedReply.Title
            rep1.Tag = SelectedReply
            rep1.IsEnabled = False
            rep2.IsEnabled = False
            rep3.IsEnabled = False
            _accept.IsEnabled = False
            mailContent.Text = SelectedReply.Content
            mailContent.LayoutParts()
        Else
            Dim btns = {rep1, rep2, rep3}
            For i = 0 To 2
                If i < replies.Count Then
                    btns(i).Content = replies(i).Title
                    btns(i).Tag = replies(i)
                Else
                    btns(i).Visibility = Visibility.Hidden
                End If
            Next
        End If
    End Sub

    Private Sub _accept_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub _cancel_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub Window_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        DragMove()
    End Sub

    Public SelectedReply As Entity.Reply = Nothing

    Private Sub rep1_Click(sender As Object, e As RoutedEventArgs)
        Dim btn = DirectCast(sender, Button)
        Dim rep = DirectCast(btn.Tag, Entity.Reply)
        mailContent.Text = rep.Content
        mailContent.LayoutParts()
        SelectedReply = rep
    End Sub
End Class
