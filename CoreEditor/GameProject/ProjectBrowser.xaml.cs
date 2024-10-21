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

namespace CoreEditor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowser.xaml
    /// </summary>
    public partial class ProjectBrowser : Window
    {
        public ProjectBrowser()
        {
            InitializeComponent();
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == openProjectBtn) 
            {
                if(createProjectBtn.IsChecked == true)
                {
                    createProjectBtn.IsChecked = false;
                    BrowserContent.Margin = new Thickness(0);
                }
                openProjectBtn.IsChecked = true;
            }
            else if(sender == createProjectBtn)
            {
                if (openProjectBtn.IsChecked == true)
                {
                    openProjectBtn.IsChecked = false;
                    BrowserContent.Margin = new Thickness(-800,0,0,0);
                }
                createProjectBtn.IsChecked = true;
            }
        }
    }
}
