using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Elk.GHComponents
{
    /// <summary>
    /// Interaction logic for ComponentLocationForm.xaml
    /// </summary>
    public partial class ComponentLocationForm : Window
    {
        LinearGradientBrush brush = null;

        public ComponentLocationForm()
        {
            InitializeComponent();
            tabTextBox.Text = Properties.Settings.Default.tabName;
            panelTextBox.Text = Properties.Settings.Default.panelName;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            bool changed = false;
            if (tabTextBox.Text != Properties.Settings.Default.tabName)
            {
                Properties.Settings.Default.tabName = tabTextBox.Text;
                changed = true;
            }
            if (panelTextBox.Text != Properties.Settings.Default.panelName)
            {
                Properties.Settings.Default.panelName = panelTextBox.Text;
                changed = true;
            }
            if (changed)
                Properties.Settings.Default.Save();
            this.Close();
        }

        private void okButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
            {
                brush = EnterBrush();
            }
            okButtonRect.Fill = brush;
        }

        private void okButton_MouseLeave(object sender, MouseEventArgs e)
        {
            okButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void closeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void closeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
            {
                brush = EnterBrush();
            }
            closeButtonRect.Fill = brush;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private LinearGradientBrush EnterBrush()
        {
            LinearGradientBrush b = new LinearGradientBrush();
            b.StartPoint = new System.Windows.Point(0, 0);
            b.EndPoint = new System.Windows.Point(0, 1);
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 195, 195, 195), 0.0));
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 245, 245, 245), 1.0));

            return b;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
