Imports System.ComponentModel

Public Class MinimizedMailWindow
    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

    End Sub

    Private _mx As Double = 0.0
    Private _my As Double = 0.0

    Private Shared _count As Integer = 12
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
            _mascot.Close()
        End If
        MyBase.OnMouseLeftButtonUp(e)
    End Sub

    Private _mascot As New MascotWindow

    Private Function CreateImageSource(source As System.Drawing.Bitmap) As ImageSource
        Dim ms As IO.Stream = New IO.MemoryStream()
        ms.Seek(0, IO.SeekOrigin.Begin)
        source.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Dim [is] = New WriteableBitmap(BitmapFrame.Create(ms))
        ms.Close()
        Return [is]
    End Function

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        _mailCount.Content = _count.ToString()
        image.Source = CreateImageSource(My.Resources.mail_icon_free5)

        _mascot.Top = Top
        _mascot.Left = Left + Width
        _mascot.Show()

        AddHandler _mascot.Closed, Sub()
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
