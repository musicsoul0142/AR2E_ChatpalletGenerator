Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports Newtonsoft.Json

Public Class MainWindow
    Private SkillList As Object
    Private Passiveflag As Boolean = True
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

        If SkillData("Lv") <> "1" Then
            SkillChat = $"◆{SkillData("Timing")}◆《{SkillData("Name")}》{{{SkillData("Name")}}}：{Category}{SkillData("Note")}{Roll}{Target}{Range}{Cost}{Reqd}"
        Else
            SkillChat = $"◆{SkillData("Timing")}◆《{SkillData("Name")}》{SkillData("Lv")}：{Category}{SkillData("Note")}{Roll}{Target}{Range}{Cost}{Reqd}"
        End If
        If SkillData("Cost") <> "―" Then
            SkillChat = SkillChat & $"{vbCrLf}:MP-{SkillData("Cost")} @{SkillData("Name")}"
        End If
        If SkillData("Lv") <> "1" Then
            SkillChat = SkillChat & $"{vbCrLf}//{SkillData("Name")}={SkillData("Lv")}"

        End If
        Return SkillChat
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
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

            For index = 1 To SkillList.Count
                If index < SkillList.Count Then
                    DataGridView1.Rows.Add()
                End If

                DataGridView1.Rows(index - 1).Cells(0).Value = True
                DataGridView1.Rows(index - 1).Cells(1).Value = index
                DataGridView1.Rows(index - 1).Cells(2).Value = SkillList(index)("Type")
                DataGridView1.Rows(index - 1).Cells(3).Value = SkillList(index)("Category")
                DataGridView1.Rows(index - 1).Cells(4).Value = SkillList(index)("Name")
                DataGridView1.Rows(index - 1).Cells(5).Value = SkillList(index)("Lv")
                DataGridView1.Rows(index - 1).Cells(6).Value = SkillList(index)("Timing")
                DataGridView1.Rows(index - 1).Cells(7).Value = SkillList(index)("Note")
                DataGridView1.Rows(index - 1).Cells(8).Value = SkillList(index)("Roll")
                DataGridView1.Rows(index - 1).Cells(9).Value = SkillList(index)("Target")
                DataGridView1.Rows(index - 1).Cells(10).Value = SkillList(index)("Range")
                DataGridView1.Rows(index - 1).Cells(11).Value = SkillList(index)("Cost")
                DataGridView1.Rows(index - 1).Cells(12).Value = SkillList(index)("Reqd")

            Next



        End If

    End Sub

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim column As New DataGridViewCheckBoxColumn
        DataGridView1.Columns.Add(column)

        DataGridView1.ColumnCount = 13
        DataGridView1.RowCount = 1

        DataGridView1.Columns(0).HeaderText = ""
        DataGridView1.Columns(1).HeaderText = "No."
        DataGridView1.Columns(2).HeaderText = "取得元"
        DataGridView1.Columns(3).HeaderText = "分類"
        DataGridView1.Columns(4).HeaderText = "名称"
        DataGridView1.Columns(5).HeaderText = "Lv"
        DataGridView1.Columns(6).HeaderText = "タイミング"
        DataGridView1.Columns(7).HeaderText = "効果"
        DataGridView1.Columns(8).HeaderText = "判定"
        DataGridView1.Columns(9).HeaderText = "対象"
        DataGridView1.Columns(10).HeaderText = "射程"
        DataGridView1.Columns(11).HeaderText = "コスト"
        DataGridView1.Columns(12).HeaderText = "使用条件"

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'チャパレ生成
        RichTextBox1.Text = ""

        For index = 1 To DataGridView1.RowCount
            If DataGridView1.Rows(index - 1).Cells(0).Value = False Then
                Continue For
            End If
            Dim test_text As String = Generate_ChatPallet(SkillList(index))
            RichTextBox1.AppendText($"{test_text}{vbCrLf}")
        Next
        'チャパレ生成ここまで
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'パッシブチェックオンオフボタン
        If Passiveflag = True Then
            For index = 0 To DataGridView1.RowCount - 1
                If DataGridView1.Rows(index).Cells(6).Value = "パッシブ" Or DataGridView1.Rows(index).Cells(6).Value = "パッシブ／メイキング" Then
                    DataGridView1.Rows(index).Cells(0).Value = False
                End If
            Next
            Passiveflag = False
        Else
            For index = 0 To DataGridView1.RowCount - 1
                If DataGridView1.Rows(index).Cells(6).Value = "パッシブ" Or DataGridView1.Rows(index).Cells(6).Value = "パッシブ／メイキング" Then
                    DataGridView1.Rows(index).Cells(0).Value = True
                End If
            Next
            Passiveflag = True
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Clipboard.SetText(RichTextBox1.Text)
    End Sub
End Class