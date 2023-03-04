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

                        If JSONObject(subkey).ToString = "race" Then
                            SubList.Add(key_last_list(subindex), "種族")
                        ElseIf JSONObject(subkey).ToString = "add" Then
                            SubList.Add(key_last_list(subindex), "他スキル")
                        ElseIf JSONObject(subkey).ToString = "general" Then
                            SubList.Add(key_last_list(subindex), "一般")
                        ElseIf JSONObject(subkey).ToString = "power" Then
                            SubList.Add(key_last_list(subindex), "パワー（共通）")
                        ElseIf JSONObject(subkey).ToString = "another" Then
                            SubList.Add(key_last_list(subindex), "異才")
                        ElseIf JSONObject(subkey).ToString = "style" Then
                            SubList.Add(key_last_list(subindex), "流派")
                        ElseIf JSONObject(subkey).ToString = "geis" Then
                            SubList.Add(key_last_list(subindex), "誓約")
                        Else
                            SubList.Add(key_last_list(subindex), JSONObject(subkey).ToString)
                        End If

                    End If
                Next
                'データの種類分ループここまで
                List.Add(index, SubList)
            Loop
        Else

        End If
        Return List

    End Function

    Function Generate_ChatPallet(SkillData)
        Dim SkillChat As String
        Dim Roll, Range, Target, Reqd, Category, Cost As String
        If SkillData("Roll") = "―" Then
            Roll = ""
        Else
            Roll = $"/判定：{SkillData("Roll")}"
        End If

        If SkillData("Range") = "―" Then
            Range = ""
        Else
            Range = $"/射程：{SkillData("Range")}"
        End If

        If SkillData("Target") = "―" Then
            Target = ""
        Else
            Target = $"/対象：{SkillData("Target")}"
        End If

        If SkillData("Reqd") = "―" Then
            Reqd = ""
        Else
            Reqd = $"/条件：{SkillData("Reqd")}"
        End If

        If SkillData("Category") = "" Then
            Category = ""
        Else
            Category = $"【{SkillData("Category")}】"
        End If

        If SkillData("Cost") = "―" Then
            Cost = ""
        Else
            Cost = $"/コスト：{SkillData("Cost")}"
        End If

        SkillChat = $"◆{SkillData("Timing")}◆《{SkillData("Name")}》{SkillData("Lv")}：{Category}/{SkillData("Note")}{Roll}{Target}{Range}{Cost}{Reqd}"
        If SkillData("Cost") <> "―" Then
            SkillChat = SkillChat & $"{vbCrLf}:MP-{SkillData("Cost")}@{SkillData("Name")}"
        End If
        Return SkillChat
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim SkillList As Object
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

            RichTextBox1.Text = ""

            For index = 1 To SkillList.Count
                If (SkillList(index)("Timing") = "パッシブ" Or SkillList(index)("Timing") = "パッシブ／メイキング") And CheckBox1.Checked = True Then
                    Continue For
                End If
                Dim test_text As String = Generate_ChatPallet(SkillList(index))
                RichTextBox1.AppendText($"{test_text}{vbCrLf}")
            Next

            For index = 1 To SkillList.Count
                Dim test_text As String = Generate_ChatPallet(SkillList(index))
                RichTextBox1.AppendText($"{test_text}{vbCrLf}")
            Next



        End If

    End Sub

End Class
