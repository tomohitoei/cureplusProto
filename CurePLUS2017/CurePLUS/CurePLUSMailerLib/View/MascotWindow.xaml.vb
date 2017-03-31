Imports System.Windows.Media.Animation

Public Class MascotWindow
    Shared Sub New()
        Dim anm1 = Sub()
                       Dim animation As New ObjectAnimationUsingKeyFrames()
                       Storyboard.SetTargetName(animation, "image")
                       Storyboard.SetTargetProperty(animation, New PropertyPath("Source"))

                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ1), TimeSpan.Zero))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ2), TimeSpan.FromSeconds(1)))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ3), TimeSpan.FromSeconds(2)))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ4), TimeSpan.FromSeconds(3)))

                       animation.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       animation.RepeatBehavior = RepeatBehavior.Forever

                       sb11.Children.Add(animation)
                       sb11.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       sb11.RepeatBehavior = RepeatBehavior.Forever

                       Dim animation2 As New ObjectAnimationUsingKeyFrames()
                       Storyboard.SetTargetName(animation2, "image")
                       Storyboard.SetTargetProperty(animation2, New PropertyPath("Source"))

                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ1), TimeSpan.Zero))
                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ2), TimeSpan.FromSeconds(1)))
                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ3), TimeSpan.FromSeconds(2)))
                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ4), TimeSpan.FromSeconds(3)))

                       animation2.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       animation2.RepeatBehavior = RepeatBehavior.Forever

                       sb21.Children.Add(animation2)
                       sb21.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       sb21.RepeatBehavior = RepeatBehavior.Forever
                   End Sub
        anm1()

        Dim anm2 = Sub()
                       Dim animation As New ObjectAnimationUsingKeyFrames()
                       Storyboard.SetTargetName(animation, "image")
                       Storyboard.SetTargetProperty(animation, New PropertyPath("Source"))

                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ5), TimeSpan.Zero))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ6), TimeSpan.FromSeconds(1)))

                       animation.Duration = TimeSpan.FromSeconds(2) ' Duration.Forever
                       animation.RepeatBehavior = RepeatBehavior.Forever

                       sb12.Children.Add(animation)
                       sb12.Duration = TimeSpan.FromSeconds(2) ' Duration.Forever
                       sb12.RepeatBehavior = RepeatBehavior.Forever

                       Dim animation2 As New ObjectAnimationUsingKeyFrames()
                       Storyboard.SetTargetName(animation2, "image")
                       Storyboard.SetTargetProperty(animation2, New PropertyPath("Source"))

                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ5), TimeSpan.Zero))
                       animation2.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.響ミニキャラ6), TimeSpan.FromSeconds(1)))

                       animation2.Duration = TimeSpan.FromSeconds(2) ' Duration.Forever
                       animation2.RepeatBehavior = RepeatBehavior.Forever

                       sb22.Children.Add(animation2)
                       sb22.Duration = TimeSpan.FromSeconds(2) ' Duration.Forever
                       sb22.RepeatBehavior = RepeatBehavior.Forever
                   End Sub
        anm2()
    End Sub

    Private Shared Function CreateImageSource(source As System.Drawing.Bitmap) As ImageSource
        Dim ms As IO.Stream = New IO.MemoryStream()
        ms.Seek(0, IO.SeekOrigin.Begin)
        source.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        Dim [is] = New WriteableBitmap(BitmapFrame.Create(ms))
        ms.Close()
        Return [is]
    End Function

    Private Shared sb11 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}
    Private Shared sb12 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}
    Private Shared sb21 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}
    Private Shared sb22 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}

    Private sb1 As Storyboard
    Private sb2 As Storyboard

    Private _timer As New Timers.Timer() With {.Interval = 100}

    Public Sub New(id As Integer)

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        sb1 = sb11
        sb2 = sb12
        If id = 1 Then
            sb1 = sb21
            sb2 = sb22
        End If

    End Sub

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
        'sb1.Begin(image)
        stat = -1

        AddHandler _timer.Elapsed,
            Sub()
                '_count += 1
                Dispatcher.Invoke(
                    Sub()
                        If 0 = MailerWindow.NewMailCount Then
                            If stat <> 0 Then
                                sb2.Stop()
                                sb1.Begin(image)
                            End If
                            stat = 0
                        Else
                            If stat <> 1 Then
                                sb1.Stop()
                                sb2.Begin(image)
                            End If
                            stat = 1
                        End If
                    End Sub)
            End Sub
        _timer.Start()
    End Sub

    Private _mx As Double = 0
    Private [_my] As Double = 0
    Protected Overrides Sub OnMouseLeftButtonDown(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonDown(e)
        _mx = Me.Left
        _my = Me.Top
        DragMove()
    End Sub

    Private stat As Integer

    Protected Overrides Sub OnMouseLeftButtonUp(e As MouseButtonEventArgs)
        MyBase.OnMouseLeftButtonUp(e)
        'If _mx = Left And _my = Top Then
        '    If stat = 0 Then
        '        sb1.Stop()
        '        sb2.Begin(image)
        '        stat = 1
        '    Else
        '        sb2.Stop()
        '        sb1.Begin(image)
        '        stat = 0
        '    End If
        'End If
        ' TODO : アニメーションさせる
    End Sub


    Protected Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        MyBase.OnMouseWheel(e)
        Dim nw = Me.Width + e.Delta / 5
        Dim nh = Me.Height + e.Delta / 5
        If nw < 256 Then nw = 256
        If nh < 256 Then nh = 256
        If nw > 640 Then nw = 640
        If nh > 640 Then nh = 640
        Width = nw
        Height = nh
    End Sub

    Private Sub QuitClicked(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub
End Class
