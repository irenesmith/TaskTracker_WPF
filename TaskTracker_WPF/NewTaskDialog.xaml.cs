using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskTracker_WPF
{
    /// <summary>
    /// Interaction logic for NewTaskDialog.xaml
    /// </summary>
    public partial class NewTaskDialog : Window
    {
        public Task NewTask;

        public NewTaskDialog()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
            PickerCompleteBy.SelectedDate = DateTime.Now;
            NewTask = new Task();
            textBoxTask.Focus();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (PickerCompleteBy.SelectedDate is not null)
            {
                NewTask.CompleteBy = (DateTime)PickerCompleteBy.SelectedDate;
            }

            NewTask.Description = textBoxTask.Text;
            NewTask.IsDone = CheckDone.IsChecked == true;

            DialogResult = true;
        }
    }
}
