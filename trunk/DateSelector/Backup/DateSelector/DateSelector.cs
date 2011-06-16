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
using System.Globalization;
using System.Xml;
using System.Windows.Resources;
using System.Windows.Controls.Primitives;

namespace Vodnev.Controls
{
    /// <summary>
    /// DateSelector control class.
    /// </summary>
    [TemplatePart(Name="PART_Day", Type=typeof(Selector))]
    [TemplatePart(Name="PART_Month", Type=typeof(Selector))]
    [TemplatePart(Name="PART_Year", Type=typeof(Selector))]
    public class DateSelector : Control
    {
        public DateSelector()
        {
            DefaultStyleKey = typeof(DateSelector);
            if (ControlStyle == DateSelectorStyle.Default)
                OnControlStyleChanged("generic.xaml", true);
        }

        /// <summary>
        /// Статический конструктор. Регистрирует свойства зависимостей.
        /// </summary>
        static DateSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateSelector), new FrameworkPropertyMetadata(typeof(DateSelector)));

            // Инициализировать свойства зависимостей.
            SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(DateSelector), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedDateChanged)));
            MinimumYearProperty = DependencyProperty.Register("MinimumYear", typeof(Int32), typeof(DateSelector), new FrameworkPropertyMetadata(1950, new PropertyChangedCallback(OnMinimumYearChanged)));
            LocaleNameProperty = DependencyProperty.Register("LocaleName", typeof(String), typeof(DateSelector), new FrameworkPropertyMetadata(String.Empty, new PropertyChangedCallback(OnLocaleNameChanged)));
            MaximumYearProperty = DependencyProperty.Register("MaximumYear", typeof(Int32), typeof(DateSelector), new FrameworkPropertyMetadata(2009, new PropertyChangedCallback(OnMaximumYearChanged)));
            ControlStyleProperty = DependencyProperty.Register("ControlStyle", typeof(DateSelectorStyle), typeof(DateSelector), new FrameworkPropertyMetadata(DateSelectorStyle.Default, new PropertyChangedCallback(DateSelectorStyleChanged)));

            SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(DateSelector));
        }

        private Selector partYear;
        private Selector partMonth;
        private Selector partDay;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            partDay = GetTemplateChild("PART_Day") as Selector;
            partDay.SelectionChanged += new SelectionChangedEventHandler(partDay_SelectionChanged);
            partMonth = GetTemplateChild("PART_Month") as Selector;
            partMonth.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
            partYear = GetTemplateChild("PART_Year") as Selector;
            partYear.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);

            FillYears();
            FillMonth();
        }

        /// <summary>
        /// Выбранная дата.
        /// </summary>
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty;

        /// <summary>
        /// Обрабатывает изменение выбранной даты.
        /// </summary>
        private static void OnSelectedDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTime newDate = (DateTime)e.NewValue;
            DateSelector dateSelector = (DateSelector)sender;
            // Установить значения элементов.
            dateSelector.partYear.SelectedItem = newDate.Year;
            dateSelector.partMonth.SelectedIndex = newDate.Month - 1;
            dateSelector.partDay.SelectedIndex = newDate.Day - 1;
            // Уведомить подписчиков об изменении даты.
            RoutedPropertyChangedEventArgs<DateTime> args = new RoutedPropertyChangedEventArgs<DateTime>((DateTime)e.OldValue, newDate);
            args.RoutedEvent = DateSelector.SelectedDateChangedEvent;
            dateSelector.RaiseEvent(args);
        }

        public static readonly RoutedEvent SelectedDateChangedEvent;

        public event RoutedPropertyChangedEventHandler<DateTime> SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }

        /// <summary>
        /// Год начала допустимого интервала.
        /// </summary>
        public Int32 MinimumYear
        {
            get { return (Int32)GetValue(MinimumYearProperty); }
            set { SetValue(MinimumYearProperty, value); }
        }

        public static readonly DependencyProperty MinimumYearProperty;

        /// <summary>
        /// Обрабатывает изменения начального года.
        /// </summary>
        private static void OnMinimumYearChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateSelector dateSelector = (DateSelector)sender;
            if ((dateSelector.MaximumYear < (Int32)e.NewValue) && (dateSelector.MaximumYear != 0))
                dateSelector.MaximumYear = (Int32)e.NewValue;
                //dateSelector.MinimumYear = dateSelector.MaximumYear;
            //else
                dateSelector.MinimumYear = (Int32)e.NewValue;
            if (null != dateSelector.partYear)
                dateSelector.FillYears();

        }

        public Int32 MaximumYear
        {
            get { return (Int32)GetValue(MaximumYearProperty); }
            set { SetValue(MaximumYearProperty, value); }
        }

        public static readonly DependencyProperty MaximumYearProperty;

        /// <summary>
        /// Обрабатывает изменение конечного года.
        /// </summary>
        private static void OnMaximumYearChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateSelector dateSelector = (DateSelector)sender;
            if ((dateSelector.MinimumYear > (Int32)e.NewValue) && (dateSelector.MinimumYear != 0))
                //dateSelector.MaximumYear = dateSelector.MinimumYear;
                dateSelector.MinimumYear = (Int32)e.NewValue;
            //else
                dateSelector.MaximumYear = (Int32)e.NewValue;
            if (null != dateSelector.partYear)
                dateSelector.FillYears();
        }

        //private Boolean showYearOnly = false;

        ///// <summary>
        ///// Отображать только год.
        ///// </summary>
        //private Boolean ShowYearOnly
        //{
        //    get
        //    {
        //        return showYearOnly;
        //    }
        //    set
        //    {
        //        showYearOnly = value;
        //        if (showYearOnly)
        //        {
        //            partDay.Visibility = Visibility.Collapsed;
        //            partMonth.Visibility = Visibility.Collapsed;
        //            // Установить дату равной 31 декабря.
        //            SelectedDate = new DateTime(SelectedDate.Year, 12, 31);
        //        }
        //    }
        //}


        public String LocaleName
        {
            get { return (String)GetValue(LocaleNameProperty); }
            set { SetValue(LocaleNameProperty, value); }
        }

        public static readonly DependencyProperty LocaleNameProperty;

        private static void OnLocaleNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateSelector dateSelector = (DateSelector)sender;
            dateSelector.FillMonth();
        }

        private CultureInfo currentCultureInfo;
        private DateTimeFormatInfo currentFormatInfo;

        /// <summary>
        /// Заполняет перечень месяцев.
        /// </summary>
        private void FillMonth()
        {
            InitializeDateTimeFormatInfo();

            partMonth.Items.Clear();

            String[] names = currentFormatInfo.MonthGenitiveNames;
            foreach (String monthName in names)
            {
                if (String.IsNullOrEmpty(monthName))
                    continue;
                partMonth.Items.Add(monthName);
            }
            partMonth.SelectedIndex = 0;
        }

        /// <summary>
        /// Инициализирует настройки локализации.
        /// </summary>
        private void InitializeDateTimeFormatInfo()
        {
            if (String.IsNullOrEmpty(LocaleName))
            {
                currentCultureInfo = CultureInfo.CurrentCulture;
            }
            else
            {
                currentCultureInfo = new CultureInfo(LocaleName);
            }
            currentFormatInfo = currentCultureInfo.DateTimeFormat;
        }

        /// <summary>
        /// Заполняет интервал (перечень) лет.
        /// </summary>
        private void FillYears()
        {
            if (MinimumYear == 0)
                return;
            partYear.Items.Clear();
            for (Int32 year = MinimumYear; year <= MaximumYear; year++)
            {
                partYear.Items.Add(year);
            }
            partYear.SelectedIndex = partYear.Items.Count / 2;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверить количество дней в выбранном месяце / году.
            Int32 selectedYear = Convert.ToInt32(partYear.SelectedItem);
            if (selectedYear == 0)
                return;
            Int32 selectedMonth = Convert.ToInt32(partMonth.SelectedIndex) + 1;
            if (selectedMonth == 0)
                return;
            Int32 daysCount = DateTime.DaysInMonth(selectedYear, selectedMonth);

            UpdateDaysCount(daysCount);
            UpdateSelectedDate();
        }

        /// <summary>
        /// При необходимости обновляет количество дней в списке.
        /// </summary>
        /// <param name="daysCount">Необходимое количество дней в выбранном месяце / году.</param>
        private void UpdateDaysCount(Int32 daysCount)
        {
            if (daysCount == partDay.Items.Count)
                return;
            if (daysCount > partDay.Items.Count)
            {
                AddDaysToMonth(daysCount);
            }
            if (daysCount < partDay.Items.Count)
            {
                RemoveDaysFromMonth(daysCount);
            }
        }

        /// <summary>
        /// Удаляет лишние дни из выбранного месяца.
        /// </summary>
        /// <param name="totalDaysCount">Необходимое количество дней в выбранном месяце / году.</param>
        private void RemoveDaysFromMonth(Int32 totalDaysCount)
        {
            Int32 startNumber = totalDaysCount + 1;
            Int32 endNumber = partDay.Items.Count;
            // При уменьшении проверить выбранный элемент.
            Int32 selectedItemIndex = partDay.SelectedIndex;
            if (selectedItemIndex > totalDaysCount - 1)
                partDay.SelectedIndex = totalDaysCount - 1;
            for (Int32 i = startNumber; i <= endNumber; i++)
                partDay.Items.Remove(i);
        }

        /// <summary>
        /// Добавляет необходимое количество дней в список.
        /// </summary>
        /// <param name="totalDaysCount">Необходимое количество дней в выбранном месяце / году.</param>
        private void AddDaysToMonth(Int32 totalDaysCount)
        {
            Int32 startNumber = partDay.Items.Count + 1;
            for (Int32 i = startNumber; i <= totalDaysCount; i++)
                partDay.Items.Add(i);
            if (startNumber == 1)
                partDay.SelectedIndex = 0;
        }

        /// <summary>
        /// Устанавливает состояние элементов управления.
        /// </summary>
        private void UpdateSelectedDate()
        {
            Int32 selectedYear = Convert.ToInt32(partYear.SelectedItem);
            Int32 selectedMonth = Convert.ToInt32(partMonth.SelectedIndex) + 1;
            Int32 selectedDay = Convert.ToInt32(partDay.SelectedItem);
            SelectedDate = new DateTime(selectedYear, selectedMonth, selectedDay);
        }

        /// <summary>
        /// При выборе другого дня обновляет дату.
        /// </summary>
        private void partDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (partDay.SelectedIndex == -1)
                partDay.SelectedIndex = 0;
            UpdateSelectedDate();
        }

        public DateSelectorStyle ControlStyle
        {
            get { return (DateSelectorStyle)GetValue(ControlStyleProperty); }
            set { SetValue(ControlStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateSelectorStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlStyleProperty;

        /// <summary>
        /// Обрабатывает изменение стиля элемента.
        /// </summary>
        private static void DateSelectorStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateSelectorStyle newStyle = (DateSelectorStyle)e.NewValue;
            DateSelector dateSelector = (DateSelector)sender;

            String resourceUri = String.Empty;
            switch (newStyle)
            {
                case DateSelectorStyle.Default:
                    resourceUri = "generic.xaml";
                    break;
                case DateSelectorStyle.Touchscreen:
                    resourceUri = "touchscreen.xaml";
                    break;
                case DateSelectorStyle.TouchscreenYearOnly:
                    resourceUri = "touchscreen.xaml";
                    break;
            }
            dateSelector.OnControlStyleChanged(resourceUri, true);
        }

        private string GetTemplateName(DateSelectorStyle type)
        {
            switch (type)
            {
                case DateSelectorStyle.Default:
                    return "Default";
                case DateSelectorStyle.Touchscreen:
                    return "Touchscreen";
                case DateSelectorStyle.TouchscreenYearOnly:
                    return "TouchscreenYearOnly";
                default:
                    return null;
            }
        }
        protected void OnControlStyleChanged(String resourceUri, Boolean applyTemplate)
        {
            ResourceDictionary genericXaml = ResourceManager.GetResourceDictionary(resourceUri);
            if (genericXaml == null)
            {
                this.Template = null;
                return;
            }
            string templateName = GetTemplateName(this.ControlStyle);
            if (string.IsNullOrEmpty(templateName))
            {
                this.Template = null;
                return;
            }
            this.Template = genericXaml[templateName] as ControlTemplate;
            if (applyTemplate)
            {
                this.ApplyTemplate();
            }            
        }

        public enum DateSelectorStyle
        {
            Default,
            Touchscreen,
            TouchscreenYearOnly
        }

        internal static class ResourceManager
        {
            private static Dictionary<string, ResourceDictionary> _resourceDictionaryCache = new Dictionary<string, ResourceDictionary>();
            /// <summary>    
            /// GetResourceDictionary    
            /// </summary>    
            public static ResourceDictionary GetResourceDictionary(string name)
            {
                var resourceName = name;
                if (string.IsNullOrEmpty(resourceName)) { resourceName = "themes/generic.xaml"; }
                ResourceDictionary retVal = null;
                if (_resourceDictionaryCache.TryGetValue(resourceName, out retVal)) { return retVal; }
                System.Reflection.Assembly assembly = typeof(ResourceManager).Assembly;
                string baseName = string.Format("{0}.g", assembly.FullName.Split(new char[] { ',' })[0]);
                string fullName = string.Format("{0}.resources", baseName);
                foreach (string s in assembly.GetManifestResourceNames())
                {
                    if (s == fullName)
                    {
                        System.Resources.ResourceManager manager = new System.Resources.ResourceManager(baseName, assembly);
                        System.Resources.ResourceSet set = manager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                        foreach (System.Collections.DictionaryEntry entry in set)
                        {
                            Console.WriteLine("{0} {1}", entry.Key, entry.Value);
                        }
                        using (System.IO.Stream stream = manager.GetStream(resourceName))
                        {
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                            {
                                retVal = System.Windows.Markup.XamlReader.Load(stream) as ResourceDictionary;//reader.ReadToEnd()
                                if (retVal != null)
                                {
                                    _resourceDictionaryCache.Add(resourceName, retVal);
                                    return retVal;
                                }
                            }
                        }
                        break;
                    }
                }
                return null;
            }
        }

    }
}
