Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Namespace GainClient
    Public Class GainMarketService

        Private Const SOCKET_TIMEOUT_MS As Integer = 30000

        Private _FailedCount As Integer
        Private _FailedConsecutiveCount As Integer
        Private _SuccessCount As Integer
        Private _Rates As Hashtable

        Public Event OnRateServiceConnected()
        Public Event OnRateServiceDisconnected()
        Public Event OnRateServiceConnectionFailed(ByVal ex As Exception)
        Public Event OnRateServiceConnectionLost(ByVal ex As Exception)
        Public Event OnRateServiceRate(ByVal rate As GainMarketData)

        Public _isMktLoggedIn As Boolean
        Dim _Key As String
        Dim _Host As String
        Dim _Port As String



        Public Shared ReadOnly Property Version() As String
            Get
                Return "1.0.0"
            End Get
        End Property

        Public ReadOnly Property SuccessfullConnectionCount() As Integer
            Get
                Return _SuccessCount
            End Get
        End Property

        Public ReadOnly Property FailedConnectionCount() As Integer
            Get
                Return _FailedCount
            End Get
        End Property

        Public ReadOnly Property FailedConsecutiveConnectionCount() As Integer
            Get
                Return _FailedConsecutiveCount
            End Get
        End Property

        Public Sub New()
            _FailedCount = 0
            _FailedConsecutiveCount = 0
            _SuccessCount = 0
        End Sub

        Public Function Connect(ByVal Host As String, ByVal Port As Integer, ByVal Key As String) As Boolean
            If (_isMktLoggedIn) Then
                GainUtil.WriteDebugLog(".....Market Data is Already Connected.")
                Return False
                'Exit Function
            End If
            _Host = Host
            _Port = Port
            _Key = Key
            Try
                Dim thread As New Thread(New ThreadStart(AddressOf Me.ProcessRatesFeed))
                _isMktLoggedIn = True
                thread.Start()
            Catch ex As Exception
                GainUtil.WriteDebugLog(".....Exception starting rate service thread: " & ex.Message)
                Return False
                'Exit Function
            End Try
            Return True
        End Function

        Public Function Disconnect() As Boolean
            If Not _isMktLoggedIn Then
                GainUtil.WriteDebugLog(".....MarketData is Not connected")
                Return False
                'Exit Function
            End If
            _isMktLoggedIn = False
            GainUtil.WriteDebugLog(".....MarketData is Disconnected Now.")
            Return True
        End Function

        Private Sub ProcessRatesFeed()
            GainUtil.WriteDebugLog(".....ProcessRatesFeed started in thread")
            Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim socket As Socket = Nothing
            Dim MessagesRead As Integer = 0
            Dim LastException As Exception = Nothing
            Dim AuthMessage As String

            Try
                Dim hostEndPoint As New IPEndPoint(Dns.GetHostEntry(_Host).AddressList(0), _Port)

                socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                socket.Blocking = True
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, SOCKET_TIMEOUT_MS)
                socket.Connect(hostEndPoint)

                If (socket.Connected = False) Then
                    GainUtil.WriteDebugLog(".....Socket failed to connected to " & hostEndPoint.ToString())
                End If
                GainUtil.WriteDebugLog("..... Sockect Connected")
                AuthMessage = _Key + "\COMPACT" + ControlChars.Cr
                socket.Send(Encoding.ASCII.GetBytes(AuthMessage))
                GainUtil.WriteDebugLog(".....Sockect Authenication Sent")

                While _isMktLoggedIn
                    Dim Message As String
                    Message = ReadMessage(socket)
                    If (Message <> "") Then
                        MessagesRead += 1
                        If (MessagesRead = 1) Then
                            _isMktLoggedIn = True
                            _SuccessCount += 1
                            _FailedConsecutiveCount = 0
                        End If
                        ProcessMessage(Message)
                    End If
                End While

            Catch ex As Exception
                GainUtil.WriteDebugLog(".....Error in Socket Read:" + ex.Message + ex.StackTrace)
                LastException = ex
            Finally
                If (Not IsNothing(socket)) Then
                    If (socket.Connected) Then
                        socket.Shutdown(SocketShutdown.Both)
                        GainUtil.WriteDebugLog(".....Closing Socket")
                        socket.Close()
                    End If
                End If
            End Try
            If Not IsNothing(LastException) Then
                _FailedCount += 1
                _FailedConsecutiveCount += 1
                If (MessagesRead > 0) Then
                    RaiseEvent OnRateServiceConnectionLost(LastException)
                    Exit Sub
                Else
                    RaiseEvent OnRateServiceConnectionFailed(LastException)
                    Exit Sub
                End If
            Else
                RaiseEvent OnRateServiceDisconnected()
                Exit Sub
            End If
            RaiseEvent OnRateServiceConnected()
        End Sub

        Private Sub ProcessMessage(ByVal Message As String)

            If (Message.StartsWith("S")) Then
                ProcessSMessage(Message.Substring(1))
            ElseIf (Message.StartsWith("R")) Then
                ProcessRMessage(Message.Substring(1))
            Else
                'GainUtil.WriteDebugLog(".....ProcessMessage Unknown message: " & Message)
            End If
        End Sub

        Private Sub ProcessSMessage(ByVal Message As String)

            _Rates = New Hashtable(20)

            Dim RateMessages() As String = Split(Message, "$")
            Dim RateMessage As String
            For Each RateMessage In RateMessages
                Try
                    Dim RateFields() As String = Split(RateMessage, "\")
                    If (RateFields.Length >= 9) Then

                        Dim rate As New GainMarketData()
                        rate.Key = RateFields(0)
                        rate.CurrencyPair = RateFields(1)
                        rate.Bid = Convert.ToDecimal(RateFields(2))
                        rate.Ask = Convert.ToDecimal(RateFields(3))
                        rate.High = Convert.ToDecimal(RateFields(4))
                        rate.Low = Convert.ToDecimal(RateFields(5))
                        rate.Dealable = RateFields(6)
                        rate.Domain = RateFields(7)
                        rate.DecimalPlaces = Convert.ToInt32(RateFields(8))
                        _Rates.Add(rate.Key, rate)
                        RaiseEvent OnRateServiceRate(rate)

                    End If

                Catch ex As Exception
                    GainUtil.WriteDebugLog(".....ProcessSMessage Exception: " & ex.ToString())
                End Try
            Next

        End Sub

        Private Sub ProcessRMessage(ByVal Message As String)
            Try
                Dim RateFields() As String = Split(Message, "\")
                If (RateFields.Length >= 4) Then
                    Dim rate As GainMarketData
                    rate = _Rates.Item(RateFields(0))
                    If (Not (IsNothing(rate))) Then
                        rate.Bid = Convert.ToDecimal(RateFields(1))
                        rate.Ask = Convert.ToDecimal(RateFields(2))
                        rate.Dealable = RateFields(3)
                        If (rate.Bid < rate.Low) Then
                            rate.Low = rate.Bid
                        End If

                        If (rate.Ask > rate.High) Then
                            rate.High = rate.Ask
                        End If
                        RaiseEvent OnRateServiceRate(rate)
                    Else
                        GainUtil.WriteDebugLog(".....ProcessCMessage Failed to find currency pair with key: " & RateFields(0))
                    End If
                End If

            Catch ex As Exception
                GainUtil.WriteDebugLog(".....ProcessCMessage Exception: " & ex.ToString())
            End Try

        End Sub

        Private Function ReadMessage(ByRef socket As Socket) As String
            Dim Done As Boolean = False
            Dim Message As New StringBuilder(10)
            Dim Buffer(0) As Byte
            While Done = False
                socket.Receive(Buffer)

                If (Buffer(0) = -1) Then
                    GainUtil.WriteDebugLog(".....Unexpected EOF during ReadMessage")
                End If

                If (Buffer(0) = 13) Then
                    Done = True
                Else
                    Message.Append(Convert.ToChar(Buffer(0)))
                End If
            End While
            ReadMessage = Message.ToString()
        End Function
    End Class
End Namespace