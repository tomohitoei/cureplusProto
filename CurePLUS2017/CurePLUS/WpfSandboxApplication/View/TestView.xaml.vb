Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports System.Xml
Imports Microsoft.CSharp
Imports WpfSandboxLib

<Autofac.AttributedComponent.Component()> Public Class TestView

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
        For Each mi In _ml.values
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
                                      mi.Received = True
                                      Dim mli1 = New MailListItem() With {.DataContext = mi1}
                                      listBox.Items.Insert(0, mli1)
                                  End Sub)
            End If
        Next

        Dim count = 0
        For i = 0 To listBox.Items.Count - 1
            Dispatcher.Invoke(Sub() If Not String.IsNullOrEmpty(DirectCast(DirectCast(listBox.Items(i), MailListItem).DataContext, MailItem).State) Then count += 1)
        Next
        NewMailCount = count
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

            Dim resources = asm.GetManifestResourceNames()
            Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Entity.Mail)))
            For Each rs In resources
                If Not rs.ToLower().EndsWith("xml") Then Continue For
                Using sr As New IO.StreamReader(asm.GetManifestResourceStream(rs))
                    Dim cml = DirectCast(serializer.Deserialize(sr), List(Of Entity.Mail))
                    For i = 0 To cml.Count - 1
                        cml(i).ID = String.Format("{0}/{1}", rs, i + 1)
                        _ml.Add(cml(i).ID, cml(i))
                    Next
                End Using
            Next
            For Each mi In _ml.Values
                If String.IsNullOrEmpty(mi.Script) Then
                    MessageBox.Show(String.Format("メールにスクリプトが設定されていません : {0}", mi.Title), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
                    End
                End If
                Dim mgrName = mi.Script
                Dim mgr = asm.CreateInstance(mgrName)
                If IsNothing(mgr) Then
                    MessageBox.Show(String.Format("メールに設定されているマネージャに該当するクラスありません : {0}/{1}", mi.Title, mgrName), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
                    End
                End If
                mi.Manager = mgr
            Next
        Next

        ' スクリプトの読み込み
        'Dim provider As New CSharpCodeProvider()
        'Dim cp As New System.CodeDom.Compiler.CompilerParameters()
        'cp.ReferencedAssemblies.Add(IO.Path.Combine(IO.Path.Combine(IO.Directory.GetCurrentDirectory), "WpfSandboxLib.dll"))
        'For Each sf In IO.Directory.GetFiles(IO.Path.Combine(IO.Directory.GetCurrentDirectory, "Scripts"), "*.script")
        '    Dim fi As New IO.FileInfo(sf)
        '    Dim source = fi.OpenText.ReadToEnd
        '    Dim result = provider.CompileAssemblyFromSource(cp, source)
        '    Dim asm = result.CompiledAssembly
        '    _scripts.Add(fi.Name, asm)
        'Next

        'Try
        '    Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Entity.Mail)))
        '    For Each mf In IO.Directory.GetFiles(IO.Path.Combine(IO.Directory.GetCurrentDirectory, "MailData"), "*.txt")
        '        Dim fi As New IO.FileInfo(mf)
        '        Using sr As New IO.StreamReader(mf)
        '            Dim cml = DirectCast(serializer.Deserialize(sr), List(Of Entity.Mail))
        '            For i = 0 To cml.Count - 1
        '                cml(i).ID = String.Format("{0}/{1}", fi.Name, i + 1)
        '                _ml.add(cml(i).ID, cml(i))
        '            Next
        '        End Using
        '    Next

        '    For Each mi In _ml.values
        '        If String.IsNullOrEmpty(mi.Script) Then
        '            MessageBox.Show(String.Format("メールにスクリプトが設定されていません : {0}", mi.Title), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
        '            End
        '        End If
        '        Dim s = mi.Script.Split("/"c)
        '        If s.Length < 2 Then
        '            MessageBox.Show(String.Format("メールに設定されているスクリプトの記載が不正です : {0}", mi.Title), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
        '            End
        '        End If
        '        If Not _scripts.ContainsKey(s(0)) Then
        '            MessageBox.Show(String.Format("メールに設定されているスクリプトに該当するファイルがありません : {0}/{1}", mi.Title, s(0)), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
        '            End
        '        End If
        '        Dim asm = _scripts(s(0))
        '        Dim mgr = asm.CreateInstance(s(1))
        '        If IsNothing(mgr) Then
        '            MessageBox.Show(String.Format("メールに設定されているマネージャに該当するクラスありません : {0}/{1}/{2}", mi.Title, s(0), s(1)), "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
        '            End
        '        End If
        '        mi.Manager = mgr
        '    Next
        'Catch ex As Exception
        'End Try

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
        'For i = 0 To _ml.Count - 1
        '    Dim mi1 As New MailItem
        '    setCharacter(_ml(i).Sender, mi1)
        '    mi1.Title = _ml(i).Title
        '    mi1.Content = _ml(i).Content
        '    mi1.ReceivedDate = New Date(2017, 3, i + 1)
        '    Dim mli1 = New MailListItem() With {.DataContext = mi1}
        '    listBox.Items.Insert(0, mli1)
        'Next

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
        mv.Left = Left - (mv.Width - Width) / 2
        mv.Top = Top - (mv.Height - Height) / 2
        mv.ShowDialog()
        Me.Visibility = Visibility.Visible
    End Sub

    Private Sub listBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim i = listBox.SelectedIndex
        If i < 0 Then Return
        Dim mli = DirectCast(listBox.Items(i), MailListItem)
        DirectCast(mli.DataContext, MailItem).State = ""
        mli.state.Content = ""
        mailContent.DataContext = mli.DataContext
        _mailView.Text = DirectCast(mli.DataContext, MailItem).Content
        _mailView.Stamp = DirectCast(mli.DataContext, MailItem).Stamp
        _mailView.AdventurePart = DirectCast(mli.DataContext, MailItem).AdventurePart
        _mailView.LayoutParts()
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
