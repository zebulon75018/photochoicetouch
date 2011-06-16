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
using System.Diagnostics;

namespace DateSelectorUsageExamples
{
    /// <summary>
    /// Interaction logic for WindowMain.xaml
    /// </summary>.
    public partial class WindowMain : Window
    {
        public WindowMain()
        {
            InitializeComponent();
            dateSelector.MonthSelected(10);
        }

        /// <summary>
        /// Adds event handlers and sets property values.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbLocaleName.SelectionChanged += new SelectionChangedEventHandler(cbLocaleName_SelectionChanged);
            cbLocaleName.SelectedIndex = 0;

            cbThemeSelector.SelectionChanged += new SelectionChangedEventHandler(cbThemeSelector_SelectionChanged);
            cbThemeSelector.SelectedIndex = 0;

            // Fill MinimumYear combo box.
            FillComboBoxWithNumbers(comboBoxMinimumYear, 1950, 2010);
            comboBoxMinimumYear.SelectionChanged += new SelectionChangedEventHandler(comboBoxMinimumYear_SelectionChanged);
            comboBoxMinimumYear.SelectedIndex = 0;

            // Fill MinimumYear combo box.
            FillComboBoxWithNumbers(comboBoxMaximumYear, 1980, 2010);
            comboBoxMaximumYear.SelectionChanged += new SelectionChangedEventHandler(comboBoxMaximumYear_SelectionChanged);
            comboBoxMaximumYear.SelectedIndex = 0;
        }

        private void FillComboBoxWithNumbers(ComboBox targetComboBox, Int32 beginNumber, Int32 endNumber)
        {
            Trace.Assert(null != targetComboBox, "null != targetComboBox");
            targetComboBox.Items.Clear();
            for (Int32 i = beginNumber; i <= endNumber; i++)
                targetComboBox.Items.Add(i);
        }

        /// <summary>
        /// DateSelector.SelectedDateChanged event handler.
        /// </summary>
        private void dateSelector_SelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (null != txtSelectedDate)
                txtSelectedDate.Text = e.NewValue.ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Selects one of the predefined templates.
        /// </summary>
        private void cbThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbThemeSelector.SelectedIndex)
            {
                case 0:
                    dateSelector.ControlStyle = Vodnev.Controls.DateSelector.DateSelectorStyle.Default;
                    break;
                case 1:
                    dateSelector.ControlStyle = Vodnev.Controls.DateSelector.DateSelectorStyle.Touchscreen;
                    break;
                case 2:
                    dateSelector.ControlStyle = Vodnev.Controls.DateSelector.DateSelectorStyle.TouchscreenYearOnly;
                    break;
                default:
                    throw new NotSupportedException("Unknown Theme Selection");
            }
        }

        /// <summary>
        /// Sets LocaleName property.
        /// </summary>
        private void cbLocaleName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cbLocaleName.SelectedItem;
            if (null != selectedItem)
                dateSelector.LocaleName = selectedItem.Content.ToString();
        }

        /// <summary>
        /// Sets MinimumYear property.
        /// </summary>
        private void comboBoxMinimumYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateSelector.MinimumYear = Convert.ToInt32(comboBoxMinimumYear.SelectedItem);
        }

        /// <summary>
        /// Sets EndYear property.
        /// </summary>
        private void comboBoxMaximumYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateSelector.MaximumYear = Convert.ToInt32(comboBoxMaximumYear.SelectedItem);
        }

        /// <summary>
        /// Sets custom template.
        /// </summary>
        private void checkBoxApplyCustomTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxApplyCustomTemplate.IsChecked == true)
            {
                // Apply custom template.
                foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
                {
                    foreach (System.Collections.DictionaryEntry entry in dictionary)
                    {
                        if ((String)entry.Key == "DateSelectorCustomTemplate")
                        {
                            dateSelector.Template = (ControlTemplate)entry.Value;
                        }
                    }
                }
            }
            else
            {
                // Clear template.
                dateSelector.Template = null;
            }
        }
    }
}
