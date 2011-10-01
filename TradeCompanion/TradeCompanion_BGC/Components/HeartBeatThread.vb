'Imports System.Threading
'Imports System.Net
'Imports RemotingServer
'Imports System.Runtime.Remoting
'Imports System.Runtime.Remoting.Channels
'Imports System.Runtime.Remoting.Channels.Http
'Public Class HeartBeatThread
'    Dim loginid As String
'    Public heartBeatThread As System.Threading.Thread
'    Private Shared _singletonHeartBeatThread As HeartBeatThread
'    Dim obj As BaseRemoteObject
'    Private Sub New()
'        Dim chnl As New HttpChannel()
'        ChannelServices.RegisterChannel(chnl)
'        obj = DirectCast(Activator.GetObject(GetType(BaseRemoteObject), "http://vin:81/SessionMaintainence/MyRemoteObject.rem"), BaseRemoteObject)
'    End Sub

'    Public Shared Function getInstance() As HeartBeatThread
'        If (_singletonHeartBeatThread Is Nothing) Then
'            _singletonHeartBeatThread = New HeartBeatThread()
'        End If
'        Return _singletonHeartBeatThread
'    End Function

'    Public Sub StartHeartBeat(ByVal login As String)
'        loginid = login
'        heartBeatThread = New Thread(AddressOf Run)
'        heartBeatThread.Start()
'    End Sub

'    Private Sub Run()
'        While (True)
'            Try
'                Dim heartBeat As RemotingServer.HeartBeat = New RemotingServer.HeartBeat
'                heartBeat.LoginID = loginid
'                heartBeat.Status = 0
'                heartBeat = obj.HeartBeat(heartBeat)

'                If (heartBeat.Status = 0) Then
'                    Util.WriteDebugLog("HearBeat : Success")
'                    'success
'                ElseIf (heartBeat.Status < 0) Then
'                    Util.WriteDebugLog("HearBeat : Failure")
'                    'Error Forceful logout
'                End If
'            Catch ex As Exception
'                Util.WriteDebugLog("HearBeat Web Service Failure" + ex.Message)
'            End Try

'            Thread.Sleep(60 * 1000)
'        End While
'    End Sub

'    Public Sub StopHeartBeat()
'        heartBeatThread.Abort()

'        Dim heartBeat As RemotingServer.HeartBeat = New RemotingServer.HeartBeat
'        heartBeat.LoginID = loginid
'        heartBeat.Status = 0
'        obj.DeleteUserSession(heartBeat)
'    End Sub

'End Class
