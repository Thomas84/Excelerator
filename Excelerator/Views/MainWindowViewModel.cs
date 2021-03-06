﻿namespace Gensler.Revit.Excelerator.Views
{
    using Autodesk.Revit.DB;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Collections.Specialized;
    using Models;

    class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _isLoaded;
        private string _excelPath;
        private Category _selectedCategory;
        private ParamField _selectedParameter;
        private ExcelItem _selectedExcelItem;
        private ObservableCollection<Category> _categoryItems;
        private ObservableCollection<ParamField> _parameterItems;
        private ObservableCollection<ExcelItem> _excelItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public Importer Importer { get; private set; }

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsLoaded)));
            }
        }

        public string ExcelPath
        {
            get => _excelPath;
            set
            {
                _excelPath = value;
                Importer = new Importer(value);
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ExcelPath))); 
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }

        public ParamField SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                _selectedParameter = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedParameter)));
            }
        }

        public ExcelItem SelectedExcelItem
        {
            get => _selectedExcelItem;
            set
            {
                if (_selectedExcelItem != null)
                    _selectedExcelItem.IsActive = false;

                _selectedExcelItem = value;

                if (_selectedExcelItem != null)
                    _selectedExcelItem.IsActive = true;

                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedExcelItem)));
            }
        }

        public ObservableCollection<Category> CategoryItems
        {
            get => _categoryItems;
            set
            {
                _categoryItems = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(CategoryItems)));
            }
        }

        public ObservableCollection<ParamField> ParameterItems
        {
            get => _parameterItems;
            set
            {
                _parameterItems = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ParameterItems)));
            }
        }

        public ObservableCollection<ExcelItem> ExcelItems
        {
            get => _excelItems;
            set
            {
                _excelItems = value;
                _excelItems.CollectionChanged += ExcelItemsCollectionChanged;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ExcelItems)));
            }
        }

        private void ExcelItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (ExcelItem newItem in e.NewItems)
                    newItem.PropertyChanged += OnPropertyChanged;

            if (e.OldItems != null)
                foreach (ExcelItem oldItem in e.OldItems)
                    oldItem.PropertyChanged -= OnPropertyChanged;
        }

        public FileCommand FileCommand { get; set; }

        public SelectCatCommand SelectCatCommand { get; set; }

        public SelectParamCommand SelectParamCommand { get; set; }

        public SelectExcelCommand SelectExcelCommand { get; set; }

        public AddCommand AddCommand { get; set; }

        public RemoveCommand RemoveCommand { get; set; }

        public EditCommand EditCommand { get; set; }

        public RunCommand RunCommand { get; set; }

        public MainWindowViewModel()
        {
            FileCommand = new FileCommand(this);
            SelectCatCommand = new SelectCatCommand(this);
            SelectParamCommand = new SelectParamCommand(this);
            SelectExcelCommand = new SelectExcelCommand(this);
            AddCommand = new AddCommand(this);
            RemoveCommand = new RemoveCommand(this);
            EditCommand = new EditCommand(this);
            RunCommand = new RunCommand(this);

            CategoryItems = new ObservableCollection<Category>(Importer.RevitCategories.OrderBy(x => x.Name));
            ParameterItems = new ObservableCollection<ParamField>();
            ExcelItems = new ObservableCollection<ExcelItem>();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
    }
}
