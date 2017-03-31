Imports System.ComponentModel

Public Class MinimizedMailWindow
    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()
        _mailCount.Content = MailerWindow.NewMailCount.ToString
    End Sub

    Private _mx As Double = 0.0
    Private _my As Double = 0.0

    Private _timer As New Timers.Timer() With {.Interval = 100}

    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        _mx = Left
        _my = Top
        DragMove()
    End Sub

    Protected Overrides Sub OnMouseLeftButtonUp(e As MouseButtonEventArgs)
        If Math.Abs(_mx - Left) < 5 And Math.Abs(_my - Top) < 5 Then
            ' ウィンドウを動かさずにクリックしただけの場合
            _mascot1.Close()
            _mascot2.Close()
        End If
        MyBase.OnMouseLeftButtonUp(e)
    End Sub

    Private _mascot1 As New MascotWindow(0)
    Private _mascot2 As New MascotWindow(1)

    Private Function CreateImageSource(source As System.Drawing.Bitmap) As ImageSource
        Dim ms As IO.Stream = New IO.MemoryStream()
        ms.Seek(0, IO.SeekOrigin.Begin)
        source.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Dim [is] = New WriteableBitmap(BitmapFrame.Create(ms))
        ms.Close()
        Return [is]
    End Function

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        image.Source = CreateImageSource(My.Resources.mail_icon_free5)

        _mascot1.Top = Top
        _mascot1.Left = Left + Width
        _mascot1.Show()
        _mascot2.Top = Top
        _mascot2.Left = Left + Width + _mascot1.Width
        _mascot2.Show()

        AddHandler _mascot1.Closed, Sub()
                                        _mascot2.Close()
                                        Me.Close()
                                    End Sub
        AddHandler _mascot2.Closed, Sub()
                                        _mascot1.Close()
                                        Me.Close()
                                    End Sub

        AddHandler _timer.Elapsed, Sub()
                                       Dispatcher.Invoke(Sub() _mailCount.Content = MailerWindow.NewMailCount.ToString)
                                   End Sub
        _timer.Start()
    End Sub


    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        _timer.Stop()
        MyBase.OnClosing(e)
    End Sub
End Class
