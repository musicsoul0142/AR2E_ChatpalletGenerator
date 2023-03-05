<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainWindow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Button1 = New Button()
        DataGridView1 = New DataGridView()
        RichTextBox1 = New RichTextBox()
        CheckBox1 = New CheckBox()
        Button2 = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(75, 73)
        Button1.Name = "Button1"
        Button1.Size = New Size(76, 48)
        Button1.TabIndex = 0
        Button1.Text = "開く"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridView1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Location = New Point(232, 12)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowTemplate.Height = 25
        DataGridView1.Size = New Size(567, 426)
        DataGridView1.TabIndex = 2
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Location = New Point(3, 280)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(223, 158)
        RichTextBox1.TabIndex = 3
        RichTextBox1.Text = ""' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(12, 255)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(124, 19)
        CheckBox1.TabIndex = 4
        CheckBox1.Text = "パッシブを出力しない"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(150, 226)
        Button2.Name = "Button2"
        Button2.Size = New Size(76, 48)
        Button2.TabIndex = 5
        Button2.Text = "チャパレ出力"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' MainWindow
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(Button2)
        Controls.Add(CheckBox1)
        Controls.Add(RichTextBox1)
        Controls.Add(DataGridView1)
        Controls.Add(Button1)
        Name = "MainWindow"
        Text = "AR2Eチャットパレットジェネレーター"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button2 As Button
End Class
