using System.Text.RegularExpressions;
using Feedback_Flow.Models;

namespace Feedback_Flow;

public partial class StudentForm : Form
{
    public string StudentFullName => txtFullName.Text.Trim();
    public string StudentEmail => txtEmail.Text.Trim();
    
    public string ClassDays 
    {
        get 
        {
            var days = new List<string>();
            foreach (var item in checkedListBoxClassDays.CheckedItems)
            {
                days.Add(item.ToString()!);
            }
            return string.Join(",", days);
        }
    }

    public StudentForm()
    {
        InitializeComponent();
    }

    public StudentForm(Student student) : this()
    {
        if (student != null)
        {
            txtFullName.Text = student.FullName;
            txtEmail.Text = student.Email;
            
            var studentDays = student.GetClassDays();
            for (int i = 0; i < checkedListBoxClassDays.Items.Count; i++)
            {
                if (studentDays.Contains(checkedListBoxClassDays.Items[i].ToString(), StringComparer.OrdinalIgnoreCase))
                {
                    checkedListBoxClassDays.SetItemChecked(i, true);
                }
            }
        }
    }

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        PerformLayout();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateInput())
        {
            DialogResult = DialogResult.None;
        }
        else
        {
            DialogResult = DialogResult.OK;
        }
    }

    private bool ValidateInput()
    {
        bool isValid = true;
        errorProvider.Clear();

        string name = txtFullName.Text.Trim();
        string email = txtEmail.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            errorProvider.SetError(txtFullName, "Name cannot be empty.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            errorProvider.SetError(txtEmail, "Invalid email format.");
            isValid = false;
        }

        return isValid;
    }
}
