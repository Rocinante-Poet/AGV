﻿#pragma checksum "..\..\MapInfo.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "007F329A2D8973A67829B634AF05A727AE25F2B1"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using AGVManagement;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace AGVManagement {
    
    
    /// <summary>
    /// Map
    /// </summary>
    public partial class Map : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 268 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SelectMap;
        
        #line default
        #line hidden
        
        
        #line 277 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid MapData;
        
        #line default
        #line hidden
        
        
        #line 324 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl MapName;
        
        #line default
        #line hidden
        
        
        #line 328 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas MapIN;
        
        #line default
        #line hidden
        
        
        #line 340 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddMap;
        
        #line default
        #line hidden
        
        
        #line 341 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CompileMap;
        
        #line default
        #line hidden
        
        
        #line 343 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Channel;
        
        #line default
        #line hidden
        
        
        #line 344 "..\..\MapInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Derive;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AGVManagement;component/mapinfo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MapInfo.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SelectMap = ((System.Windows.Controls.TextBox)(target));
            
            #line 268 "..\..\MapInfo.xaml"
            this.SelectMap.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new System.Windows.RoutedEventHandler(this.SelectMap_Click));
            
            #line default
            #line hidden
            return;
            case 2:
            this.MapData = ((System.Windows.Controls.DataGrid)(target));
            
            #line 277 "..\..\MapInfo.xaml"
            this.MapData.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.MapData_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.MapName = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 4:
            this.MapIN = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            this.AddMap = ((System.Windows.Controls.Button)(target));
            
            #line 340 "..\..\MapInfo.xaml"
            this.AddMap.Click += new System.Windows.RoutedEventHandler(this.AddMap_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CompileMap = ((System.Windows.Controls.Button)(target));
            
            #line 341 "..\..\MapInfo.xaml"
            this.CompileMap.Click += new System.Windows.RoutedEventHandler(this.CompileMap_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Channel = ((System.Windows.Controls.Button)(target));
            
            #line 343 "..\..\MapInfo.xaml"
            this.Channel.Click += new System.Windows.RoutedEventHandler(this.Channel_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Derive = ((System.Windows.Controls.Button)(target));
            
            #line 344 "..\..\MapInfo.xaml"
            this.Derive.Click += new System.Windows.RoutedEventHandler(this.Derive_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
