﻿using GUIForDiskpart.Presentation.Presenter;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for ConsoleReturn.xaml
    /// </summary>
    public partial class UCLog : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ESaveLog;
        
        public delegate void DOnTextChanged(object sender, TextChangedEventArgs e);
        public event DOnTextChanged ELogUI;


        public UCLog()
        {
            InitializeComponent();
        }

        public void SaveLog_Click(object sender, RoutedEventArgs e) => ESaveLog?.Invoke(sender, e);
        public void LogUI_TextChanged(object sender, TextChangedEventArgs e) => ELogUI?.Invoke(sender, e);
    }
}
