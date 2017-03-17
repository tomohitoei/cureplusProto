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

        Try
            Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Entity.Mail)))
            Using sr As New IO.StreamReader("mailtest.txt")
                _ml = DirectCast(serializer.Deserialize(sr), List(Of Entity.Mail))
            End Using
        Catch ex As Exception

        End Try
        'ml.Add(New Entity.Mail() With {.Sender = 1, .Title = "test", .Content = "hoge"})
        'ml.Add(New Entity.Mail() With {.Sender = 2, .Title = "test", .Content = "hoge"})
        'ml.Add(New Entity.Mail() With {.Sender = 3, .Title = "test", .Content = "hoge"})
        'ml.Add(New Entity.Mail() With {.Sender = 1, .Title = "test", .Content = "hoge"})
        'Try
        '    'XmlSerializerオブジェクトを作成
        '    'オブジェクトの型を指定する
        '    Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Entity.Mail)))
        '    '書き込むファイルを開く（UTF-8 BOM無し）
        '    Dim sw As New System.IO.StreamWriter(
        '    "d:\mailtest.txt", False, New System.Text.UTF8Encoding(False))
        '    'シリアル化し、XMLファイルに保存する
        '    serializer.Serialize(sw, ml)
        '    'ファイルを閉じる
        '    sw.Close()
        'Catch ex As Exception
        'End Try

    End Sub

    Private _ml As New List(Of Entity.Mail)


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

        Dim setCharacter = Sub(id As Integer, mi As MailItem)
                               Select Case id
                                   Case 1
                                       mi.CharacterIcon = CreateImageSource(My.Resources._02_021_ひめ_アイコン)
                                       mi.CharacterName = "ひめ"
                                   Case 2
                                       mi.CharacterIcon = CreateImageSource(My.Resources._01_011_響_アイコン)
                                       mi.CharacterName = "響"
                               End Select
                           End Sub
        For i = 0 To _ml.Count - 1
            Dim mi1 As New MailItem
            setCharacter(_ml(i).Sender, mi1)
            mi1.Title = _ml(i).Title
            mi1.Content = _ml(i).Content
            mi1.ReceivedDate = New Date(2017, 3, i + 1)
            Dim mli1 = New MailListItem() With {.DataContext = mi1}
            listBox.Items.Insert(0, mli1)
        Next

        'For i = 0 To 10
        '    Dim mi1 As New MailItem
        '    mi1.CharacterIcon = CreateImageSource(My.Resources._02_021_ひめ_アイコン)
        '    mi1.CharacterName = "ひめ"
        '    mi1.Title = "聞いて聞いて！"
        '    mi1.Content = "聞いて聞いて！" & vbCrLf & "めぐみがね" & vbCrLf & vbCrLf & "aaaaa"
        '    mi1.ReceivedDate = New Date(2017, 3, i + 1)
        '    Dim mli1 = New MailListItem() With {.DataContext = mi1}
        '    Dim mi2 As New MailItem
        '    mi2.CharacterIcon = CreateImageSource(My.Resources._01_011_響_アイコン)
        '    mi2.CharacterName = "響"
        '    mi2.Title = "今日さ、学校で"
        '    mi2.Content = "AAAAAあああああ" & vbCrLf & "カップケーキいっぱい" & vbCrLf & "aaaaa"
        '    mi2.ReceivedDate = New Date(2017, 3, i + 1)
        '    For j = 0 To 30
        '        mi2.Content &= vbCrLf & j & "長いメール"
        '    Next
        '    mi2.Content &= vbCrLf & "長い行　あああああああああああああああああああああああああああああああああああ"

        '    Dim mli2 = New MailListItem() With {.DataContext = mi2}
        '    listBox.Items.Insert(0, mli1)
        '    listBox.Items.Insert(0, mli2)
        'Next
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
        Dim mli = DirectCast(listBox.Items(i), MailListItem)
        mailContent.DataContext = mli.DataContext
        _mailView.Text = DirectCast(mli.DataContext, MailItem).Content
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
