'***********************************************************************
'  PDCstream.Concentrator.vb - PDCstream Concentrator
'  Copyright � 2004 - TVA, all rights reserved
'
'  Build Environment: VB.NET, Visual Studio 2003
'  Primary Developer: James R Carroll, System Analyst [WESTAFF]
'      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
'       Phone: 423/751-2827
'       Email: jrcarrol@tva.gov
'
'  Code Modification History:
'  ---------------------------------------------------------------------
'  11/12/2004 - James R Carroll
'       Initial version of source generated
'
'  Note:  Because DatAWare DAQ plug-in's only poll once per second,
'  data can't be considered stale until after one second, this will
'  also generally create a one second lag-time in the real-time data
'  broadcast - the only way to increase this resolution would be
'  increase the polling interval in DatAWare and this is currently
'  not configurable.  However, Brian Fox has told me that it
'  probably wouldn't be hard to add this if this becomes an issue...
'
'***********************************************************************

Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports TVA.Shared.DateTime
Imports TVA.Services

Namespace PDCstream

    Public Class Concentrator

        Implements IServiceComponent
        Implements IDisposable

        Private m_parent As DatAWarePDC
        Private m_configFile As ConfigFile
        Private m_dataQueue As DataQueue
        Private m_udpClient As UdpClient
        Private m_broadcastIPs As IPEndPoint()
        Private WithEvents m_processTimer As Timers.Timer
        Private WithEvents m_sweepTimer As Timers.Timer
        Private m_enabled As Boolean
        Private m_processing As Boolean
        Private m_startTime As Single
        Private m_stopTime As Single
        Private m_bytesBroadcasted As Long
        Private m_packetsPublished As Long

        Public Event SamplePublished(ByVal sample As DataSample)

#Region " Common Primary Service Process Code "

        Public Sub New(ByVal parent As DatAWarePDC, ByVal configFileName As String, ByVal broadcastIPs As IPEndPoint())

            m_parent = parent
            m_configFile = New ConfigFile(configFileName)
            m_dataQueue = New DataQueue(m_configFile)
            m_udpClient = New UdpClient
            m_broadcastIPs = broadcastIPs
            m_processTimer = New Timers.Timer
            m_sweepTimer = New Timers.Timer
            m_enabled = True
            m_processing = False

            ' We check for ready-to-publish or stale data packets at the sample rate interval
            With m_processTimer
                .Interval = 1000 / m_configFile.SampleRate
                .AutoReset = False
            End With

            ' We check for samples that need to be removed every second
            With m_sweepTimer
                .Interval = 1000
                .AutoReset = True
                .Enabled = True
            End With

        End Sub

        Protected Overrides Sub Finalize()

            MyBase.Finalize()
            Dispose()

        End Sub

        Public Overridable Sub Dispose() Implements IServiceComponent.Dispose, IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ' Any needed shutdown code for your primary service process should be added here - note that this class
            ' instance is available for the duration of the service lifetime...

        End Sub

        ' This function handles updating status for the primary service process
        Public Sub UpdateStatus(ByVal Status As String, Optional ByVal LogStatusToEventLog As Boolean = False, Optional ByVal EntryType As System.Diagnostics.EventLogEntryType = EventLogEntryType.Information)

            m_parent.UpdateStatus(Status, LogStatusToEventLog, EntryType)

        End Sub

        Public Sub UpdateProgress(ByVal current As Long, ByVal total As Long)

            m_parent.ServiceHelper.SendServiceProgress(current, total)

        End Sub

        Public Property Enabled() As Boolean
            Get
                Return m_enabled
            End Get
            Set(ByVal Value As Boolean)
                m_enabled = Value
            End Set
        End Property

        Public Property Processing() As Boolean
            Get
                Return m_processing
            End Get
            Set(ByVal Value As Boolean)
                m_processing = Value

                If m_processing Then
                    m_startTime = DateTime.Now.Ticks
                    m_stopTime = 0
                    m_bytesBroadcasted = 0
                    m_packetsPublished = 0
                Else
                    m_stopTime = DateTime.Now.Ticks
                End If
            End Set
        End Property

        Public ReadOnly Property RunTime() As Double
            Get
                Dim ProcessingTime As Long

                If m_startTime > 0 Then
                    If m_stopTime > 0 Then
                        ProcessingTime = m_stopTime - m_startTime
                    Else
                        ProcessingTime = DateTime.Now.Ticks - m_startTime
                    End If
                End If

                If ProcessingTime < 0 Then ProcessingTime = 0

                Return ProcessingTime / 10000000L
            End Get
        End Property

