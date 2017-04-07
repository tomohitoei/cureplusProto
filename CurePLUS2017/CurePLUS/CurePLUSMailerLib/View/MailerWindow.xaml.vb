Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Windows.Threading
Imports System.Xml
Imports Microsoft.CSharp

Public Class MailerWindow

    Private _gameData As New GameData
    Private _mutex As Threading.Mutex = Nothing


    Private _mainLoopTimer As Timers.Timer

    Private _context As ApplicationContext = ApplicationContext.Instance()
    Private _count As Integer = 0
    Private _scripts As New Dictionary(Of String, Reflection.Assembly)
    Private _plugins As New Dictionary(Of String, Reflection.Assembly)
    Private _pluginedTypes As New List(Of Type)

    Public Shared NewMailCount As Integer

    Private _forceClose As Boolean = False

    Private Sub MainLoop(sender As Object, e As Timers.ElapsedEventArgs)
        _mainLoopTimer.Stop()
        Try
            _context.SetValue(Of DateTime)("現在日時", Now)
            _context.SetValue(Of Integer)("累積起動時間", _context.GetValue(Of Integer)("累積起動時間"))

            For Each mi In _gameData.MailTriggerList.Values
                If mi.Received Then Continue For
                If mi.Manager.canReceive(_context) Then
                    Dispatcher.Invoke(Sub() ReceiveMail(mi))
                End If
            Next

            Dim count = 0
            For i = 0 To _mailTitleList.Items.Count - 1
                Dim ii = i
                Dispatcher.Invoke(Sub()
                                      If Not String.IsNullOrEmpty(DirectCast(DirectCast(_mailTitleList.Items(ii), MailListItem).DataContext, MailItem).State) Then count += 1
                                  End Sub)
            Next
            NewMailCount = count
        Finally
            _mainLoopTimer.Start()
        End Try
    End Sub

    Private Function MakeMailItem(rm As Entity.ReceivedMail) As MailListItem
        Dim mail = _gameData.MailTriggerList(rm.ParentID)
        Dim mi As New MailItem
        Util.setCharacter(mail.Sender, mi)
        mi.Title = mail.Title
        mi.Content = mail.Content
        mi.Stamp = mail.Stamp
        mi.AdventurePart = mail.AdventurePart
        mi.ReceivedDate = rm.Timestamp
        mi.DoneAdventurePart = mail.DoneAdventurePart
        mi.Mail = mail
        mail.Received = True
        Dim mli1 = New MailListItem() With {.DataContext = mi, .Tag = rm}
        Return mli1
    End Function

    Private Sub ReceiveMail(mail As Entity.Mail)
        Dim rm As New Entity.ReceivedMail() With {.ParentID = mail.ID, .Read = False, .Timestamp = Now}
        Dim mli1 = MakeMailItem(rm)
        _mailTitleList.Items.Insert(0, mli1)
        _gameData.ReceivedMailList.Add(rm)
    End Sub

    Private Sub ReadPlugins()
        _plugins.Clear()

        ' Pluginsフォルダが無い場合・プラグインのdllが無い場合の対処
        Dim pd = IO.Path.Combine(IO.Directory.GetCurrentDirectory, "Plugins")
        If Not IO.Directory.Exists(pd) Then
            MessageBox.Show("プラグインフォルダが見つかりません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
            _forceClose = True
            Close()
            Return
        End If
        Dim dllList = IO.Directory.GetFiles(pd, "*.dll")
        If 0 = dllList.Count Then
            _forceClose = True
            MessageBox.Show("プラグインのdllファイルがありません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
            Close()
            Return
        End If
        For Each f In dllList
            Dim fi As New IO.FileInfo(f)
            Dim asm = Reflection.Assembly.LoadFrom(f)
            _plugins.Add(fi.Name, asm)
        Next
        _pluginedTypes.Clear()
        For Each plugin In _plugins.Values
            For Each type In plugin.ExportedTypes
                'If type Is GetType(IMailManager) Or type Is GetType(IReplyManager) Then
                _pluginedTypes.Add(type)
                'End If
            Next
        Next
    End Sub

    Private Sub WriteUserSettingInContext()
        _context.SetValue("UserSettings/Username", _gameData.UserSettings.Username)
        _context.SetValue("UserSettings/Nickname1", _gameData.UserSettings.Nickname1)
        _context.SetValue("UserSettings/Nickname2", _gameData.UserSettings.Nickname2)
        _context.SetValue("UserSettings/BirthMonth", _gameData.UserSettings.BirthMonth)
        _context.SetValue("UserSettings/BirthDay", _gameData.UserSettings.BirthDay)
    End Sub

    Private Sub NewGame()
        If Not IsNothing(_mainLoopTimer) Then _mainLoopTimer.Stop()
        Threading.Thread.Sleep(1000)

        mailContent.DataContext = Nothing
        _mailView.Text = String.Empty
        _mailView.Stamp = String.Empty
        _mailView.AdventurePart = String.Empty
        _mailView.LayoutParts()
        _currentMail = Nothing

        _gameData.Clear()
        _mailTitleList.Items.Clear()

        ' プラグインの読み込み
        For Each asm In _plugins.Values
            ' メール・リプライマネージャ作成
            Dim mmList As New Dictionary(Of Type, Entity.Mail)
            Dim rmList As New List(Of IReplyManager)
            For Each typ In asm.GetExportedTypes()
                Try
                    Dim obj = asm.CreateInstance(typ.FullName)
                    If TypeOf obj Is IThreadDataInitializer Then
                        Dim ti = DirectCast(obj, IThreadDataInitializer)
                        ti.Initialize(_context)
                    ElseIf TypeOf obj Is IMailManager Then
                        Dim mmgr = DirectCast(obj, IMailManager)
                        Dim mail As New Entity.Mail
                        Dim al = typ.CustomAttributes()
                        If al.Count = 0 Then Continue For ' 属性の付いていないマネージャはスキップ
                        For Each a In al
                            Util.SetMember(a, mail, {})
                        Next
                        mail.ID = typ.Name
                        mail.Manager = mmgr
                        mmList.Add(typ, mail)
                        _gameData.MailTriggerList.Add(typ.Name, mail)
                    ElseIf TypeOf obj Is IReplyManager Then
                        rmList.Add(DirectCast(obj, IReplyManager))
                    End If
                Catch ex As Exception
                End Try
            Next

            ' リプライとメールを関連付ける
            For Each rmgr In rmList
                Dim al = rmgr.GetType().CustomAttributes()
                Dim target As Type = Nothing
                Dim temp As New With {.Parent = Nothing}
                For Each a In al
                    Util.SetMember(a, temp, {})
                    If Not IsNothing(temp.Parent) Then Exit For
                Next
                If IsNothing(temp.Parent) Then Continue For ' 属性の付いていないマネージャはスキップ
                If mmList.ContainsKey(DirectCast(temp.Parent, Type)) Then
                    Dim reply As New Entity.Reply
                    For Each a In al
                        Util.SetMember(a, reply, {"Parent"})
                    Next
                    reply.Manager = rmgr
                    Dim parent = mmList(DirectCast(temp.Parent, Type))
                    parent.Replies.Add(reply)
                    reply.Parent = parent
                End If
            Next
        Next

        _context.SetValue(Of DateTime)("初回起動日時", Now)
        _context.SetValue(Of Integer)("累積起動時間", 0)
        _context.SetValue(Of Integer)("起動回数", 1)
        WriteUserSettingInContext()

        If Not IsNothing(_mainLoopTimer) Then _mainLoopTimer.Start()
    End Sub

    Private Sub RestoreGame()
        ' リプライへの親メールセット
        For i = 0 To _gameData.MailTriggerList.Count - 1
            Dim mail = _gameData.MailTriggerList.Values(i)
            For Each rep In mail.Replies
                rep.Parent = mail
            Next
        Next
        '受信済みメール一覧
        NewMailCount = 0
        For i = 0 To _gameData.ReceivedMailList.Count - 1
            Dim rm = _gameData.ReceivedMailList(i)
            Dim mli1 = MakeMailItem(rm)
            If rm.Read Then
                mli1.State = ""
                DirectCast(mli1.DataContext, MailItem).State = ""
            Else
                NewMailCount += 1
            End If
            _mailTitleList.Items.Insert(0, mli1)
        Next

    End Sub

    'MoveWindow関数の宣言
    Private Declare Function MoveWindow Lib "user32" Alias "MoveWindow" _
    (ByVal hwnd As IntPtr, ByVal x As Integer, ByVal y As Integer,
    ByVal nWidth As Integer, ByVal nHeight As Integer,
    ByVal bRepaint As Integer) As Integer

    Private Sub KickAdventurePart(scenarioName As String)
        ' アドベンチャーパート用にゲームパラメータをファイルに書き出す
        'Dim ocd = IO.Directory.GetCurrentDirectory()
        Dim tempPath = IO.Path.GetTempPath()
        Dim cd = IO.Directory.GetCurrentDirectory()
        Try
            Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, Object)))
            Dim settings As New XmlWriterSettings()
            settings.Encoding = New System.Text.UTF8Encoding(False)
            Using sw = New IO.StreamWriter(IO.Path.Combine(tempPath, "params.xml"))
                Using xw As XmlWriter = XmlWriter.Create(sw, settings)
                    serializer.WriteObject(xw, _gameData.GameParameters)
                    xw.Close()
                End Using
                sw.Close()
            End Using
        Catch ex As Exception
        End Try

        ' アドベンチャーパート起動
        IO.Directory.SetCurrentDirectory(cd)
        IO.File.WriteAllText(IO.Path.Combine(tempPath, "TargetScenario.txt"), scenarioName & vbCrLf)
        Threading.Thread.Sleep(1000)
        Dim p = Process.Start(IO.Path.Combine(cd, "キュアぷらす スクリプトデバッガ.exe"))

        'アイドル状態になるまで待機
        p.WaitForInputIdle()
        Me.Visibility = Visibility.Hidden
        Threading.Thread.Sleep(200)
        System.Windows.Forms.Application.DoEvents()
        MoveWindow(p.MainWindowHandle, Left, Top, 640, 480, 1)
        p.WaitForExit()
        Threading.Thread.Sleep(200)

        ' ゲームパラメータを更新する
        Try
            Dim serializer As New DataContractSerializer(GetType(Dictionary(Of String, Object)))
            Dim settings As New XmlReaderSettings()
            Using sr = New IO.StreamReader(IO.Path.Combine(tempPath, "params.xml"))
                Using xr As XmlReader = XmlReader.Create(sr, settings)
                    _gameData.GameParameters = DirectCast(serializer.ReadObject(xr), Dictionary(Of String, Object))
                    _context._gameParameters = _gameData.GameParameters
                    xr.Close()
                End Using
                sr.Close()
            End Using
        Catch ex As Exception
        End Try

        _currentMail.Manager.onDoneAdventurePart(_context)
        Me.Activate()

        ' アドベンチャーパート完了フラグの設定
        Dim i = _mailTitleList.SelectedIndex
        Dim mli = DirectCast(_mailTitleList.Items(i), MailListItem)
        Dim mail = DirectCast(mli.DataContext, MailItem)
        _mailView.DoneAdventurePart = True
        mail.DoneAdventurePart = True
        mail.Mail.DoneAdventurePart = True

        Me.Visibility = Visibility.Visible
    End Sub

    Public Sub New()
        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' ゲームデータを復元
        Try
            ReadPlugins()
            AddHandler _mailView.KickScenario, AddressOf KickAdventurePart

            Dim stg As Dictionary(Of String, Object) = Nothing
            Try
                Dim serializer As New DataContractSerializer(GetType(GameData), _pluginedTypes)
                Dim settings As New XmlReaderSettings()
                Dim xw As XmlReader = XmlReader.Create(New IO.StringReader(My.Settings.UserSetting), settings)
                _gameData = DirectCast(serializer.ReadObject(xw), GameData)
                xw.Close()
                stg = _gameData.GameParameters
            Catch ex As Exception
            End Try
