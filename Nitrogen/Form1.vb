Imports System.Security.Cryptography
Imports System.Text
Imports System.Net.NetworkInformation
Imports System.Media
Imports System.Threading
Public Class Form1
    Dim player As New SoundPlayer(My.Resources.youtube_LDRNYUucKvA_audio)
    Dim playbackThread As Thread

    Private Function GetMacAddress() As String
        Dim networkInterfaces() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
        For Each netInterface As NetworkInterface In networkInterfaces
            If (netInterface.NetworkInterfaceType = NetworkInterfaceType.Ethernet OrElse
            netInterface.NetworkInterfaceType = NetworkInterfaceType.Wireless80211) AndAlso
            netInterface.OperationalStatus = OperationalStatus.Up Then
                Return netInterface.GetPhysicalAddress().ToString()
            End If
        Next

        Return Nothing
    End Function


    Public Sub DisplayHashedMacAddress()
        Try
            Dim macAddress As String = GetMacAddress()

            If macAddress IsNot Nothing Then
                Dim sha256 As SHA256 = SHA256.Create()
                Dim data As Byte() = Encoding.Default.GetBytes(macAddress)
                Dim hashedBytes As Byte() = sha256.ComputeHash(data)
                Dim sb As New StringBuilder()

                For Each b As Byte In hashedBytes
                    sb.Append(b.ToString("x2")) ' Convert bytes to hexadecimal representation
                Next

                RichTextBox1.Text = sb.ToString()
                Refresh()
            Else
                RichTextBox1.Text = "MAC address not available."
                Refresh()
            End If
        Catch ex As Exception
            RichTextBox1.Text = "Error: " & ex.Message
            Refresh()
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        playbackThread = New Thread(AddressOf PlayBackgroundMusic)
        playbackThread.IsBackground = True
        playbackThread.Start()
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Stop the background thread before closing the form
        playbackThread.Abort() ' Terminate the background thread (not recommended for production use)
    End Sub


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub PlayBackgroundMusic()
        ' Continuously play the audio in a loop
        Do
            player.PlaySync() ' Use PlaySync to block and play continuously
        Loop
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DisplayHashedMacAddress()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RichTextBox2_TextChanged(sender As Object, e As EventArgs)

    End Sub
End Class
