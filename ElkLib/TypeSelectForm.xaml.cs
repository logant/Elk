using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Elk.Common
{
    /// <summary>
    /// Interaction logic for TypeSelectForm.xaml
    /// </summary>
    public partial class TypeSelectForm : Window
    {
        LinearGradientBrush brush = null;
        bool individualOutputs = false;

        ObservableCollection<string> availableTypes = new ObservableCollection<string>();
        ObservableCollection<string> selectedTypes = new ObservableCollection<string>();

        public ObservableCollection<string> SelectedTypes
        {
            get { return selectedTypes; }
            set { selectedTypes = value; }
        }

        public ObservableCollection<string> AvailableTypes
        {
            get { return availableTypes; }
            set { availableTypes = value; }
        }

        public bool IndividualOutputs
        {
            get { return individualOutputs; }
            set { individualOutputs = value; }
        }

        public TypeSelectForm(ElkLib.FeatureType featureType, List<string> selected, bool individual, bool allowIndividual)
        {
            InitializeComponent();
            
            #region Get Available Types
            List<string> availableTemp = new List<string>();
            switch (featureType)
            {
                case ElkLib.FeatureType.Aerialway:
                    availableTemp = Elk.Common.ElkLib.Aerialways;
                    break;
                case ElkLib.FeatureType.Aeroway:
                    availableTemp = Elk.Common.ElkLib.Aeroways;
                    break;
                case ElkLib.FeatureType.Amenity:
                    availableTemp = Elk.Common.ElkLib.Amenities;
                    break;
                case ElkLib.FeatureType.Barrier:
                    availableTemp = Elk.Common.ElkLib.Barrier;
                    break;
                case ElkLib.FeatureType.Boundary:
                    availableTemp = Elk.Common.ElkLib.Boundary;
                    break;
                case ElkLib.FeatureType.Building:
                    availableTemp = Elk.Common.ElkLib.Building;
                    break;
                case ElkLib.FeatureType.Craft:
                    availableTemp = Elk.Common.ElkLib.Craft;
                    break;
                case ElkLib.FeatureType.Emergency:
                    availableTemp = Elk.Common.ElkLib.Emergency;
                    break;
                case ElkLib.FeatureType.Geological:
                    availableTemp = Elk.Common.ElkLib.Geological;
                    break;
                case ElkLib.FeatureType.Highway:
                    availableTemp = Elk.Common.ElkLib.Highway;
                    break;
                case ElkLib.FeatureType.Historic:
                    availableTemp = Elk.Common.ElkLib.Historic;
                    break;
                case ElkLib.FeatureType.Landuse:
                    availableTemp = Elk.Common.ElkLib.Landuse;
                    break;
                case ElkLib.FeatureType.Leisure:
                    availableTemp = Elk.Common.ElkLib.Leisure;
                    break;
                case ElkLib.FeatureType.ManMade:
                    availableTemp = Elk.Common.ElkLib.ManMade;
                    break;
                case ElkLib.FeatureType.Military:
                    availableTemp = Elk.Common.ElkLib.Military;
                    break;
                case ElkLib.FeatureType.Natural:
                    availableTemp = Elk.Common.ElkLib.Natural;
                    break;
                case ElkLib.FeatureType.Office:
                    availableTemp = Elk.Common.ElkLib.Office;
                    break;
                case ElkLib.FeatureType.Place:
                    availableTemp = Elk.Common.ElkLib.Places;
                    break;
                case ElkLib.FeatureType.Power:
                    availableTemp = Elk.Common.ElkLib.Power;
                    break;
                case ElkLib.FeatureType.PublicTransport:
                    availableTemp = Elk.Common.ElkLib.PublicTransport;
                    break;
                case ElkLib.FeatureType.Railway:
                    availableTemp = Elk.Common.ElkLib.Railway;
                    break;
                case ElkLib.FeatureType.Route:
                    availableTemp = Elk.Common.ElkLib.Route;
                    break;
                case ElkLib.FeatureType.Shop:
                    availableTemp = Elk.Common.ElkLib.Shop;
                    break;
                case ElkLib.FeatureType.Sport:
                    availableTemp = Elk.Common.ElkLib.Sport;
                    break;
                case ElkLib.FeatureType.Tourism:
                    availableTemp = Elk.Common.ElkLib.Toursim;
                    break;
                case ElkLib.FeatureType.Waterway:
                    availableTemp = Elk.Common.ElkLib.Waterway;
                    break;
            };

            availableTemp.Sort();
            foreach (string s in availableTemp)
            {
                if(!selected.Contains(s))
                    availableTypes.Add(s);
            }
            #endregion

            // hide the Individual Outputs if it's not selectable
            if (!allowIndividual)
                outputsCheckbox.Visibility = System.Windows.Visibility.Hidden;

            // remove the already selected types from the available ways.
            foreach (string s in selected)
            {
                selectedTypes.Add(s);
            }

            // Set the Individual Outputs Checkbox
            if (individual)
            {
                individualOutputs = true;
                outputsCheckbox.IsChecked = true;
            }

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cancelButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
                brush = EnterBrush();

            cancelButtonRect.Fill = brush;
        }

        private void cancelButton_MouseLeave(object sender, MouseEventArgs e)
        {
            cancelButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the checkbox value
            if (selectedTypes.Count <= 10 && outputsCheckbox.IsChecked == true)
            {
                individualOutputs = true;
            }
            else
            {
                individualOutputs = false;
            }
            DialogResult = true;

            Close();
        }

        private void okButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (brush == null)
                brush = EnterBrush();

            okButtonRect.Fill = brush;
        }

        private void okButton_MouseLeave(object sender, MouseEventArgs e)
        {
            okButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Get SelectedIndex from listbox
            try
            {
                int selectedIndex = availableListBox.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    selectedTypes.Add(availableListBox.SelectedItem as string);
                    availableTypes.RemoveAt(selectedIndex);

                    // Enable/Disable the Checkbox
                    if (selectedTypes.Count <= 10)
                        outputsCheckbox.IsEnabled = true;
                    else
                        outputsCheckbox.IsEnabled = false;

                    List<string> tempSelected = selectedTypes.ToList();
                    tempSelected.Sort();

                    selectedTypes.Clear();
                    foreach (string s in tempSelected)
                    {
                        selectedTypes.Add(s);
                    }
                }
            }
            catch { }
        }

        private void addButton_MouseEnter(object sender, MouseEventArgs e)
        {
            addButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 195, 195, 195));
        }

        private void addButton_MouseLeave(object sender, MouseEventArgs e)
        {
            addButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            // Get SelectedIndex from listbox
            try
            {
                int selectedIndex = selectedListBox.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    availableTypes.Add(selectedListBox.SelectedItem as string);
                    selectedTypes.RemoveAt(selectedIndex);

                    // Enable/Disable the Checkbox
                    if (selectedTypes.Count <= 10)
                        outputsCheckbox.IsEnabled = true;
                    else
                        outputsCheckbox.IsEnabled = false;

                    
                    // Sort the available and selected types
                    List<string> tempAvailable = availableTypes.ToList();
                    tempAvailable.Sort();

                    availableTypes.Clear();
                    foreach (string s in tempAvailable)
                    {
                        availableTypes.Add(s);
                    }

                }
            }
            catch { }
        }

        private void removeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            removeButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 195, 195, 195));
        }

        private void removeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            removeButtonRect.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
        }

        private LinearGradientBrush EnterBrush()
        {
            LinearGradientBrush b = new LinearGradientBrush();
            b.StartPoint = new System.Windows.Point(0, 0);
            b.EndPoint = new System.Windows.Point(0, 1);
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 245, 245, 245), 0.0));
            b.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 195, 195, 195), 1.0));

            return b;
        }
    }
}