#End Region

        Public ReadOnly Property ConfigFile() As ConfigFile
            Get
                Return m_configFile
            End Get
        End Property

        Public ReadOnly Property DataQueue() As DataQueue
            Get
                Return m_dataQueue
            End Get
        End Property

        Private Sub m_processTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles m_processTimer.Elapsed

            Dim packetIsStale As Boolean
            Dim binaryLength As Integer
            Dim broadcastIP As IPEndPoint
            Dim x, y, z As Integer

            ' Check for non-published samples...
            For x = 0 To m_dataQueue.SampleCount - 1
                With m_dataQueue(x)
                    If Not .Published Then
                        ' Check for non-published packets in this sample...
                        For y = 0 To .Rows.Length - 1
                            With .Rows(y)
                                If Not .Published Then
                                    ' Determine if packet is stale (number of whole seconds past base time are 1 or more...)
                                    packetIsStale = (m_dataQueue.DistanceFromBaseTime(.Timestamp) > 0)

                                    ' We send any non-published data-packets that may be getting stale or any packets that are ready to go
                                    If .ReadyToPublish Or packetIsStale Then
                                        Try
                                            binaryLength = .BinaryLength

                                            ' Send binary image over UDP broadcast channels
                                            For z = 0 To m_broadcastIPs.Length - 1
                                                m_udpClient.Send(.BinaryImage, binaryLength, m_broadcastIPs(z))
                                            Next

                                            .Published = True
                                            m_bytesBroadcasted += binaryLength
                                            m_packetsPublished += 1
                                        Catch ex As Exception
                                            UpdateStatus("Error publishing data packet: " & ex.Message)
                                            .Published = packetIsStale
                                        End Try
                                    End If
                                End If
                            End With
                        Next
                    End If
                End With
            Next

            m_processTimer.Enabled = (m_enabled And m_dataQueue.SampleCount > 0)

        End Sub

        Private Sub m_sweepTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles m_sweepTimer.Elapsed

            If m_enabled Then
                ' Kick-start process timer if needed
                If m_dataQueue.SampleCount > 0 And Not m_processing Then
                    m_processTimer.Enabled = True
                    Processing = True
                End If

                For x As Integer = 0 To m_dataQueue.SampleCount - 1
                    If m_dataQueue(x).Published Then
                        ' We send out a notification that a new sample has been published so that anyone can have a chance
                        ' to perform any last steps with the data before we remove it - in our case the DatAWare
                        ' Aggregator will pick this up and send the averaged sample to the permanent archive...
                        RaiseEvent SamplePublished(m_dataQueue(x))
                    End If
                Next

                m_dataQueue.RemovePublishedSamples()
            ElseIf m_processing Then
                Processing = False
            End If

        End Sub

#Region " IService Component Implementation "


        Public ReadOnly Property Name() As String Implements IServiceComponent.Name
            Get
                Return Me.GetType.Name
            End Get
        End Property

        Public Sub ProcessStateChanged(ByVal NewState As ProcessState) Implements IServiceComponent.ProcessStateChanged

            ' This class executes as a result of a change in process state, so nothing to do...

        End Sub

        Public Sub ServiceStateChanged(ByVal NewState As ServiceState) Implements IServiceComponent.ServiceStateChanged

            ' We respect changes in service state by enabling or disabling our processing state as needed...
            Select Case NewState
                Case ServiceState.Paused
                    Enabled = False
                Case ServiceState.Resumed
                    Enabled = True
            End Select

        End Sub

        Public ReadOnly Property Status() As String Implements IServiceComponent.Status
            Get
                With New StringBuilder
                    .Append("  Concentration process is: " & IIf(Enabled, "Enabled", "Disabled") & vbCrLf)
                    .Append("  Current processing state: " & IIf(Processing, "Executing", "Idle") & vbCrLf)
                    .Append("    Total process run time: " & SecondsToText(RunTime) & vbCrLf)
                    .Append("          Broadcast volume: " & (m_bytesBroadcasted / 1024 / 1024).ToString("0.00") & " Mb" & vbCrLf)
                    .Append("            Broadcast rate: " & (m_bytesBroadcasted * 8 / RunTime / 1024 / 1024).ToString("0.00") & " mbps" & vbCrLf)
                    .Append("         Samples published: " & m_packetsPublished / m_configFile.SampleRate & vbCrLf)
                    .Append("               Sample rate: " & (m_packetsPublished / m_configFile.SampleRate / RunTime).ToString("0.00") & " samples/sec" & vbCrLf)

                    Return .ToString()
                End With
            End Get
        End Property

#End Region

    End Class

End Namespace