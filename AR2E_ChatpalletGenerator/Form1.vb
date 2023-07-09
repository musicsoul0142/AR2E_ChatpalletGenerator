Imports System.Configuration
Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json

Public Class MainWindow
    'スキルチャパレを書く為の設定クラス
    Public Class WriteSettings
        '一個の設定のクラス
        Public Class SingleSetting
            Public Template As String
            Public IsHideOnEmpty As Boolean
            Private EmptyValue As String

            Public Sub New(Optional Template As String = "{0}", Optional IsHideOnEmpty As Boolean = True, Optional EmptyValue As String = "―")
                Me.Template = Template
                Me.IsHideOnEmpty = IsHideOnEmpty
                Me.EmptyValue = EmptyValue
            End Sub

            Public Function IsEmptyValue(Value As String) As Boolean
                Return Value = EmptyValue
            End Function
        End Class

        Public SkillTemplate As String = "◆[タイミング]◆《[名称]》[レベル]：[分類][効果][判定][対象][射程][コスト][制限]"
        Public Timing, Name, Level, Note As New SingleSetting
        Public Category As New SingleSetting("【{0}】", EmptyValue:="")
        Public Roll As New SingleSetting("/判定：{0}")
        Public Target As New SingleSetting("/対象：{0}")
        Public Range As New SingleSetting("/射程：{0}")
        Public Cost As New SingleSetting("/コスト：{0}")
        Public Reqd As New SingleSetting("/条件：{0}")

        Public Function GetSettingByType(Type As String) As SingleSetting
            Dim Result As SingleSetting

            Select Case Type
                Case "Category"
                    Result = Category
                Case "Name"
                    Result = Name
                Case "Lv"
                    Result = Level
                Case "Timing"
                    Result = Timing
                Case "Note"
                    Result = Note
                Case "Roll"
                    Result = Roll
                Case "Target"
                    Result = Target
                Case "Range"
                    Result = Range
                Case "Cost"
                    Result = Cost
                Case "Reqd"
                    Result = Reqd
                Case Else
                    Result = New SingleSetting
            End Select

            Return Result
        End Function
    End Class

    Private SettingFile As String = "writeSettings.json"
    Private WriteSetting As New WriteSettings

    Private SkillList As Object
    Private Passiveflag As Boolean = True

    'スキルチャパレを書く為の設定を読み込み
    Public Sub Settings_Load()
        If Not File.Exists(SettingFile) Then Return

        Dim rawstr As String
        Using sr As New StreamReader(SettingFile)
            rawstr = sr.ReadToEnd
            sr.Close()
        End Using
        WriteSetting = JsonConvert.DeserializeObject(Of WriteSettings)(rawstr)
    End Sub

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
        Dim Timing, Name, Level, Note, Roll, Range, Target, Reqd, Category, Cost As String

        Dim GetString = Function(Type As String)
                            Dim Value As String = SkillData(Type)
                            Dim Setting As WriteSettings.SingleSetting = WriteSetting.GetSettingByType(Type)
                            If Setting.IsHideOnEmpty AndAlso Setting.IsEmptyValue(Value) Then
                                Return ""
                            Else
                                Return String.Format(Setting.Template, Value)
                            End If
                        End Function

        Timing = (GetString("Timing").Replace("／メイキング", ""))
        Name = GetString("Name")
        Level = GetString("Lv")
        Category = GetString("Category")
        Note = GetString("Note")
        Roll = GetString("Roll")
        Target = GetString("Target")
        Range = GetString("Range")
        Cost = GetString("Cost")
        Reqd = GetString("Reqd")

        SkillChat = WriteSetting.SkillTemplate
        SkillChat = Replace(SkillChat, "[タイミング]", Timing)
        SkillChat = Replace(SkillChat, "[名称]", Name)
        SkillChat = Replace(SkillChat, "[レベル]", Level)
        SkillChat = Replace(SkillChat, "[分類]", Category)
        SkillChat = Replace(SkillChat, "[効果]", Note)
        SkillChat = Replace(SkillChat, "[判定]", Roll)
        SkillChat = Replace(SkillChat, "[対象]", Target)
        SkillChat = Replace(SkillChat, "[射程]", Range)
        SkillChat = Replace(SkillChat, "[コスト]", Cost)
        SkillChat = Replace(SkillChat, "[制限]", Reqd)

        If Not WriteSetting.GetSettingByType("Cost").IsEmptyValue(SkillData("Cost")) Then
            SkillChat &= $"{vbCrLf}:MP-{SkillData("Cost")} @{SkillData("Name")}"
        End If

        If SkillData("Lv") > "1" Then
            SkillChat &= $"{vbCrLf}//{SkillData("Name")}={SkillData("Lv")}"
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
            Dim JsonObject As Object

            Try
                JsonObject = JsonConvert.DeserializeObject(Of Object)(rawstr)
            Catch ex As Newtonsoft.Json.JsonReaderException
                MessageBox.Show("読み込めませんでした。" & vbCrLf & "ゆとシート2 For AR2Eから出力されたJSONファイルを指定して下さい。",
                                "読み込みエラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End Try

            If IsNothing(JsonObject("skill1Type")) Then
                MessageBox.Show("データの読み込みに失敗しました。" & vbCrLf & "ゆとシート2 For AR2Eから出力されたJSONファイルを指定して下さい。",
                                "読み込みエラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim loop_type As String = "Integer"
            Dim key_first As String = "skill"
            Dim key_last_list() As String = {"Name", "Lv", "Note", "Range", "Reqd", "Roll", "Target", "Timing", "Category", "Type", "Cost"}
            Dim default_value() As String = {"Invalid Data", "0", "", "―", "―", "―", "―", "", "", "", "―"}

            SkillList = Data_Load(loop_type, key_first, key_last_list, default_value, JsonObject)

            DataGridView1.Rows.Clear()
            Passiveflag = True

            Dim row_number As Integer

            For index = 0 To SkillList.Count - 1
                DataGridView1.Rows.Add()
                row_number = index + 1

                Dim Skill = SkillList(row_number)

                DataGridView1.Rows(index).Cells(0).Value = True
                DataGridView1.Rows(index).Cells(1).Value = row_number
                DataGridView1.Rows(index).Cells(2).Value = Skill("Type")
                DataGridView1.Rows(index).Cells(3).Value = Skill("Category")
                DataGridView1.Rows(index).Cells(4).Value = Skill("Name")
                DataGridView1.Rows(index).Cells(5).Value = Skill("Lv")
                DataGridView1.Rows(index).Cells(6).Value = Skill("Timing")
                DataGridView1.Rows(index).Cells(7).Value = Skill("Note")
                DataGridView1.Rows(index).Cells(8).Value = Skill("Roll")
                DataGridView1.Rows(index).Cells(9).Value = Skill("Target")
                DataGridView1.Rows(index).Cells(10).Value = Skill("Range")
                DataGridView1.Rows(index).Cells(11).Value = Skill("Cost")
                DataGridView1.Rows(index).Cells(12).Value = Skill("Reqd")
            Next
        End If

    End Sub

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 設定ファイルがあればロードする
        Settings_Load()

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

        For index = 1 To 12
            DataGridView1.Columns(index).ReadOnly = True
        Next

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
        Try
            If RichTextBox1.Text = "" Then
                Exit Sub
            End If
            Clipboard.Clear()
            System.Threading.Thread.Sleep(100)
            Clipboard.SetText(RichTextBox1.Text)
            '        Catch ex As System.ArgumentNullException
            '            Return
        Catch ex As Exception
            MsgBox("クリップボードへのコピーに失敗しました。再試行するか、対象テキストを選択して Ctrl+C を押してコピーしてください。", MsgBoxStyle.Exclamation, MainWindow.ActiveForm.Text)

        End Try
    End Sub
End Class