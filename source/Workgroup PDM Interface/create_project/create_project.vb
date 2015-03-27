Imports System.Xml

Imports PDMWorks

Module create_project

    Sub Main()
        If My.Application.CommandLineArgs.Count < 2 Then
            Console.WriteLine("Usage: create_project.exe Name Description [Parent]")
            Return
        End If

        Dim name As String = My.Application.CommandLineArgs(0)
        Dim description As String = My.Application.CommandLineArgs(1)
        Dim parent As String = Nothing
        If My.Application.CommandLineArgs.Count > 2 Then
            parent = My.Application.CommandLineArgs(2)
        End If

        Dim host As String = "localhost"
        Dim username As String = "pdmwadmin"
        Dim password As String = "pdmwadmin"
        Dim reader As XmlTextReader = New XmlTextReader("WPDM.config")
        reader.WhitespaceHandling = WhitespaceHandling.None
        reader.Read()
        reader.Read()
        While Not reader.EOF
            reader.Read()
            If reader.Name.Equals("WPDMHost") Then
                reader.Read()
                host = reader.Value
                reader.Read()
            ElseIf reader.Name.Equals("WPDMUsername") Then
                reader.Read()
                username = reader.Value
                reader.Read()
            ElseIf reader.Name.Equals("WPDMPassword") Then
                reader.Read()
                password = reader.Value
                reader.Read()
            End If
        End While

        Dim connection As PDMWConnection = CreateObject("PDMWorks.PDMWConnection")
        Try
            connection.Login(username, password, host)
            connection.CreateProject(name, description, parent)
        Catch ex As Exception
            Debug.Print(ex.Message)
            Environment.ExitCode = 1
        Finally
            connection.Logout()
        End Try
    End Sub

End Module
