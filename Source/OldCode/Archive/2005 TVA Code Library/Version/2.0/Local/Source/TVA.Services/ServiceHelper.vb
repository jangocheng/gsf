' 08-29-06

Imports System.Text
Imports System.Drawing
Imports System.ComponentModel
Imports System.ServiceProcess
Imports TVA.IO
Imports TVA.Common
Imports TVA.Communication
Imports TVA.Serialization
Imports TVA.Scheduling
Imports TVA.Configuration
Imports TVA.Configuration.Common

<ToolboxBitmap(GetType(ServiceHelper))> _
Public Class ServiceHelper
    Implements IPersistSettings, ISupportInitialize

#Region " Member Declaration "

    Private m_service As ServiceBase
    Private m_logStatusUpdates As Boolean
    Private m_requestHistoryLimit As Integer
    Private m_queryableSettingsCategories As String
    Private m_lastClientRequest As ClientRequest
    Private m_processes As List(Of ServiceProcess)
    Private m_serviceComponents As List(Of IServiceComponent)
    Private m_persistsettings As Boolean
    Private m_settingsCategoryName As String

    Private m_connectedClients As List(Of ClientInfo)
    Private m_clientRequestHistory As List(Of ClientRequestInfo)
    Private m_clientRequestHandlers As List(Of ClientRequestHandlerInfo)

    Private m_startedEventHandlerList As List(Of StartedEventHandler)
    Private m_stoppedEventHandlerList As List(Of EventHandler)

    Private WithEvents m_communicationServer As ICommunicationServer

#End Region

#Region " Event Declaration "

    ''' <summary>
    ''' Occurs when the service has started.
    ''' </summary>
    ''' <remarks>This is a non-blocking event.</remarks>
    Public Custom Event Started As StartedEventHandler
        AddHandler(ByVal value As StartedEventHandler)
            m_startedEventHandlerList.Add(value)
        End AddHandler

        RemoveHandler(ByVal value As StartedEventHandler)
            m_startedEventHandlerList.Remove(value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As GenericEventArgs(Of Object()))
            For Each handler As StartedEventHandler In m_startedEventHandlerList
                handler.BeginInvoke(sender, e, Nothing, Nothing)
            Next
        End RaiseEvent
    End Event

    ''' <summary>
    ''' Occurs when the service has stopped.
    ''' </summary>
    ''' <remarks>This is a non-blocking event.</remarks>
    Public Custom Event Stopped As EventHandler
        AddHandler(ByVal value As EventHandler)
            m_stoppedEventHandlerList.Add(value)
        End AddHandler

        RemoveHandler(ByVal value As EventHandler)
            m_stoppedEventHandlerList.Remove(value)
        End RemoveHandler

        RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)
            For Each handler As EventHandler In m_stoppedEventHandlerList
                handler.BeginInvoke(sender, e, Nothing, Nothing)
            Next
        End RaiseEvent
    End Event

    ''' <summary>
    ''' Occurs when the service is paused.
    ''' </summary>
    Public Event Paused As EventHandler

    ''' <summary>
    ''' Occurs when the service is resumed.
    ''' </summary>
    Public Event Resumed As EventHandler

    ''' <summary>
    ''' Occurs when the system is being shutdowm.
    ''' </summary>
    Public Event Shutdown As EventHandler

    ''' <summary>
    ''' Occurs when a request is received from a client.
    ''' </summary>
    Public Event ReceivedClientRequest(ByVal sender As Object, ByVal e As ClientRequestEventArgs)

#End Region

