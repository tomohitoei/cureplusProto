Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports System.Xml

<Autofac.AttributedComponent.Component()> Public Class TestView

    Private _userSettings As New UserSettings
    Private _mutex As Threading.Mutex = Nothing
    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        Dim createdNew As Boolean = False
        _mutex = New System.Threading.Mutex(True, "cureplusMutex", createdNew)
        If Not createdNew Then
            MessageBox.Show("キュアぷらすがすでに起動しています", "通知", MessageBoxButton.OK, MessageBoxImage.Stop)
            _mutex.Close()
            End
        End If
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
        addfavoriteimage.Source = CreateImageSource(My.Resources.お気に入り追加アイコン)
        replyimage1.Source = CreateImageSource(My.Resources.返信メールアイコン)

        Try
            Dim serializer As New DataContractSerializer(GetType(UserSettings))
            Dim settings As New XmlReaderSettings()
            Dim xw As XmlReader = XmlReader.Create(New IO.StringReader(My.Settings.UserSetting), settings)
            _userSettings = DirectCast(serializer.ReadObject(xw), UserSettings)
            xw.Close()
        Catch ex As Exception
        End Try

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

    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

        Try
            Dim serializer As New DataContractSerializer(GetType(UserSettings))
            Dim settings As New XmlWriterSettings()
            settings.Encoding = New System.Text.UTF8Encoding(False)
            Dim s As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(s, settings)
            serializer.WriteObject(xw, _userSettings)
            xw.Close()
            My.Settings.UserSetting = s.ToString()
            My.Settings.Save()
        Catch ex As Exception
        End Try
        _mutex.Close()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        DragMove()
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Close()
    End Sub

    Private Sub minimize_Click(sender As Object, e As RoutedEventArgs) Handles minimize.Click
        Me.Visibility = Visibility.Hidden
        Dim mv As New MinimizedMailWindow()
        mv.Left = Left - (mv.Width - Width) / 2
        mv.Top = Top - (mv.Height - Height) / 2
        mv.ShowDialog()
        Me.Visibility = Visibility.Visible
    End Sub

    Private Sub listBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim i = listBox.SelectedIndex
        If i < 0 Then Return
        mailContent.DataContext = DirectCast(listBox.Items(i), MailListItem).DataContext
    End Sub

    Private Sub settings_Click(sender As Object, e As RoutedEventArgs)
        Dim sw As New SettingWindow
        sw.DataContext = _userSettings.Clone
        If sw.ShowDialog Then
            _userSettings = DirectCast(sw.DataContext, UserSettings)
        End If
    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        If MessageBox.Show("キュアぷらすを終了します、よろしいですか", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.No Then
            e.Cancel = True
        End If
        MyBase.OnClosing(e)
    End Sub
End Class
