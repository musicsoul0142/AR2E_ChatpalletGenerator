Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports Newtonsoft.Json

Public Class MainWindow
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SkillList As New Dictionary(Of Integer, Dictionary(Of String, String))
        Dim rawstr As String

        Dim ofdJSON As New OpenFileDialog()

        ofdJSON.Filter = "JSONファイル(*.json)|*.json|すべてのファイル(*.*)|*.*"
        ofdJSON.FilterIndex = 1
        ofdJSON.Title = "開くファイルを選択してください"
        ofdJSON.RestoreDirectory = True

        If ofdJSON.ShowDialog() = DialogResult.OK Then
            Using sr As New StreamReader(ofdJSON.FileName)
                rawstr = sr.ReadToEnd
                sr.Close()
            End Using
            Dim JsonObject As Object = JsonConvert.DeserializeObject(Of Object)(rawstr)

            Dim SkillNumber As Integer = 0
            Do
                Dim SkillData As New Dictionary(Of String, String)
                If SkillNumber = 0 Then

                    SkillData.Add("Name", "―")
                    SkillData.Add("Lv", "0")
                    SkillData.Add("Note", "―")
                    SkillData.Add("Range", "―")
                    SkillData.Add("Reqd", "―")
                    SkillData.Add("Roll", "―")
                    SkillData.Add("Target", "―")
                    SkillData.Add("Timing", "―")
                    SkillData.Add("Category", "―")
                    SkillData.Add("Type", "―")
                    SkillData.Add("Cost", "―")



                Else
                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Name")) Then
                        Exit Do
                    Else
                        SkillData.Add("Name", JsonObject("skill" & SkillNumber.ToString & "Name").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Lv")) Then
                        SkillData.Add("Lv", "0")
                    Else
                        SkillData.Add("Lv", JsonObject("skill" & SkillNumber.ToString & "Lv").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Note")) Then
                        SkillData.Add("Note", "")
                    Else
                        SkillData.Add("Note", JsonObject("skill" & SkillNumber.ToString & "Note").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Range")) Then
                        SkillData.Add("Range", "―")
                    Else
                        SkillData.Add("Range", JsonObject("skill" & SkillNumber.ToString & "Range").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Reqd")) Then
                        SkillData.Add("Reqd", "―")
                    Else
                        SkillData.Add("Reqd", JsonObject("skill" & SkillNumber.ToString & "Reqd").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Roll")) Then
                        SkillData.Add("Roll", "―")
                    Else
                        SkillData.Add("Roll", JsonObject("skill" & SkillNumber.ToString & "Roll").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Target")) Then
                        SkillData.Add("Target", "―")
                    Else
                        SkillData.Add("Target", JsonObject("skill" & SkillNumber.ToString & "Target").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Timing")) Then
                        SkillData.Add("Timing", "")
                    Else
                        SkillData.Add("Timing", JsonObject("skill" & SkillNumber.ToString & "Timing").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Category")) Then
                        SkillData.Add("Category", "―")
                    Else
                        SkillData.Add("Category", JsonObject("skill" & SkillNumber.ToString & "Category").ToString)
                    End If

                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Type")) Then
                        SkillData.Add("Type", "")
                    Else
                        SkillData.Add("Type", JsonObject("skill" & SkillNumber.ToString & "Type").ToString)
                    End If


                    If IsNothing(JsonObject("skill" & SkillNumber.ToString & "Cost")) Then
                        SkillData.Add("Cost", "―")
                    Else
                        SkillData.Add("Cost", JsonObject("skill" & SkillNumber.ToString & "Cost").ToString)
                    End If

                End If
                SkillList.Add(SkillNumber, SkillData)
                SkillNumber = SkillNumber + 1

            Loop

        End If

    End Sub

End Class
