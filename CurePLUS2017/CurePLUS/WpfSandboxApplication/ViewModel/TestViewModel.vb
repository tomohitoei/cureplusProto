<Autofac.AttributedComponent.Component()> Public Class TestViewModel
    Inherits Caliburn.Micro.PropertyChangedBase
    Implements ITest

    Private _count As Integer = 90

    Public Property Count As Integer
        Get
            Return _count
        End Get
        Set(value As Integer)
            _count = value
            NotifyOfPropertyChange(Function() Count)
            NotifyOfPropertyChange(Function() CanIncrementCount)
        End Set
    End Property

    Public Sub IncrementCount(delta As Integer)
        Count += delta
    End Sub

    Public ReadOnly Property CanIncrementCount As Boolean
        Get
            Return Count < 100
        End Get
    End Property
End Class
