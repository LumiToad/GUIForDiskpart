﻿#pragma checksum "..\..\..\..\userControls\FreeSpaceEntryUI.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F19855AAB840080D5EC9B63445219D5A82FC0CB7"
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
    /// FreeSpaceEntryUI
    /// </summary>
    public partial class FreeSpaceEntryUI : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label UnpartSpaceLabel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label UnPartSpaceValue;
        
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
            System.Uri resourceLocater = new System.Uri("/GUIForDiskpart;V1.0.2310.168;component/usercontrols/freespaceentryui.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
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
            
            #line 14 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreatePartition_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateVolume_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 16 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateVDisk_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 19 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MultiCreate_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 27 "..\..\..\..\userControls\FreeSpaceEntryUI.xaml"
            this.MainGrid.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.UnpartSpaceLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.UnPartSpaceValue = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

