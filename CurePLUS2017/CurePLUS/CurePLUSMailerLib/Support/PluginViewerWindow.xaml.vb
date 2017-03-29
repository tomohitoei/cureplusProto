Imports System.Reflection
Imports Microsoft.Win32

Public Class PluginViewerWindow

    Private _mailTriggerList As New Dictionary(Of String, Entity.Mail)

    Private Sub _miFileOpenPlugin_Click(sender As Object, e As RoutedEventArgs)
        Dim f As New OpenFileDialog()
        f.Filter = "dllファイル(*.dll)|*.dll"
        f.InitialDirectory = IO.Path.Combine(IO.Directory.GetCurrentDirectory, "Plugins")
        If Not f.ShowDialog() Then Return

        _mailTriggerList.Clear()
        _mailList.Items.Clear()
        _targets.Items.Clear()

        Dim asm = Reflection.Assembly.LoadFrom(f.FileName)

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
                        Util.SetMember(a, mail, {})
                    Next
                    mail.ID = typ.FullName
                    mail.Manager = mmgr
                    mmList.Add(typ, mail)
                    _mailTriggerList.Add(typ.Name, mail)
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

        If 0 = _mailTriggerList.Count Then
            MessageBox.Show("メールデータが見つかりません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
            Return
        End If

        For i = 0 To _mailTriggerList.Count - 1
            _mailList.Items.Add(_mailTriggerList.Values(i).Title)
        Next
        _mailList.SelectedIndex = 0
    End Sub

    Private Sub _miFileExit_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private _disableChangedEvent As Boolean = False
    Private Sub _mailList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If _disableChangedEvent Then Return
        _disableChangedEvent = True
        Try
            Dim index = _mailList.SelectedIndex
            Dim mail = _mailTriggerList.Values(index)
            _targets.Items.Clear()
            Dim ti As ListBoxItem
            ti = New ListBoxItem() With {.Content = String.Format("本文 : {0}", mail.Title)}
            ti.Tag = mail
            _targets.Items.Add(ti)
            For i = 0 To Math.Min(2, mail.Replies.Count - 1)
                ti = New ListBoxItem() With {.Content = String.Format("返信{0} : {1}", i + 1, mail.Replies(i).Title)}
                ti.Tag = mail.Replies(i)
                _targets.Items.Add(ti)
            Next
        Catch ex As Exception
        Finally
            _disableChangedEvent = False
        End Try
    End Sub

    Private Sub _targets_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If _disableChangedEvent Then Return
        _disableChangedEvent = True
        Try
            Dim ti = DirectCast(_targets.Items(_targets.SelectedIndex), ListBoxItem)
            If 0 = _targets.SelectedIndex Then
            Dim mail = DirectCast(ti.Tag, Entity.Mail)
                _content.Text = mail.Content
                _directInput.Text = mail.Content
                _content.Stamp = mail.Stamp
                _content.AdventurePart = mail.AdventurePart
        Else
            Dim rep = DirectCast(ti.Tag, Entity.Reply)
            _content.Text = rep.Content
                _directInput.Text = rep.Content
                _content.Stamp = String.Empty
                _content.AdventurePart = String.Empty
            End If
        _content.LayoutParts()
        Finally
        _disableChangedEvent = False
        End Try
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        'If _disableChangedEvent Then Return
        '_miViewDirectInput.IsChecked = Not _miViewDirectInput.IsChecked
        'If _miViewDirectInput.IsChecked Then
        '    _mailList.Visibility = Visibility.Hidden
        '    _targets.Visibility = Visibility.Hidden
        '    _directInput.Visibility = Visibility.Visible
        'Else
        '    _mailList.Visibility = Visibility.Visible
        '    _targets.Visibility = Visibility.Visible
        '    _directInput.Visibility = Visibility.Hidden
        'End If
    End Sub

    Private Sub _directInput_TextChanged(sender As Object, e As TextChangedEventArgs)
        If _disableChangedEvent Then Return
        _content.Text = _directInput.Text
        _content.Stamp = String.Empty
        _content.AdventurePart = String.Empty
        _content.LayoutParts()
    End Sub
End Class
