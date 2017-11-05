﻿using Chem4Word.Core.UI.Forms;
using Chem4Word.View;
using Microsoft.Office.Interop.Word;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static Chem4Word.Core.UserInteractions;
using UserControl = System.Windows.Controls.UserControl;

namespace Chem4Word.Library
{
    /// <summary>
    /// Interaction logic for LibraryItemControl.xaml
    /// </summary>
    public partial class LibraryItemControl : UserControl
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public LibraryItemControl()
        {
            InitializeComponent();
        }

        public LibraryViewControl ParentControl
        {
            get
            {
                return VisualTreeHelpers.FindAncestor<LibraryViewControl>(this);
            }
        }

        public LibraryViewModel MyViewModel
        {
            get
            {
                return ParentControl.MainGrid.DataContext as LibraryViewModel;
            }
        }

        private void InsertCopyButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            Globals.Chem4WordV3.Telemetry.Write(module, "Action", "Triggered");
            try
            {
                ActiveDocument = Globals.Chem4WordV3.Application.ActiveDocument;
                if (ActiveDocument?.ActiveWindow?.Selection != null)
                {
                    Navigator.NavigatorSupport.InsertChemistry(true, ActiveDocument.Application, FlexDisplay);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public Document ActiveDocument
        {
            get { return (Document)GetValue(ActiveDocumentProperty); }
            set { SetValue(ActiveDocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveDocument.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveDocumentProperty =
            DependencyProperty.Register("ActiveDocument", typeof(Document), typeof(LibraryItemControl), new PropertyMetadata(null));

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (AskUserYesNo("Do you want to delete this structure from the Library?") == DialogResult.Yes)
                {
                    LibraryModel.DeleteChemistry(((LibraryViewModel.Chemistry)this.DataContext).ID);
                    Globals.Chem4WordV3.LibraryNames = LibraryModel.GetLibraryNames();
                    ParentControl.MainGrid.DataContext = new LibraryViewModel();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void DelTag_OnClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void LibraryControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //LibraryViewModel currentModel = ParentControl.MainGrid.DataContext as LibraryViewModel;
            //currentModel.SaveChanges();
        }

        private void LibraryControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //UserInteractions.InformUser("Clicked the ellipse");
        }
    }
}