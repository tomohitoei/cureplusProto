Public Interface IMailManager
    Function canReceive(context As ApplicationContext) As Boolean
    Sub onReceived(context As ApplicationContext)
    Sub onRead(context As ApplicationContext)
End Interface