#Region " Code Scope: Public "

    Public Delegate Sub StartedEventHandler(ByVal sender As Object, ByVal e As GenericEventArgs(Of Object()))

    ''' <summary>
    ''' Gets or sets the parent service to which the service helper belongs.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The parent service to which the service helper belongs.</returns>
    Public Property Service() As ServiceBase
        Get
            Return m_service
        End Get
        Set(ByVal value As ServiceBase)
            m_service = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the instance of TCP server used for communicating with the clients.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of TCP server.</returns>
    Public Property CommunicationServer() As ICommunicationServer
        Get
            Return m_communicationServer
        End Get
        Set(ByVal value As ICommunicationServer)
            m_communicationServer = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a boolean value indicating whether status updates are to be logged to a text file.
    ''' </summary>
    ''' <value></value>
    ''' <returns>True if status updates are to be logged to a text file; otherwise false.</returns>
    <Category("Service Helper"), DefaultValue(GetType(Boolean), "True")> _
    Public Property LogStatusUpdates() As Boolean
        Get
            Return m_logStatusUpdates
        End Get
        Set(ByVal value As Boolean)
            m_logStatusUpdates = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the number of request entries to be kept in the history.
    ''' </summary>
    ''' <value></value>
    ''' <returns>The number of request entries to be kept in the history.</returns>
    <Category("Service Helper"), DefaultValue(GetType(Integer), "50")> _
    Public Property RequestHistoryLimit() As Integer
        Get
            Return m_requestHistoryLimit
        End Get
        Set(ByVal value As Integer)
            If value > 0 Then
                m_requestHistoryLimit = value
            Else
                Throw New ArgumentOutOfRangeException("RequestHistoryLimit", "Value must be greater that 0.")
            End If
        End Set
    End Property

    Public Property QueryableSettingsCategories() As String
        Get
            Return m_queryableSettingsCategories
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                m_queryableSettingsCategories = value
            Else
                Throw New ArgumentNullException("QueryableSettingsCategories")
            End If
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property LastClientRequest() As ClientRequest
        Get
            Return m_lastClientRequest
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Processes() As List(Of ServiceProcess)
        Get
            Return m_processes
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ConnectedClients() As List(Of ClientInfo)
        Get
            Return m_connectedClients
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ClientRequestHistory() As List(Of ClientRequestInfo)
        Get
            Return m_clientRequestHistory
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ClientRequestHandlers() As List(Of ClientRequestHandlerInfo)
        Get
            Return m_clientRequestHandlers
        End Get
    End Property

    ''' <summary>
    ''' Gets a list of all the components that implement the TVA.Services.IServiceComponent interface.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of System.Collections.Generic.List(Of TVA.Services.IServiceComponent).</returns>
    <Browsable(False)> _
    Public ReadOnly Property ServiceComponents() As List(Of IServiceComponent)
        Get
            Return m_serviceComponents
        End Get
    End Property

    ''' <summary>
    ''' To be called when the service is starts (inside the service's OnStart method).
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub OnStart(ByVal args As String())

        If m_service IsNot Nothing Then
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Clients", "Displays list of clients connected to the service", AddressOf ListClients))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Settings", "Displays queryable service settings from config file", AddressOf ListSettings))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Processes", "Displays list of service or system processes", AddressOf ListProcesses))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Schedules", "Displays list of process schedules defined in the service", AddressOf ListSchedules))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("History", "Displays list of requests received from the clients", AddressOf ListRequestHistory))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Help", "Displays list of commands supported by the service", AddressOf ListRequestHelp))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Start", "Start a service or system process", AddressOf StartProcess))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Abort", "Aborts a service or system process", AddressOf AbortProcess))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("UpdateSettings", "Updates service setting in the config file", AddressOf UpdateSettings))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("ReloadSettings", "Reloads services settings from the config file", AddressOf ReloadSettings))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Reschedule", "Reschedules a process defined in the service", AddressOf RescheduleProcess))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Unschedule", "Unschedules a process defined in the service", AddressOf UnscheduleProcess))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("SaveSchedules", "Saves process schedules to the config file", AddressOf SaveSchedules))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("LoadSchedules", "Loads process schedules from the config file", AddressOf LoadSchedules))
            m_clientRequestHandlers.Add(New ClientRequestHandlerInfo("Status", "Displays the current service status", AddressOf GetServiceStatus))

            m_serviceComponents.Add(ScheduleManager)
            m_serviceComponents.Add(m_communicationServer)

            If m_communicationServer IsNot Nothing Then
                m_communicationServer.Handshake = True
                m_communicationServer.HandshakePassphrase = m_service.ServiceName
            End If

            For Each component As IServiceComponent In m_serviceComponents
                If component IsNot Nothing Then component.ServiceStateChanged(ServiceState.Started)
            Next

            If m_logStatusUpdates Then LogFile.Open()

            RaiseEvent Started(Me, New GenericEventArgs(Of Object())(args))

            SendServiceStateChangedResponse(ServiceState.Started)
        Else
            Throw New InvalidOperationException("Service cannot be started. The Service property of ServiceHelper is not set.")
        End If

    End Sub

    ''' <summary>
    ''' To be called when the service is stopped (inside the service's OnStop method).
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub OnStop()

        SendServiceStateChangedResponse(ServiceState.Stopped)

        ' Abort any processes that are currently executing.
        For Each process As ServiceProcess In m_processes
            If process IsNot Nothing Then process.Abort()
        Next

        ' Notify all of the components that the service is stopping.
        For Each component As IServiceComponent In m_serviceComponents
            If component IsNot Nothing Then component.ServiceStateChanged(ServiceState.Stopped)
        Next

        If LogFile.IsOpen Then LogFile.Close()

        RaiseEvent Stopped(Me, EventArgs.Empty)

    End Sub

    ''' <summary>
    ''' To be called when the service is paused (inside the service's OnPause method).
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub OnPause()

        For Each component As IServiceComponent In m_serviceComponents
            If component IsNot Nothing Then component.ServiceStateChanged(ServiceState.Paused)
        Next

        RaiseEvent Paused(Me, EventArgs.Empty)

        SendServiceStateChangedResponse(ServiceState.Paused)

    End Sub

    ''' <summary>
    ''' To be called when the service is resumed (inside the service's OnContinue method).
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub OnResume()

        For Each component As IServiceComponent In m_serviceComponents
            If component IsNot Nothing Then component.ServiceStateChanged(ServiceState.Resumed)
        Next

        RaiseEvent Resumed(Me, EventArgs.Empty)

        SendServiceStateChangedResponse(ServiceState.Resumed)

    End Sub

    ''' <summary>
    ''' To be when the system is shutting down (inside the service's OnShutdown method).
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub OnShutdown()

        SendServiceStateChangedResponse(ServiceState.Shutdown)

        ' Abort any processes that are executing.
        For Each process As ServiceProcess In m_processes
            If process IsNot Nothing Then process.Abort()
        Next

        ' Stop all of the components that implement IServiceComponent interface.
        For Each component As IServiceComponent In m_serviceComponents
            If component IsNot Nothing Then component.ServiceStateChanged(ServiceState.Shutdown)
        Next

        RaiseEvent Shutdown(Me, EventArgs.Empty)

    End Sub

    ''' <summary>
    ''' To be called when the state of a process changes.
    ''' </summary>
    ''' <param name="processName">Name of the process whose state changed.</param>
    ''' <param name="processState">New state of the process.</param>
    Public Sub ProcessStateChanged(ByVal processName As String, ByVal processState As ProcessState)

        For Each component As IServiceComponent In m_serviceComponents
            If component IsNot Nothing Then component.ProcessStateChanged(processName, processState)
        Next

        SendProcessStateChangedResponse(processName, processState)

    End Sub

    ''' <summary>
    ''' Sends the specified response to all of the connected clients.
    ''' </summary>
    ''' <param name="response">The response to be sent to the clients.</param>
    Public Sub SendResponse(ByVal response As ServiceResponse)

        m_communicationServer.Multicast(response)

    End Sub

    ''' <summary>
    ''' Sends the specified resonse to the specified client only.
    ''' </summary>
    ''' <param name="clientID">ID of the client to whom the response is to be sent.</param>
    ''' <param name="response">The response to be sent to the client.</param>
    Public Sub SendResponse(ByVal clientID As Guid, ByVal response As ServiceResponse)

        m_communicationServer.SendTo(clientID, response)

    End Sub

    Public Sub UpdateStatus(ByVal message As String)

        UpdateStatus(message, 1)

    End Sub

    Public Sub UpdateStatus(ByVal message As String, ByVal crlfCount As Integer)

        With New StringBuilder()
            .Append(message)

            For i As Integer = 0 To crlfCount - 1
                .AppendLine()
            Next

            ' Send the status update to all connected clients.
            SendUpdateClientStatusResponse(.ToString())

            ' Log the status update to the log file if logging is enabled.
            If m_logStatusUpdates Then
                If Not LogFile.IsOpen Then
                    LogFile.Open()
                End If
                LogFile.WriteTimestampedLine(.ToString())
            End If
        End With

    End Sub

    Public Sub AddProcess(ByVal processExecutionMethod As ServiceProcess.ExecutionMethodSignature, _
        ByVal processName As String)

        AddProcess(processExecutionMethod, processName, Nothing)

    End Sub

    Public Sub AddProcess(ByVal processExecutionMethod As ServiceProcess.ExecutionMethodSignature, _
            ByVal processName As String, ByVal processParameters As Object())

        processName = processName.Trim()

        If FindProcess(processName) Is Nothing Then
            m_processes.Add(New ServiceProcess(processExecutionMethod, processName, processParameters, Me))
        Else
            Throw New InvalidOperationException(String.Format("Process ""{0}"" is already defined.", processName))
        End If

    End Sub

    Public Sub AddScheduledProcess(ByVal processExecutionMethod As ServiceProcess.ExecutionMethodSignature, _
            ByVal processName As String, ByVal processSchedule As String)

        AddScheduledProcess(processExecutionMethod, processName, Nothing, processSchedule)

    End Sub

    Public Sub AddScheduledProcess(ByVal processExecutionMethod As ServiceProcess.ExecutionMethodSignature, _
            ByVal processName As String, ByVal processParameters As Object(), ByVal processSchedule As String)

        AddProcess(processExecutionMethod, processName, processParameters)
        ScheduleProcess(processName, processSchedule)

    End Sub

    Public Sub ScheduleProcess(ByVal processName As String, ByVal scheduleRule As String)

        processName = processName.Trim()

        If FindProcess(processName) IsNot Nothing Then
            ' The specified process exists, so we'll schedule it, or update its schedule if it is acheduled already.
            Dim existingSchedule As Schedule = ScheduleManager.FindSchedule(processName)

            If existingSchedule IsNot Nothing Then
                ' Update the process schedule if it is already exists.
                existingSchedule.Rule = scheduleRule
            Else
                ' Schedule the process if it is not scheduled already.
                ScheduleManager.Schedules.Add(New Schedule(processName, scheduleRule))
            End If
        Else
            Throw New InvalidOperationException(String.Format("Process ""{0}"" is not defined.", processName))
        End If

    End Sub

    Public Function FindProcess(ByVal processName As String) As ServiceProcess

        Dim match As ServiceProcess = Nothing
        For Each process As ServiceProcess In m_processes
            If String.Compare(process.Name, processName, True) = 0 Then
                match = process
                Exit For
            End If
        Next
        Return match

    End Function

    Public Function FindClient(ByVal clientID As Guid) As ClientInfo

        Dim match As ClientInfo = Nothing
        For Each clientInfo As ClientInfo In m_connectedClients
            If clientID = clientInfo.ClientID Then
                match = clientInfo
                Exit For
            End If
        Next
        Return match

    End Function

    Public Function FindRequestHandler(ByVal requestType As String) As ClientRequestHandlerInfo

        Dim match As ClientRequestHandlerInfo = Nothing
        For Each handler As ClientRequestHandlerInfo In m_clientRequestHandlers
            If String.Compare(handler.Command, requestType, True) = 0 Then
                match = handler
                Exit For
            End If
        Next
        Return match

    End Function

#Region " Interface Implementation "

#Region " IPersistSettings "

    Public Property PersistSettings() As Boolean Implements IPersistSettings.PersistSettings
        Get
            Return m_persistsettings
        End Get
        Set(ByVal value As Boolean)
            m_persistsettings = value
        End Set
    End Property

    Public Property SettingsCategoryName() As String Implements IPersistSettings.SettingsCategoryName
        Get
            Return m_settingsCategoryName
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                m_settingsCategoryName = value
            Else
                Throw New ArgumentNullException("ConfigurationCategory")
            End If
        End Set
    End Property

    Public Sub LoadSettings() Implements IPersistSettings.LoadSettings

        Try
            With TVA.Configuration.Common.CategorizedSettings(m_settingsCategoryName)
                If .Count > 0 Then
                    LogStatusUpdates = .Item("LogStatusUpdates").GetTypedValue(m_logStatusUpdates)
                    RequestHistoryLimit = .Item("RequestHistoryLimit").GetTypedValue(m_requestHistoryLimit)
                    QueryableSettingsCategories = .Item("QueryableSettingsCategories").GetTypedValue(m_queryableSettingsCategories)
                End If
            End With
        Catch ex As Exception
            ' We'll encounter exceptions if the settings are not present in the config file.
        End Try

    End Sub

    Public Sub SaveSettings() Implements IPersistSettings.SaveSettings

        If m_persistsettings Then
            Try
                With TVA.Configuration.Common.CategorizedSettings(m_settingsCategoryName)
                    .Clear()
                    With .Item("LogStatusUpdates", True)
                        .Value = m_logStatusUpdates.ToString()
                        .Description = ""
                    End With
                    With .Item("RequestHistoryLimit", True)
                        .Value = m_requestHistoryLimit.ToString()
                        .Description = ""
                    End With
                    With .Item("QueryableSettingsCategories", True)
                        .Value = m_queryableSettingsCategories
                        .Description = ""
                    End With
                End With
                TVA.Configuration.Common.SaveSettings()
            Catch ex As Exception
                ' We might encounter an exception if for some reason the settings cannot be saved to the config file.
            End Try
        End If

    End Sub

#End Region

#Region " ISupportInitialize "

    Public Sub BeginInit() Implements System.ComponentModel.ISupportInitialize.BeginInit

        ' We don't need to do anything before the component is initialized.

    End Sub

    Public Sub EndInit() Implements System.ComponentModel.ISupportInitialize.EndInit

        If Not DesignMode Then
            LoadSettings()  ' Load settings from the config file.
        End If

    End Sub

#End Region

#End Region

#End Region

#Region " Code Scope: Private "

    Private Sub SendUpdateClientStatusResponse(ByVal response As String)

        Dim serviceResponse As New ServiceResponse()
        serviceResponse.Type = "UPDATECLIENTSTATUS"
        serviceResponse.Message = response
        SendResponse(serviceResponse)

    End Sub

    Private Sub SendServiceStateChangedResponse(ByVal serviceState As ServiceState)

        Dim serviceResponse As New ServiceResponse()
        serviceResponse.Type = "SERVICESTATECHANGED"
        serviceResponse.Message = m_service.ServiceName & ">" & serviceState.ToString()
        SendResponse(serviceResponse)

    End Sub

    Private Sub SendProcessStateChangedResponse(ByVal processName As String, ByVal processState As ProcessState)

        Dim serviceResponse As New ServiceResponse()
        serviceResponse.Type = "PROCESSSTATECHANGED"
        serviceResponse.Message = processName & ">" & processState.ToString()
        SendResponse(serviceResponse)

    End Sub

#Region " Request Handlers "

    Private Sub ListClients()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of clients currently connected to the service.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Clients -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            If m_connectedClients.Count > 0 Then
                ' Display info about all of the clients connected to the service.
                With New StringBuilder()
                    .AppendFormat("Clients connected to {0}:", m_service.ServiceName)
                    .AppendLine()
                    .AppendLine()
                    .Append("Client".PadRight(25))
                    .Append(" ")
                    .Append("Machine".PadRight(15))
                    .Append(" ")
                    .Append("User".PadRight(15))
                    .Append(" ")
                    .Append("Connected".PadRight(20))
                    .AppendLine()
                    .Append(New String("-"c, 25))
                    .Append(" ")
                    .Append(New String("-"c, 15))
                    .Append(" ")
                    .Append(New String("-"c, 15))
                    .Append(" ")
                    .Append(New String("-"c, 20))
                    For Each clientInfo As ClientInfo In m_connectedClients
                        .AppendLine()
                        .Append(clientInfo.ClientName.PadRight(25))
                        .Append(" ")
                        .Append(clientInfo.MachineName.PadRight(15))
                        .Append(" ")
                        If Not String.IsNullOrEmpty(clientInfo.UserName) Then
                            .Append(clientInfo.UserName.PadRight(15))
                        Else
                            .Append("[Not Available]".PadRight(15))
                        End If
                        .Append(" ")
                        .Append(clientInfo.ConnectedAt.ToString("MM/dd/yy hh:mm:ss tt").PadRight(20))
                    Next

                    UpdateStatus(.ToString(), 2)
                End With
            Else
                ' This will never be the case because at the least the client sending the request will be connected.
                UpdateStatus(String.Format("No clients are connected to {0}", m_service.ServiceName), 2)
            End If
        End If

    End Sub

    Private Sub ListSettings()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of queryable settings of the service from the config file.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Settings -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim settingsCategories As String() = m_queryableSettingsCategories.Replace(" ", "").Split(","c)
            If settingsCategories.Length > 0 Then
                ' Display info about all of the queryable settings defined in the service.
                With New StringBuilder()
                    .AppendFormat("Queryable settings of {0}:", m_service.ServiceName)
                    .AppendLine()
                    .AppendLine()
                    .Append("Category".PadRight(25))
                    .Append(" ")
                    .Append("Name".PadRight(20))
                    .Append(" ")
                    .Append("Value".PadRight(30))
                    .AppendLine()
                    .Append(New String("-"c, 25))
                    .Append(" ")
                    .Append(New String("-"c, 20))
                    .Append(" ")
                    .Append(New String("-"c, 30))
                    For Each category As String In settingsCategories
                        For Each setting As CategorizedSettingsElement In CategorizedSettings(category)
                            .AppendLine()
                            .Append(category.PadRight(25))
                            .Append(" ")
                            .Append(setting.Name.PadRight(20))
                            .Append(" ")
                            If Not String.IsNullOrEmpty(setting.Value) Then
                                .Append(setting.Value.PadRight(30))
                            Else
                                .Append("[Not Set]".PadRight(30))
                            End If
                        Next
                    Next

                    UpdateStatus(.ToString(), 2)
                End With
            Else
                ' No queryable settings are defined in the service.
                UpdateStatus(String.Format("No queryable settings are defined in {0}.", m_service.ServiceName), 2)
            End If
        End If

    End Sub

    Private Sub ListProcesses()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of defined service processes or running system processes.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Processes -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -system".PadRight(20))
                .Append("Displays system processes instead of service processes")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim listSystemProcesses As Boolean = m_lastClientRequest.Arguments.Exists("system")

            If Not listSystemProcesses Then
                If m_processes.Count > 0 Then
                    ' Display info about all the processes defined in the service.
                    With New StringBuilder()
                        .AppendFormat("Processes defined in {0}:", m_service.ServiceName)
                        .AppendLine()
                        .AppendLine()
                        .Append("Name".PadRight(20))
                        .Append(" ")
                        .Append("State".PadRight(15))
                        .Append(" ")
                        .Append("Last Exec. Start".PadRight(20))
                        .Append(" ")
                        .Append("Last Exec. Stop".PadRight(20))
                        .AppendLine()
                        .Append(New String("-"c, 20))
                        .Append(" ")
                        .Append(New String("-"c, 15))
                        .Append(" ")
                        .Append(New String("-"c, 20))
                        .Append(" ")
                        .Append(New String("-"c, 20))
                        For Each process As ServiceProcess In m_processes
                            .AppendLine()
                            .Append(process.Name.PadRight(20))
                            .Append(" ")
                            .Append(process.CurrentState.ToString().PadRight(15))
                            .Append(" ")
                            If process.ExecutionStartTime <> Date.MinValue Then
                                .Append(process.ExecutionStartTime.ToString("MM/dd/yy hh:mm:ss tt").PadRight(20))
                            Else
                                .Append("[Not Executed]".PadRight(20))
                            End If
                            .Append(" ")
                            If process.ExecutionStopTime <> Date.MinValue Then
                                .Append(process.ExecutionStopTime.ToString("MM/dd/yy hh:mm:ss tt").PadRight(20))
                            Else
                                If process.ExecutionStartTime <> Date.MinValue Then
                                    .Append("[Executing]".PadRight(20))
                                Else
                                    .Append("[Not Executed]".PadRight(20))
                                End If
                            End If
                        Next

                        UpdateStatus(.ToString(), 2)
                    End With
                Else
                    ' No processes defined in the service to be displayed.
                    UpdateStatus(String.Format("No processes are defined in {0}.", m_service.ServiceName), 2)
                End If
            Else
                With New StringBuilder()
                    .AppendFormat("Processes running on {0}:", Environment.MachineName)
                    .AppendLine()
                    .AppendLine()
                    .Append("ID".PadRight(5))
                    .Append(" ")
                    .Append("Name".PadRight(25))
                    .Append(" ")
                    .Append("Priority".PadRight(15))
                    .Append(" ")
                    .Append("Responding".PadRight(10))
                    .Append(" ")
                    .Append("Start Time".PadRight(20))
                    .AppendLine()
                    .Append(New String("-"c, 5))
                    .Append(" ")
                    .Append(New String("-"c, 25))
                    .Append(" ")
                    .Append(New String("-"c, 15))
                    .Append(" ")
                    .Append(New String("-"c, 10))
                    .Append(" ")
                    .Append(New String("-"c, 20))
                    For Each process As Process In process.GetProcesses()
                        Try
                            .Append(process.StartInfo.UserName)
                            .AppendLine()
                            .Append(process.Id.ToString().PadRight(5))
                            .Append(" ")
                            .Append(process.ProcessName.PadRight(25))
                            .Append(" ")
                            .Append(process.PriorityClass.ToString().PadRight(15))
                            .Append(" ")
                            .Append(IIf(process.Responding, "Yes", "No").PadRight(10))
                            .Append(" ")
                            .Append(process.StartTime.ToString("MM/dd/yy hh:mm:ss tt").PadRight(20))
                        Catch ex As Exception

                        End Try
                    Next

                    UpdateStatus(.ToString(), 2)
                End With
            End If
        End If

    End Sub

    Private Sub ListSchedules()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of schedules for processes defined in the service.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Schedules -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            If ScheduleManager.Schedules.Count > 0 Then
                ' Display info about all the process schedules defined in the service.
                With New StringBuilder()
                    .AppendFormat("Process schedules defined in {0}:", m_service.ServiceName)
                    .AppendLine()
                    .AppendLine()
                    .Append("Name".PadRight(20))
                    .Append(" ")
                    .Append("Rule".PadRight(20))
                    .Append(" ")
                    .Append("Last Due".PadRight(30))
                    .AppendLine()
                    .Append(New String("-"c, 20))
                    .Append(" ")
                    .Append(New String("-"c, 20))
                    .Append(" ")
                    .Append(New String("-"c, 30))
                    For Each schedule As Schedule In ScheduleManager.Schedules
                        .AppendLine()
                        .Append(schedule.Name.PadRight(20))
                        .Append(" ")
                        .Append(schedule.Rule.PadRight(20))
                        .Append(" ")
                        If schedule.LastDueAt <> Date.MinValue Then
                            .Append(schedule.LastDueAt.ToString().PadRight(30))
                        Else
                            .Append("[Never]".PadRight(30))
                        End If
                    Next

                    UpdateStatus(.ToString(), 2)
                End With
            Else
                UpdateStatus(String.Format("No process schedules are defined in {0}.", m_service.ServiceName), 2)
            End If
        End If

    End Sub

    Private Sub ListRequestHistory()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of recent requests received from the clients.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       History -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            With New StringBuilder()
                .AppendFormat("History of requests received by {0}:", m_service.ServiceName)
                .AppendLine()
                .AppendLine()
                .Append("Command".PadRight(20))
                .Append(" ")
                .Append("Received".PadRight(25))
                .Append(" ")
                .Append("Sender".PadRight(30))
                .AppendLine()
                .Append(New String("-"c, 20))
                .Append(" ")
                .Append(New String("-"c, 25))
                .Append(" ")
                .Append(New String("-"c, 30))
                For Each request As ClientRequestInfo In m_clientRequestHistory
                    .AppendLine()
                    .Append(request.Command.PadRight(20))
                    .Append(" ")
                    .Append(request.ReceivedAt.ToString().PadRight(25))
                    .Append(" ")
                    ' For some reason StringBuilder.AppendFormat() doesn't work as expected with String.PadRight()
                    .Append((request.Sender.UserName & " from " & request.Sender.MachineName).PadRight(30))
                Next

                UpdateStatus(.ToString(), 2)
            End With
        End If

    End Sub

    Private Sub ListRequestHelp()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Displays a list of commands supported by the service.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Help -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            With New StringBuilder()
                .AppendFormat("Commands supported by {0}:", m_service.ServiceName)
                .AppendLine()
                .AppendLine()
                .Append("Command".PadRight(20))
                .Append(" ")
                .Append("Description".PadRight(55))
                .AppendLine()
                .Append(New String("-"c, 20))
                .Append(" ")
                .Append(New String("-"c, 55))
                For Each handler As ClientRequestHandlerInfo In m_clientRequestHandlers
                    If handler.IsAdvertised Then
                        .AppendLine()
                        .Append(handler.Command.PadRight(20))
                        .Append(" ")
                        .Append(handler.CommandDescription.PadRight(55))
                    End If
                Next

                UpdateStatus(.ToString(), 2)
            End With
        End If

    End Sub

    Private Sub ReloadSettings()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 1 Then
            With New StringBuilder()
                .Append("Reloads settings of the component whose settings are saved under the specified category in the config file.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       ReloadSettings ""Category Name"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .AppendLine()
                .Append("IMPORTANT: Category name must be defined as one of the queryable settings categories in the QueryableSettingsCategories property of ServiceHelper. ")
                .Append("Also, category name is case sensitive so it must be the same case as it appears in the settings listing.")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim targetComponent As Component = Nothing
            Dim categoryName As String = m_lastClientRequest.Arguments("orderedarg1")
            If m_queryableSettingsCategories.IndexOf(categoryName) >= 0 Then
                If m_settingsCategoryName = categoryName Then
                    LoadSettings()
                    targetComponent = Me
                Else
                    If targetComponent IsNot Nothing Then
                        For Each component As Component In components.Components
                            Dim reloadableComponent As IPersistSettings = TryCast(component, IPersistSettings)
                            If reloadableComponent IsNot Nothing AndAlso _
                                    reloadableComponent.SettingsCategoryName = categoryName Then
                                reloadableComponent.LoadSettings()
                                targetComponent = component
                                Exit For
                            End If
                        Next
                    End If

                    If targetComponent IsNot Nothing Then
                        For Each component As Component In m_serviceComponents
                            Dim reloadableComponent As IPersistSettings = TryCast(component, IPersistSettings)
                            If reloadableComponent IsNot Nothing AndAlso _
                                    reloadableComponent.SettingsCategoryName = categoryName Then
                                reloadableComponent.LoadSettings()
                                targetComponent = component
                                Exit For
                            End If
                        Next
                    End If
                End If

                If targetComponent IsNot Nothing Then
                    UpdateStatus(String.Format("Successfully loaded settings for component ""{0}"" from category ""{1}"".", targetComponent.GetType().Name, categoryName), 2)
                Else
                    UpdateStatus(String.Format("Failed to load component settings from category ""{0}"" - No corresponding component found.", categoryName), 2)
                End If
            Else
                UpdateStatus(String.Format("Failed to load component settings from category ""{0}"" - Category is not one of the queryable categories.", categoryName), 2)
            End If
        End If

    End Sub

    Private Sub UpdateSettings()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 3 Then
            ' We'll display help about the request since we either don't have the required arguments or the user
            ' has explicitly requested for the help to be displayed for this request type.
            With New StringBuilder()
                .Append("Updates the specified setting under the specified category in the config file.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       UpdateSettings ""Category Name"" ""Setting Name"" ""Setting Value"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -add".PadRight(20))
                .Append("Adds specified setting to the specified category")
                .AppendLine()
                .Append("       -delete".PadRight(20))
                .Append("Deletes specified setting from the specified category")
                .AppendLine()
                .Append("       -reload".PadRight(20))
                .Append("Causes corresponding component to reload settings")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list all of the queryable settings")
                .AppendLine()
                .AppendLine()
                .Append("IMPORTANT: Category name must be defined as one of the queryable settings categories in the QueryableSettingsCategories property of ServiceHelper. ")
                .Append("Also, category and setting names are case sensitive so they must be the same case as they appears in the settings listing.")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim categoryName As String = m_lastClientRequest.Arguments("orderedarg1")
            Dim settingName As String = m_lastClientRequest.Arguments("orderedarg2")
            Dim settingValue As String = m_lastClientRequest.Arguments("orderedarg3")
            Dim addSetting As Boolean = m_lastClientRequest.Arguments.Exists("add")
            Dim deleteSetting As Boolean = m_lastClientRequest.Arguments.Exists("delete")
            Dim doReloadSettings As Boolean = m_lastClientRequest.Arguments.Exists("reload")
            Dim doListSettings As Boolean = m_lastClientRequest.Arguments.Exists("list")

            If m_queryableSettingsCategories.IndexOf(categoryName) >= 0 Then
                ' The specified category is one of the defined queryable categories.
                Select Case True
                    Case addSetting
                        UpdateStatus(String.Format("Attempting to add setting ""{0}"" under category ""{1}""...", settingName, categoryName), 2)
                        CategorizedSettings(categoryName).Add(settingName, settingValue)
                        TVA.Configuration.Common.SaveSettings()
                        UpdateStatus(String.Format("Successfully added setting ""{0}"" under category ""{1}"".", settingName, categoryName), 2)
                    Case deleteSetting
                        Dim setting As CategorizedSettingsElement = CategorizedSettings(categoryName)(settingName)
                        If setting IsNot Nothing Then
                            UpdateStatus(String.Format("Attempting to delete setting ""{0}"" under category ""{1}""...", settingName, categoryName), 2)
                            CategorizedSettings(categoryName).Remove(setting)
                            TVA.Configuration.Common.SaveSettings()
                            UpdateStatus(String.Format("Successfully deleted setting ""{0}"" under category ""{1}"".", settingName, categoryName), 2)
                        Else
                            UpdateStatus(String.Format("Failed to delete setting ""{0}"" under category ""{1}"" - Setting does not exist.", settingName, categoryName), 2)
                        End If
                    Case Else
                        Dim setting As CategorizedSettingsElement = CategorizedSettings(categoryName)(settingName)
                        If setting IsNot Nothing Then
                            ' The requested setting does exist under the specified category.
                            UpdateStatus(String.Format("Attempting to update setting ""{0}"" under category ""{1}""...", settingName, categoryName), 2)
                            setting.Value = settingValue
                            TVA.Configuration.Common.SaveSettings()
                            UpdateStatus(String.Format("Successfully updated setting ""{0}"" under category ""{1}"".", settingName, categoryName), 2)
                        Else
                            ' The requested setting does not exist under the specified category.
                            UpdateStatus(String.Format("Failed to update value of setting ""{0}"" under category ""{1}"" - Setting does not exist.", settingName, categoryName), 2)
                        End If
                End Select

                If doReloadSettings Then
                    ' The user has requested to reload settings for all the components.
                    Dim originalRequest As ClientRequest = m_lastClientRequest
                    m_lastClientRequest = ClientRequest.Parse(String.Format("reloadsettings {0}", categoryName))
                    ReloadSettings()
                    m_lastClientRequest = originalRequest
                End If

                If doListSettings Then
                    ' The user has requested to list all of the queryable settings.
                    Dim originalRequest As ClientRequest = m_lastClientRequest
                    m_lastClientRequest = ClientRequest.Parse("Settings")
                    ListSettings()
                    m_lastClientRequest = originalRequest
                End If
            Else
                ' The specified category is not one of the defined queryable categories.
                UpdateStatus(String.Format("Failed to update value of setting ""{0}"" under category ""{1}"" - Category is not one of the queryable categories.", settingName, categoryName), 2)
            End If
        End If

    End Sub

    Private Sub StartProcess()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 1 Then
            With New StringBuilder()
                .Append("Starts execution of the specified service or system process.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Start ""Process Name"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -system".PadRight(20))
                .Append("Treats the specified process as a system process")
                .AppendLine()
                .Append("       -restart".PadRight(20))
                .Append("Aborts the process if executing and start it again")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all service or system processes")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim processName As String = m_lastClientRequest.Arguments("orderedarg1")
            Dim isSystemProcess As Boolean = m_lastClientRequest.Arguments.Exists("system")
            Dim doRestartProcess As Boolean = m_lastClientRequest.Arguments.Exists("restart")
            Dim doListProcesses As Boolean = m_lastClientRequest.Arguments.Exists("list")

            If doRestartProcess Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse(String.Format("Abort ""{0}"" -{1}", processName, IIf(isSystemProcess, "system", "")))
                AbortProcess()
                m_lastClientRequest = originalRequest
            End If

            If Not isSystemProcess Then
                Dim processToStart As ServiceProcess = FindProcess(processName)

                If processToStart IsNot Nothing Then
                    If processToStart.CurrentState <> ProcessState.Processing Then
                        UpdateStatus(String.Format("Attempting to start service process ""{0}""...", processName), 2)
                        processToStart.Start()
                        UpdateStatus(String.Format("Successfully started service process ""{0}"".", processName), 2)
                    Else
                        UpdateStatus(String.Format("Failed to start process ""{0}"" - Process is already executing.", processName), 2)
                    End If
                Else
                    UpdateStatus(String.Format("Failed to start service process ""{0}"" - Process is not defined.", processName), 2)
                End If
            Else
                Try
                    UpdateStatus(String.Format("Attempting to start system process ""{0}""...", processName), 2)
                    Dim startedProcess As Process = Process.Start(processName)
                    If startedProcess IsNot Nothing Then
                        UpdateStatus(String.Format("Successfully started system process ""{0}"".", processName), 2)
                    Else
                        UpdateStatus(String.Format("Failed to start system process ""{0}"" - Reason unknown.", processName), 2)
                    End If
                Catch ex As Exception
                    UpdateStatus(String.Format("Failed to start system process ""{0}"" - {1}.", processName, ex.Message), 2)
                End Try
            End If

            If doListProcesses Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse(String.Format("Processes -{0}", IIf(isSystemProcess, "system", "")))
                ListProcesses()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub AbortProcess()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 1 Then
            With New StringBuilder()
                .Append("Aborts the specified service or system process if executing.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Abort ""Process Name"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -system".PadRight(20))
                .Append("Treats the specified process as a system process")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all service or system processes")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim processName As String = m_lastClientRequest.Arguments("orderedarg1")
            Dim isSystemProcess As Boolean = m_lastClientRequest.Arguments.Exists("system")
            Dim doListProcesses As Boolean = m_lastClientRequest.Arguments.Exists("list")

            If Not isSystemProcess Then
                Dim processToAbort As ServiceProcess = FindProcess(processName)

                If processToAbort IsNot Nothing Then
                    If processToAbort.CurrentState = ProcessState.Processing Then
                        UpdateStatus(String.Format("Attempting to abort service process ""{0}""...", processName), 2)
                        processToAbort.Abort()
                        UpdateStatus(String.Format("Successfully aborted service process ""{0}"".", processName), 2)
                    Else
                        UpdateStatus(String.Format("Failed to abort service process ""{0}"" - Process is not executing.", processName), 2)
                    End If
                Else
                    UpdateStatus(String.Format("Failed to abort service process ""{0}"" - Process is not defined.", processName), 2)
                End If
            Else
                Dim processToAbort As Process = Nothing

                If String.Compare(processName, "Me", True) = 0 Then
                    processName = Process.GetCurrentProcess().ProcessName
                End If

                For Each process As Process In process.GetProcessesByName(processName)
                    ' Lookup for the system process by name.
                    processToAbort = process
                    Exit For
                Next

                If processToAbort Is Nothing Then
                    Dim processID As Integer

                    If Integer.TryParse(processName, processID) AndAlso processID > 0 Then
                        processToAbort = Process.GetProcessById(processID)
                    End If
                End If

                If processToAbort IsNot Nothing Then
                    Try
                        UpdateStatus(String.Format("Attempting to abort system process ""{0}""...", processName), 2)
                        processToAbort.Kill()
                        If processToAbort.WaitForExit(10000) Then
                            UpdateStatus(String.Format("Successfully aborted system process ""{0}"".", processName), 2)
                        Else
                            UpdateStatus(String.Format("Failed to abort system process ""{0}"" - Process not responding.", processName), 2)
                        End If
                    Catch ex As Exception
                        UpdateStatus(String.Format("Failed to abort system process ""{0}"" - {1}.", processName, ex.Message), 2)
                    End Try
                Else
                    UpdateStatus(String.Format("Failed to abort system process ""{0}"" - Process is not running.", processName), 2)
                End If
            End If

            If doListProcesses Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse(String.Format("Processes -{0}", IIf(isSystemProcess, "system", "")))
                ListProcesses()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub RescheduleProcess()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 2 Then
            With New StringBuilder()
                .Append("Schedules or re-schedules an existing process defined in the service.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Reschedule ""Process Name"" ""Schedule Rule"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -save".PadRight(20))
                .Append("Saves all process schedules to the config file")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all process schedules")
                .AppendLine()
                .AppendLine()
                .Append("NOTE: The schedule rule is a UNIX ""cron"" style rule which consists of 5 parts (For example, ""* * * * *""). ")
                .Append("Following is a brief description of each of the 5 parts that make up the rule:")
                .AppendLine()
                .AppendLine()
                .Append("   Part 1 - Minute part; value range 0 to 59. ")
                .AppendLine()
                .Append("   Part 2 - Hour part; value range 0 to 23. ")
                .AppendLine()
                .Append("   Part 3 - Day of month part; value range 1 to 31. ")
                .AppendLine()
                .Append("   Part 4 - Month part; value range 1 to 12. ")
                .AppendLine()
                .Append("   Part 5 - Day of week part; value range 0 to 6 (0 = Sunday). ")
                .AppendLine()
                .AppendLine()
                .Append("Following is a description of valid syntax for all parts of the rule:")
                .AppendLine()
                .AppendLine()
                .Append("   *       - Any value in the range for the date-time part.")
                .AppendLine()
                .Append("   */n     - Every nth value for the data-time part.")
                .AppendLine()
                .Append("   n1-n2   - Range of values (inclusive) for the date-time part.")
                .AppendLine()
                .Append("   n1,n2   - 1 or more specific values for the date-time part.")
                .AppendLine()
                .AppendLine()
                .Append("Examples:")
                .AppendLine()
                .AppendLine()
                .Append("   ""* * * * *""   - Process executes every minute.")
                .AppendLine()
                .Append("   ""*/5 * * * *"" - Process executes every 5 minutes.")
                .AppendLine()
                .Append("   ""5 * * * *""   - Process executes 5 past every hour.")
                .AppendLine()
                .Append("   ""0 0 * * *""   - Process executes every day at midnight.")
                .AppendLine()
                .Append("   ""0 0 1 * *""   - Process executes 1st of every month at midnight.")
                .AppendLine()
                .Append("   ""0 0 * * 0""   - Process executes every Sunday at midnight.")
                .AppendLine()
                .Append("   ""0 0 31 12 *"" - Process executes on December 31 at midnight.")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim processName As String = m_lastClientRequest.Arguments("orderedarg1")
            Dim scheduleRule As String = m_lastClientRequest.Arguments("orderedarg2")
            Dim doSaveSchedules As Boolean = m_lastClientRequest.Arguments.Exists("save")
            Dim doListSchedules As Boolean = m_lastClientRequest.Arguments.Exists("list")

            Try
                UpdateStatus(String.Format("Attempting to schedule process ""{0}"" with rule ""{1}""...", processName, scheduleRule), 2)
                ScheduleProcess(processName, scheduleRule)
                UpdateStatus(String.Format("Successfully scheduled process ""{0}"" with rule ""{1}"".", processName, scheduleRule), 2)

                If doSaveSchedules Then
                    Dim originalRequest As ClientRequest = m_lastClientRequest
                    m_lastClientRequest = ClientRequest.Parse("SaveSchedules")
                    SaveSchedules()
                    m_lastClientRequest = originalRequest
                End If
            Catch ex As Exception
                UpdateStatus(String.Format("Failed to schedule process ""{0}"" - {1}", processName, ex.Message), 2)
            End Try

            If doListSchedules Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse("Schedules")
                ListSchedules()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub UnscheduleProcess()

        If m_lastClientRequest.Arguments.ContainsHelpRequest OrElse m_lastClientRequest.Arguments.OrderedArgCount < 1 Then
            With New StringBuilder()
                .Append("Unschedules a scheduled process defined in the service.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       Unschedule ""Process Name"" -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -save".PadRight(20))
                .Append("Saves all process schedules to the config file")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all process schedules")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim processName As String = m_lastClientRequest.Arguments("orderedarg1")
            Dim doSaveSchedules As Boolean = m_lastClientRequest.Arguments.Exists("save")
            Dim doListSchedules As Boolean = m_lastClientRequest.Arguments.Exists("list")
            Dim scheduleToRemove As Schedule = ScheduleManager.FindSchedule(processName)

            If scheduleToRemove IsNot Nothing Then
                UpdateStatus(String.Format("Attempting to unschedule process ""{0}""...", processName), 2)
                ScheduleManager.Schedules.Remove(scheduleToRemove)
                UpdateStatus(String.Format("Successfully unscheduled process ""{0}"".", processName), 2)

                If doSaveSchedules Then
                    Dim originalRequest As ClientRequest = m_lastClientRequest
                    m_lastClientRequest = ClientRequest.Parse("SaveSchedules")
                    SaveSchedules()
                    m_lastClientRequest = originalRequest
                End If
            Else
                UpdateStatus(String.Format("Failed to unschedule process ""{0}"" - Process is not scheduled."))
            End If

            If doListSchedules Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse("Schedules")
                ListSchedules()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub SaveSchedules()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Saves all process schedules to the config file.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       SaveSchedules -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all process schedules")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim doListSchedules As Boolean = m_lastClientRequest.Arguments.Exists("list")

            UpdateStatus("Attempting to save process schedules to the config file...", 2)
            ScheduleManager.SaveSettings()
            UpdateStatus("Successfully saved process schedules to the config file.", 2)

            If doListSchedules Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse("Schedules")
                ListSchedules()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub LoadSchedules()

        If m_lastClientRequest.Arguments.ContainsHelpRequest Then
            With New StringBuilder()
                .Append("Loads all process schedules from the config file.")
                .AppendLine()
                .AppendLine()
                .Append("   Usage:")
                .AppendLine()
                .Append("       LoadSchedules -options")
                .AppendLine()
                .AppendLine()
                .Append("   Options:")
                .AppendLine()
                .Append("       -?".PadRight(20))
                .Append("Displays this help message")
                .AppendLine()
                .Append("       -list".PadRight(20))
                .Append("Displays list of all process schedules")

                UpdateStatus(.ToString(), 2)
            End With
        Else
            Dim doListSchedules As Boolean = m_lastClientRequest.Arguments.Exists("list")

            UpdateStatus("Attempting to load process schedules from the config file...", 2)
            ScheduleManager.LoadSettings()
            UpdateStatus("Successfully loaded process schedules from the config file.", 2)

            If doListSchedules Then
                Dim originalRequest As ClientRequest = m_lastClientRequest
                m_lastClientRequest = ClientRequest.Parse("Schedules")
                ListSchedules()
                m_lastClientRequest = originalRequest
            End If
        End If

    End Sub

    Private Sub GetServiceStatus()

        With New StringBuilder()
            .Append(String.Format("Status of components used by {0}:", m_service.ServiceName))
            .Append(Environment.NewLine)
            For Each serviceComponent As IServiceComponent In m_serviceComponents
                If serviceComponent IsNot Nothing Then
                    .Append(Environment.NewLine)
                    .Append(String.Format("Status of {0}:", serviceComponent.Name))
                    .Append(Environment.NewLine)
                    .Append(serviceComponent.Status)
                End If
            Next

            UpdateStatus(.ToString(), 2)
        End With

    End Sub

    Private Sub InvalidClientRequest()

        UpdateStatus(String.Format("Failed to process request of type ""{0}"" - Request is invalid.", m_lastClientRequest.Command), 2)

    End Sub

#End Region

#Region " Event Handlers "

#Region " CommunicationServer "

    Private Sub m_communicationServer_ClientDisconnected(ByVal sender As Object, ByVal e As IdentifiableSourceEventArgs) Handles m_communicationServer.ClientDisconnected

        m_connectedClients.Remove(FindClient(e.Source))

    End Sub

    Private Sub m_communicationServer_ReceivedClientData(ByVal sender As Object, ByVal e As IdentifiableItemEventArgs(Of Byte())) Handles m_communicationServer.ReceivedClientData

        Dim info As ClientInfo = GetObject(Of ClientInfo)(e.Item)
        Dim request As ClientRequest = GetObject(Of ClientRequest)(e.Item)

        If info IsNot Nothing Then
            ' We've received client information from a recently connected client.
            info.ConnectedAt = Date.Now
            m_connectedClients.Add(info)
        ElseIf request IsNot Nothing Then
            Try
                Dim receivedClientRequestEvent As New ClientRequestEventArgs(e.Source, request)

                ' Log the received request.
                m_lastClientRequest = request
                m_clientRequestHistory.Add(New ClientRequestInfo(request.Command, FindClient(e.Source), Date.Now))
                If m_clientRequestHistory.Count > m_requestHistoryLimit Then
                    ' We'll remove old request entries if we've exceeded the limit for request history.
                    m_clientRequestHistory.RemoveRange(0, (m_clientRequestHistory.Count - m_requestHistoryLimit))
                End If

                ' Notify the consumer about the incoming request from client.
                RaiseEvent ReceivedClientRequest(Me, ReceivedClientRequestEvent)
                If ReceivedClientRequestEvent.Cancel Then Exit Sub

                Dim requestHandler As ClientRequestHandlerInfo = FindRequestHandler(request.Command)
                If requestHandler IsNot Nothing Then
                    requestHandler.HandlerMethod()
                Else
                    InvalidClientRequest()
                End If
            Catch ex As Exception
                UpdateStatus(String.Format("Failed to process request of type ""{0}"" - {1}.", m_lastClientRequest.Command, ex.Message), 2)
            End Try
        Else
            InvalidClientRequest()
        End If

    End Sub

#End Region

#Region " ScheduleManager "

    Private Sub ScheduleManager_ScheduleDue(ByVal sender As Object, ByVal e As ScheduleEventArgs) Handles ScheduleManager.ScheduleDue

        Dim scheduledProcess As ServiceProcess = FindProcess(e.Schedule.Name)
        If scheduledProcess IsNot Nothing Then
            scheduledProcess.Start() ' Start the process execution if it exists.
        End If

    End Sub

#End Region

#Region " LogFile "

    Private Sub LogFile_LogException(ByVal sender As Object, ByVal e As ExceptionEventArgs) Handles LogFile.LogException

        ' We'll let the connected clients know that we encountered an exception while logging the status update.
        m_logStatusUpdates = False
        UpdateStatus(String.Format("Error occurred while logging status update: {0}", e.Exception.ToString()))
        m_logStatusUpdates = True

    End Sub

#End Region

#End Region

#End Region

End Class