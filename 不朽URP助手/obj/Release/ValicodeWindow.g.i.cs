﻿#pragma checksum "..\..\ValicodeWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ADE6E6968F47860ACC63326C0FAE13F4"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

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
using 不朽URP助手;


namespace 不朽URP助手 {
    
    
    /// <summary>
    /// ValicodeWindow
    /// </summary>
    public partial class ValicodeWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\ValicodeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid_vali;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\ValicodeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbx_tip;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ValicodeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img_vali;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ValicodeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbx_valicode;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\ValicodeWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btu_submit;
        
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
            System.Uri resourceLocater = new System.Uri("/不朽URP助手;component/valicodewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ValicodeWindow.xaml"
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
            this.grid_vali = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.tbx_tip = ((System.Windows.Controls.TextBlock)(target));
            
            #line 24 "..\..\ValicodeWindow.xaml"
            this.tbx_tip.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.tbx_tip_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.img_vali = ((System.Windows.Controls.Image)(target));
            
            #line 26 "..\..\ValicodeWindow.xaml"
            this.img_vali.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.img_vali_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbx_valicode = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\ValicodeWindow.xaml"
            this.tbx_valicode.KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btu_submit = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\ValicodeWindow.xaml"
            this.btu_submit.Click += new System.Windows.RoutedEventHandler(this.btu_submit_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

