Public Class SettingWindow
    Private Sub _cancelButton_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub _acceptButton_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub
End Class
