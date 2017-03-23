Namespace MailContent

    Public Class Parser

        Public Sub New()

        End Sub



    End Class

    Public Class LineParser
        Private _buf As String
        Private _index As Integer = 0

        Public Sub New(buf As String)
            _buf = buf
        End Sub

        Public ReadOnly Property Read() As Token
            Get
                If _buf.Length <= _index Then Return Nothing

                If _index <= _buf.Length - 2 AndAlso _buf.Substring(_index, 2).Equals("\{") Then
                    ' 絵文字
                    Dim i = _index + 2
                    Do
                        If _buf.Length <= i Then Exit Do
                        If _buf.Substring(i, 1).Equals("}") Then Exit Do
                        i += 1
                    Loop

                    Dim value = _buf.Substring(_index + 2, i - _index - 2)
                    _index = i + 1
                    Return New Token() With {.Value = value, .Type = Token.TokenType.Emoji}

                Else
                    ' テキスト
                    Dim i = _index + 1
                    Do
                        If _buf.Length <= i Then Exit Do
                        If i <= _buf.Length - 2 AndAlso _buf.Substring(i, 2).Equals("\{") Then Exit Do
                        i += 1
                    Loop
                    Dim value = _buf.Substring(_index, i - _index)
                    _index = i
                    Return New Token() With {.Value = value, .Type = Token.TokenType.Text}
                End If
            End Get
        End Property

        Public Class Token
            Public Property Value As String
            Public Property Type As TokenType
            Public Enum TokenType
                Text
                Emoji
            End Enum
        End Class
    End Class

End Namespace
