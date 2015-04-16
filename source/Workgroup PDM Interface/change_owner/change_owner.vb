Imports System.Text.RegularExpressions
Imports System.Xml

Imports PDMWorks

Module change_owner

    Sub Main()
        If My.Application.CommandLineArgs.Count < 2 Then
            Console.WriteLine("Usage: change_owner.exe Project Name [Owner]")
            Return
        End If

        Dim project = My.Application.CommandLineArgs(0)
        Dim filename As String = My.Application.CommandLineArgs(1)
        Dim pattern As String = "^" + Regex.Escape(filename).Replace("\*", ".*") + "(|\.sldprt|\.slddrw|\.sldasm)$"
        Dim matcher As Regex = New Regex(pattern, RegexOptions.IgnoreCase)
        Dim owner As String = Nothing
        If My.Application.CommandLineArgs.Count > 2 Then
            owner = My.Application.CommandLineArgs(2)
        End If
        If project.Equals("*") Then
            project = Nothing
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
            Dim documents As IPDMWDocuments = connection.Documents(project)
            For Each document As IPDMWDocument In documents
                If matcher.IsMatch(document.Name) Then
                    If document.Owner.Equals(username) Then
                        document.ReleaseOwnership()
                    End If
                    If owner Is Nothing Then
                        document.ChangeOwner(username)
                        document.ReleaseOwnership()
                    Else
                        document.ChangeOwner(owner)
                    End If
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Environment.ExitCode = 1
        Finally
            connection.Logout()
        End Try
    End Sub

End Module
