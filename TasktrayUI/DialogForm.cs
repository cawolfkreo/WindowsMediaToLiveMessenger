using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioSyncMSNUI
{
    class DialogForm
    {
        public static string ShowDialog(string text, string caption, string example)
        {
            Form form = new()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label txtLabel = new()
            {
                Left = 50,
                Top = 10,
                Text = text,
                MaximumSize = new System.Drawing.Size(400, 0),
                AutoSize = true
            };

            TextBox txtBox = new()
            {
                Left = 50,
                Top = 90,
                Width = 400,
                SelectedText = example
            };

            Button confirmBtn = new()
            {
                Text = "Ok",
                Left = 350,
                Width = 100,
                Top = 170,
                Height = 30,
                DialogResult = DialogResult.OK
            };

            confirmBtn.Click += (sender, e) => { form.Close(); };

            form.Controls.Add(txtLabel);
            form.Controls.Add(txtBox);
            form.Controls.Add(confirmBtn);
            
            form.AcceptButton = confirmBtn;

            return form.ShowDialog() == DialogResult.OK ? txtBox.Text : "";
        }
    }
}
