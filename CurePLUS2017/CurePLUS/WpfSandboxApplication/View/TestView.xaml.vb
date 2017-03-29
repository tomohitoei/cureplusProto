Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Windows.Threading
Imports System.Xml
Imports Microsoft.CSharp
Imports WpfSandboxLib

'<Autofac.AttributedComponent.Component()>
Public Class TestView

    Private _userSettings As New UserSettings
    Private _mutex As Threading.Mutex = Nothing


    Private _mainLoopTimer As Timers.Timer

    Private _count As Integer = 0
    Private _scripts As New Dictionary(Of String, Reflection.Assembly)
    Private _plugins As New Dictionary(Of String, Reflection.Assembly)

    Private _ml As New Dictionary(Of String, Entity.Mail)

    Public Shared NewMailCount As Integer

    Private Sub setCharacter(id As Integer, mi As MailItem)
        Select Case id
            Case 1
                mi.CharacterIcon = CreateImageSource(My.Resources._02_021_ひめ_アイコン)
                mi.CharacterName = "ひめ"
            Case 2
                mi.CharacterIcon = CreateImageSource(My.Resources._01_011_響_アイコン)
                mi.CharacterName = "響"
        End Select
    End Sub

    Private Sub MainLoop(sender As Object, e As Timers.ElapsedEventArgs)
        _context.SetValue(Of DateTime)("現在日時", Now)
        For Each mi In _ml.Values
            If mi.Received Then Continue For
            Dim canReceive = mi.Manager.GetType().GetMethod("canReceive")
            Dim rr = CBool(canReceive.Invoke(mi.Manager, New Object() {_context}))
            If rr Then
                Dispatcher.Invoke(Sub()
                                      Dim mi1 As New MailItem
                                      setCharacter(mi.Sender, mi1)
                                      mi1.Title = mi.Title
                                      mi1.Content = mi.Content
                                      mi1.Stamp = mi.Stamp
                                      mi1.AdventurePart = mi.AdventurePart
                                      mi1.ReceivedDate = Now
                                      mi1.Mail = mi
                                      mi.Received = True
                                      Dim mli1 = New MailListItem() With {.DataContext = mi1}
                                      _ListBox.Items.Insert(0, mli1)
                                  End Sub)
            End If
        Next

        Dim count = 0
        For i = 0 To _ListBox.Items.Count - 1
            Dispatcher.Invoke(Sub() If Not String.IsNullOrEmpty(DirectCast(DirectCast(_listBox.Items(i), MailListItem).DataContext, MailItem).State) Then count += 1)
        Next
        NewMailCount = count
    End Sub

    Private Sub SetMember(source As CustomAttributeData, target As Object, ignore() As String)
        Dim props As New Dictionary(Of String, PropertyInfo)
        For Each pi In target.GetType().GetProperties
            props.Add(pi.Name, pi)
        Next

        For Each na In source.NamedArguments
            If ignore.Contains(na.MemberName) Then Continue For
            If props.ContainsKey(na.MemberName) Then
                Dim tp = props(na.MemberName)
                tp.SetValue(target, na.TypedValue.Value)
            End If
        Next
    End Sub

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

        InitializeApplication()

        _ml.Clear()
        _plugins.Clear()

        ' プラグインの読み込み
        For Each f In IO.Directory.GetFiles(IO.Path.Combine(IO.Directory.GetCurrentDirectory, "Plugins"), "*.dll")
            Dim fi As New IO.FileInfo(f)
            Dim asm = Reflection.Assembly.LoadFrom(f)
            _plugins.Add(fi.Name, asm)

            ' メール・リプライマネージャ作成
            Dim mmList As New Dictionary(Of Type, Entity.Mail)
            Dim rmList As New List(Of IReplyManager)
            For Each typ In asm.GetExportedTypes()
                Try
                    Dim obj = asm.CreateInstance(typ.FullName)
                    If TypeOf obj Is IMailManager Then
                        Dim mmgr = DirectCast(obj, IMailManager)
                        Dim mail As New Entity.Mail
                        Dim al = typ.CustomAttributes()
                        If al.Count = 0 Then Continue For ' 属性の付いていないマネージャはスキップ
                        For Each a In al
                            SetMember(a, mail, {})
                        Next
                        mail.Manager = mmgr
                        mmList.Add(typ, mail)
                        _ml.Add(typ.Name, mail)
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
                    SetMember(a, temp, {})
                    If Not IsNothing(temp.Parent) Then Exit For
                Next
                If IsNothing(temp.Parent) Then Continue For ' 属性の付いていないマネージャはスキップ
                If mmList.ContainsKey(DirectCast(temp.Parent, Type)) Then
                    Dim reply As New Entity.Reply
                    For Each a In al
                        SetMember(a, reply, {"Parent"})
                    Next
                    reply.Manager = rmgr
                    Dim parent = mmList(DirectCast(temp.Parent, Type))
                    parent.Replies.Add(reply)
                    reply.Parent = parent
                End If
            Next
        Next

        _mainLoopTimer = New Timers.Timer(1000)
        AddHandler _mainLoopTimer.Elapsed, AddressOf MainLoop
        _mainLoopTimer.Start()

    End Sub

    Private _context As ApplicationContext = ApplicationContext.Instance()

    Private Sub InitializeApplication()
        _context.SetValue(Of DateTime)("初回起動日時", Now)
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
    End Sub

    Protected Overrides Sub OnClosed(e As EventArgs)
        MyBase.OnClosed(e)

        RemoveHandler _mainLoopTimer.Elapsed, AddressOf MainLoop

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
        mv.Left = Left() - (mv.Width - Width) / 2
        mv.Top = Top - (mv.Height - Height) / 2
        mv.ShowDialog()
        Me.Visibility = Visibility.Visible
    End Sub

    Private Sub listBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim i = _listBox.SelectedIndex
        If i < 0 Then Return
        Dim mli = DirectCast(_listBox.Items(i), MailListItem)
        Dim mail = DirectCast(mli.DataContext, MailItem)
        mail.State = ""
        mli.State = ""
        mailContent.DataContext = mli.DataContext
        _mailView.Text = DirectCast(mli.DataContext, MailItem).Content
        _mailView.Stamp = DirectCast(mli.DataContext, MailItem).Stamp
        _mailView.AdventurePart = DirectCast(mli.DataContext, MailItem).AdventurePart
        _mailView.LayoutParts()
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

        ' TODO : すでに返信したメールについての処理（返信した選択肢の未表示して送信ボタンは押せない感じ？）

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
