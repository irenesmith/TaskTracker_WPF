using System.Windows;

namespace TaskTracker_WPF
{
    /// <summary>
    /// Interaction logic for NewTaskDialog.xaml
    /// </summary>
    public partial class NewTaskDialog : Window
    {
        public Task NewTask;

        /// <summary>
        /// Constructor for the Task dialog window.
        /// </summary>
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
