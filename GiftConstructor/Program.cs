using GiftQuestionCreator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GiftQuestionCreator
{
    public partial class MainForm : Form
    {
        private List<Question> questions = new List<Question>();

        private ComboBox cmbQuestionType;
        private TextBox txtQuestionText;
        private TextBox txtOptions;
        private TextBox txtCorrectAnswer;
        private DataGridView dgvQuestions;

        public MainForm()
        {
            // Инициализация формы
            this.Text = "Генератор GIFT для Moodle";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создание элементов интерфейса
            cmbQuestionType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Top
            };
            cmbQuestionType.Items.AddRange(new string[] { "Множественный выбор", "Верно/Неверно", "Короткий ответ", "Соответствие", "Эссе" });

            var lblQuestionText = new Label { Text = "Текст вопроса:", Dock = DockStyle.Top, AutoSize = true };
            txtQuestionText = new TextBox { Dock = DockStyle.Top, Multiline = true, Height = 60 };

            var lblOptions = new Label { Text = "Варианты ответа (по одному на строке):", Dock = DockStyle.Top, AutoSize = true };
            txtOptions = new TextBox { Dock = DockStyle.Top, Multiline = true, Height = 100 };

            var lblCorrectAnswer = new Label { Text = "Правильный ответ:", Dock = DockStyle.Top, AutoSize = true };
            txtCorrectAnswer = new TextBox { Dock = DockStyle.Top };

            var btnAddQuestion = new Button { Text = "Добавить вопрос", Dock = DockStyle.Bottom };
            var btnGenerateGift = new Button { Text = "Сгенерировать GIFT", Dock = DockStyle.Bottom };
            var btnRemoveQuestion = new Button { Text = "Удалить вопрос", Dock = DockStyle.Bottom };

            btnAddQuestion.Click += btnAddQuestion_Click;
            btnGenerateGift.Click += btnGenerateGift_Click;
            btnRemoveQuestion.Click += btnRemoveQuestion_Click;

            dgvQuestions = new DataGridView
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            dgvQuestions.Columns[0].Name = "Текст вопроса";
            dgvQuestions.Columns[1].Name = "Тип вопроса";

            // Добавление элементов на форму
            Controls.Add(dgvQuestions);
            Controls.Add(btnRemoveQuestion);
            Controls.Add(btnGenerateGift);
            Controls.Add(btnAddQuestion);
            Controls.Add(txtCorrectAnswer);
            Controls.Add(lblCorrectAnswer);
            Controls.Add(txtOptions);
            Controls.Add(lblOptions);
            Controls.Add(txtQuestionText);
            Controls.Add(lblQuestionText);
            Controls.Add(cmbQuestionType);
        }

        private void btnAddQuestion_Click(object sender, EventArgs e)
        {
            var questionType = cmbQuestionType.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(questionType))
            {
                MessageBox.Show("Пожалуйста, выберите тип вопроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var question = new Question
            {
                Type = questionType,
                Text = txtQuestionText.Text,
                Options = new List<string>(txtOptions.Lines),
                CorrectAnswer = txtCorrectAnswer.Text
            };

            questions.Add(question);
            dgvQuestions.Rows.Add(question.Text, question.Type);

            ClearInputFields();
        }

        private void btnGenerateGift_Click(object sender, EventArgs e)
        {
            var giftContent = GenerateGiftContent();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "GIFT Файлы (*.gift)|*.gift",
                Title = "Сохранить файл GIFT"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, giftContent);
                MessageBox.Show("Файл GIFT успешно сохранён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GenerateGiftContent()
        {
            var giftContent = "";
            foreach (var question in questions)
            {
                giftContent += question.ToGiftFormat() + "\n\n";
            }

            return giftContent;
        }

        private void ClearInputFields()
        {
            txtQuestionText.Clear();
            txtOptions.Clear();
            txtCorrectAnswer.Clear();
            cmbQuestionType.SelectedIndex = -1;
        }

        private void btnRemoveQuestion_Click(object sender, EventArgs e)
        {
            if (dgvQuestions.SelectedRows.Count > 0)
            {
                int rowIndex = dgvQuestions.SelectedRows[0].Index;
                questions.RemoveAt(rowIndex);
                dgvQuestions.Rows.RemoveAt(rowIndex);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите вопрос для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "MainForm";
            this.Text = "Создать GIFT для Moodle";
            this.ResumeLayout(false);

        }
    }

}

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
