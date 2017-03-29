Public Interface IReplyManager
    Function canSelect(context As ApplicationContext) As Boolean
    Sub onSent(context As ApplicationContext)
End Interface
