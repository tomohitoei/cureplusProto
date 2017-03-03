Imports System.ComponentModel

Public Class MinimizedMailWindow
    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

    End Sub

    Private _mx As Double = 0.0
    Private _my As Double = 0.0

    Private Shared _count As Integer = 12
    Private _timer As New Timers.Timer() With {.Interval = 5000}

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
            '            Me.Close()
        End If
        MyBase.OnMouseLeftButtonUp(e)
    End Sub

    Private _mascot As New MascotWindow

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        _mailCount.Content = _count.ToString()

        Dim bmp As New RenderTargetBitmap(CInt(image.Width), CInt(image.Height), 96, 96, PixelFormats.Default)
        Dim dv As New DrawingVisual()
        Dim dc = dv.RenderOpen()
        dc.DrawRectangle(Brushes.White, New Pen(Brushes.Black, 5), New Rect(0, 0, bmp.Width, bmp.Height))
        dc.Close()
        bmp.Render(dv)
        image.Source = bmp

        _mascot.Top = Top
        _mascot.Left = Left + Width
        _mascot.Show()

        AddHandler _mascot.Closed, Sub()
                                       Me.Close()
                                   End Sub

        AddHandler _timer.Elapsed, Sub()
                                       _count += 1
                                       Dispatcher.Invoke(Sub() _mailCount.Content = _count.ToString())
                                   End Sub
        _timer.Start()
    End Sub


    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        _timer.Stop()
        MyBase.OnClosing(e)
    End Sub
End Class
