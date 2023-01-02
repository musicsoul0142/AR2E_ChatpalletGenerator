Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports Newtonsoft.Json

Public Class MainWindow

    Function Data_Load(loop_type, key_first, key_last_list, default_value, JSONObject)
        Dim List As New Dictionary(Of Object, Dictionary(Of String, String))

        If loop_type = "Integer" Then
            Dim index As Integer = 0
            Do
                index = index + 1
                Dim SubList As New Dictionary(Of String, String)
                'データの種類分ループここから
                Dim subloop As Integer = default_value.Length - 1
                For subindex = 0 To subloop
                    Dim subkey As String = key_first & index.ToString & key_last_list(subindex)
                    'subkey = skill0Name
                    If subindex = 0 And IsNothing(JSONObject(subkey)) Then
                        '連番データの最終行が存在しない時、終了
                        Exit Do
                    ElseIf IsNothing(JSONObject(subkey)) Then
                        SubList.Add(key_last_list(subindex), default_value(subindex))
                    Else
                        SubList.Add(key_last_list(subindex), JSONObject(subkey).ToString)
                    End If
                Next
                'データの種類分ループここまで
                List.Add(index, SubList)
            Loop
        Else

        End If
        Return List

    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SkillList As New Dictionary(Of Object, Dictionary(Of String, String))
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

            Dim loop_type As String = "Integer"
            Dim key_first As String = "skill"
            Dim key_last_list() As String = {"Name", "Lv", "Note", "Range", "Reqd", "Roll", "Target", "Timing", "Category", "Type", "Cost"}
            Dim default_value() As String = {"Invalid Data", "0", "", "―", "―", "―", "―", "", "", "", "―"}

            SkillList = Data_Load(loop_type, key_first, key_last_list, default_value, JsonObject)

            MsgBox(SkillList.Count)

        End If

    End Sub

End Class
