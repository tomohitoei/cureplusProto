Public Class AppBootstrapper
    Inherits Caliburn.Micro.BootstrapperBase

    Public Sub New()
        Initialize()
    End Sub

    Private _container As Caliburn.Micro.SimpleContainer = Nothing


    Protected Overrides Sub Configure()
        _container = New Caliburn.Micro.SimpleContainer()
        _container.RegisterSingleton(
            GetType(Caliburn.Micro.IWindowManager), "mgr",
            GetType(Caliburn.Micro.WindowManager))
        _container.RegisterSingleton(GetType(Caliburn.Micro.IEventAggregator), "ea",
                                     GetType(Caliburn.Micro.EventAggregator))
        _container.RegisterPerRequest(GetType(ITest), "test",
                                      GetType(TestViewModel))
    End Sub

    Protected Overrides Sub OnStartup(sender As Object, e As StartupEventArgs)
        DisplayRootViewFor(Of ITest)()
    End Sub

    Protected Overrides Sub OnExit(sender As Object, e As EventArgs)
        MyBase.OnExit(sender, e)
    End Sub

    Protected Overrides Function GetInstance(service As Type, key As String) As Object
        Dim instance = _container.GetInstance(service, key)
        If Not IsNothing(instance) Then Return instance
        Throw New InvalidOperationException("指定のインスタンスを見つけることができませんでした")
    End Function

    Protected Overrides Function GetAllInstances(service As Type) As IEnumerable(Of Object)
        Return _container.GetAllInstances(service)
    End Function

    Protected Overrides Sub BuildUp(instance As Object)
        _container.BuildUp(instance)
    End Sub
End Class