retry:
            _context._gameParameters = stg
            If IsNothing(stg) OrElse Not _context._gameParameters.ContainsKey("初回起動日時") OrElse _gameData.MailTriggerList.Count = 0 Then
                NewGame()
            Else
                _context._gameParameters = stg
                Try
                    _context.SetValue(Of Integer)("起動回数", _context.GetValue(Of Integer)("起動回数"))
                    RestoreGame()
                Catch ex As Exception
                    MessageBox.Show("前回起動時のゲーム状態を回復できないのでゲームデータを初期化します", "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
                    _context._gameParameters = Nothing
                    GoTo retry
                End Try
            End If

            If _gameData.MailTriggerList.Count = 0 Then
                MessageBox.Show("メール送信キューにデータがないため、ゲームを起動できません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
                _forceClose = True
                Close()
            End If

            Icon = Util.CreateImageSource(My.Resources.ひめミニキャラ1)
        Catch ex As Exception
            MessageBox.Show(ex.ToString(), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
            Close()
            Return
        End Try

        Dim createdNew As Boolean = False
        _mutex = New System.Threading.Mutex(True, "cureplusMutex", createdNew)
        If Not createdNew Then
            MessageBox.Show("キュアぷらすがすでに起動しています", "通知", MessageBoxButton.OK, MessageBoxImage.Stop)
            _mutex.Close()
            Close()
            Return
        End If

        _mainLoopTimer = New Timers.Timer(1000)
        AddHandler _mainLoopTimer.Elapsed, AddressOf MainLoop
        _mainLoopTimer.Start()
    End Sub

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        changeimage.Source = Util.CreateImageSource(My.Resources.切り替えアイコン)
        newmailimage.Source = Util.CreateImageSource(My.Resources.新規メールアイコン)
        favoriteimage.Source = Util.CreateImageSource(My.Resources.お気に入りアイコン)
        mementoimage.Source = Util.CreateImageSource(My.Resources.想い出アイコン)
        settingsimage.Source = Util.CreateImageSource(My.Resources.設定アイコン)
        minimizeimage.Source = Util.CreateImageSource(My.Resources.最小化アイコン)
        addfavoriteimage.Source = Util.CreateImageSource(My.Resources.お気に入り追加アイコン)
        replyimage1.Source = Util.CreateImageSource(My.Resources.返信メールアイコン)
    End Sub

    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

        Try
            _gameData.GameParameters = _context._gameParameters
            Dim serializer As New DataContractSerializer(GetType(GameData), _pluginedTypes)
            Dim settings As New XmlWriterSettings()
            settings.Encoding = New System.Text.UTF8Encoding(False)
            Dim s As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(s, settings)
            serializer.WriteObject(xw, _gameData)
            xw.Close()
            My.Settings.UserSetting = s.ToString()
            My.Settings.Save()
            RemoveHandler _mainLoopTimer.Elapsed, AddressOf MainLoop
        Catch ex As Exception
        End Try
        _mutex.Close()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseButtonEventArgs)
        DragMove()
        MyBase.OnMouseDown(e)
    End Sub

    Private _viewer As PluginViewerWindow = Nothing

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        If ((System.Windows.Forms.Control.ModifierKeys And Forms.Keys.Control) <> 0) And
           ((System.Windows.Forms.Control.ModifierKeys And Forms.Keys.Shift) <> 0) And
           ((System.Windows.Forms.Control.ModifierKeys And Forms.Keys.Alt) <> 0) Then

            If IsNothing(_viewer) OrElse Not _viewer.IsVisible Then
                _viewer = New PluginViewerWindow()
                _viewer.Show()
            Else
                _viewer.Activate()
            End If
        Else
            Close()
        End If
    End Sub

    Private Sub minimize_Click(sender As Object, e As RoutedEventArgs) Handles minimize.Click
        Me.Visibility = Visibility.Hidden
        Dim mv As New MinimizedMailWindow()
        mv.Left = Left() - (mv.Width - Width) / 2
        mv.Top = Top - (mv.Height - Height) / 2
        mv.ShowDialog()
        Me.Visibility = Visibility.Visible
    End Sub

    Private Sub _mailTitleList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim i = _mailTitleList.SelectedIndex
        If i < 0 Then Return
        Dim mli = DirectCast(_mailTitleList.Items(i), MailListItem)
        Dim mail = DirectCast(mli.DataContext, MailItem)
        _gameData.ReceivedMailList(_gameData.ReceivedMailList.Count - i - 1).Read = True
        mail.State = ""
        mli.State = ""
        mailContent.DataContext = mli.DataContext
        _mailView.Text = DirectCast(mli.DataContext, MailItem).Content
        _mailView.Stamp = DirectCast(mli.DataContext, MailItem).Stamp
        _mailView.AdventurePart = DirectCast(mli.DataContext, MailItem).AdventurePart
        _mailView.LayoutParts()
        _mailView.DoneAdventurePart = mail.DoneAdventurePart
        _currentMail = mail.Mail
        'If 0 < mail.Mail.Replies.Count Then
        '    reply.IsEnabled = True
        'Else
        '    reply.IsEnabled = False
        'End If
    End Sub

    Private _currentMail As Entity.Mail = Nothing

    Private Sub reply_Click(sender As Object, e As RoutedEventArgs)
        If IsNothing(_currentMail) Then Return

        ' TODO : すでに返信したメールについての処理（返信した選択肢のみ表示して送信ボタンは押せない感じ？）

        Dim rl As New List(Of Entity.Reply)
        If IsNothing(_currentMail.SelectedReply) Then
            For i = 0 To Math.Min(2, _currentMail.Replies.Count - 1)
                Dim rm = DirectCast(_currentMail.Replies(i).Manager, IReplyManager)
                If rm.canSelect(ApplicationContext.Instance) Then
                    rl.Add(_currentMail.Replies(i))
                End If
            Next
            If rl.Count = 0 Then Return
        Else
            ' 送信済みの場合
            rl = Nothing
        End If

        Dim rw As New ReplyWindow(rl, _currentMail.SelectedReply)
        rw.Owner = Me

        If rw.ShowDialog() Then
            ' リプライ送信処理
            _currentMail.SelectedReply = rw.SelectedReply
            Dim mgr = DirectCast(rw.SelectedReply.Manager, IReplyManager)
            mgr.onSent(_context)
            MessageBox.Show(String.Format("{0}を送信しました", rw.SelectedReply.Title, "情報"))
        End If
    End Sub

    Private Sub settings_Click(sender As Object, e As RoutedEventArgs)
        Dim sw As New SettingWindow
        sw.DataContext = _gameData.UserSettings.Clone
        sw.Owner = Me
        If sw.ShowDialog Then
            If sw.ClearGameData Then
                NewGame()
            Else
                _gameData.UserSettings = DirectCast(sw.DataContext, UserSettings)
                WriteUserSettingInContext()
            End If
        End If
    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        If Not _forceClose Then
            If MessageBox.Show("キュアぷらすを終了します、よろしいですか", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.No Then
                e.Cancel = True
            End If
        End If
        MyBase.OnClosing(e)
    End Sub

End Class
