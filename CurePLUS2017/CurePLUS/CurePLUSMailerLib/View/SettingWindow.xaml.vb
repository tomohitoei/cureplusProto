Public Class SettingWindow
    Private Sub _cancelButton_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

    Private Sub _acceptButton_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub _clearDataButton_Click(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("ゲームデータをクリアします。この操作は元に戻せません、よろしいですか", "確認", MessageBoxButton.OK, MessageBoxImage.Question) Then
            ClearGameData = True
            DialogResult = True
        End If
    End Sub

    Public Property ClearGameData As Boolean = False

End Class
