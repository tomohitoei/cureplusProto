Imports System.Windows.Media.Animation

Public Class MascotWindow
    Shared Sub New()
        Dim anm1 = Sub()
                       Dim animation As New ObjectAnimationUsingKeyFrames()
                       Storyboard.SetTargetName(animation, "image")
                       Storyboard.SetTargetProperty(animation, New PropertyPath("Source"))

                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ１), TimeSpan.Zero))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ2), TimeSpan.FromSeconds(1)))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ3), TimeSpan.FromSeconds(2)))
                       animation.KeyFrames.Add(New DiscreteObjectKeyFrame(CreateImageSource(My.Resources.ひめミニキャラ4), TimeSpan.FromSeconds(3)))

                       animation.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       animation.RepeatBehavior = RepeatBehavior.Forever

                       sb1.Children.Add(animation)
                       sb1.Duration = TimeSpan.FromSeconds(4) ' Duration.Forever
                       sb1.RepeatBehavior = RepeatBehavior.Forever
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

                       sb2.Children.Add(animation)
                       sb2.Duration = TimeSpan.FromSeconds(2) ' Duration.Forever
                       sb2.RepeatBehavior = RepeatBehavior.Forever
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

    Private Shared sb1 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}
    Private Shared sb2 As New Storyboard() With {.RepeatBehavior = RepeatBehavior.Forever, .AutoReverse = False}

    Private _timer As New Timers.Timer() With {.Interval = 100}

    Private Sub OnLoaded(sender As Object, e As RoutedEventArgs)
        'sb1.Begin(image)
        stat = -1

        AddHandler _timer.Elapsed,
            Sub()
                '_count += 1
                Dispatcher.Invoke(
                    Sub()
                        If 0 = TestView.NewMailCount Then
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
