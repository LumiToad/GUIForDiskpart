﻿#pragma checksum "..\..\..\..\windows\FormatWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A9E10CB25916F37405C47E3B31D0858E19B38556"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using GUIForDiskpart;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GUIForDiskpart {
    
    
    /// <summary>
    /// FormatDriveWindow
    /// </summary>
    public partial class FormatDriveWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DiskDetailValue;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FileSystemValue;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VolumeValue;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SizeValue;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DriveLetterValue;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox QuickFormattingValue;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CompressionValue;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ConfirmButton;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CancelButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\windows\FormatWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ErrorMessageValue;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUIForDiskpart;V1.0.2310.168;component/windows/formatwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\windows\FormatWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.DiskDetailValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.FileSystemValue = ((System.Windows.Controls.ComboBox)(target));
            
            #line 13 "..\..\..\..\windows\FormatWindow.xaml"
            this.FileSystemValue.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.VolumeValue = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.SizeValue = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\..\..\windows\FormatWindow.xaml"
            this.SizeValue.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SizeValue_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DriveLetterValue = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.QuickFormattingValue = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.CompressionValue = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.ConfirmButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\windows\FormatWindow.xaml"
            this.ConfirmButton.Click += new System.Windows.RoutedEventHandler(this.ConfirmButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\windows\FormatWindow.xaml"
            this.CancelButton.Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.ErrorMessageValue = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

