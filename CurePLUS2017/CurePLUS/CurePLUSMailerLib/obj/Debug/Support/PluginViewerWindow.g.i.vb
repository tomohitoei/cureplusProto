﻿#ExternalChecksum("..\..\..\Support\PluginViewerWindow.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","CBA4BEA2DBCF7ACE549C322812B7F339")
'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports CurePLUSMailerLib
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''PluginViewerWindow
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class PluginViewerWindow
    Inherits System.Windows.Window
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",20)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _miFileOpenPlugin As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",21)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _miFileExit As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",24)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _miViewDirectInput As System.Windows.Controls.MenuItem
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",28)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _mailList As System.Windows.Controls.ComboBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",29)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _targets As System.Windows.Controls.ListBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",31)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _directInput As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",35)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents _content As CurePLUSMailerLib.MailContentView
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/CurePLUSMailerLib;component/support/pluginviewerwindow.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
    Friend Function _CreateDelegate(ByVal delegateType As System.Type, ByVal handler As String) As System.[Delegate]
        Return System.[Delegate].CreateDelegate(delegateType, Me, handler)
    End Function
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me._miFileOpenPlugin = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",20)
            AddHandler Me._miFileOpenPlugin.Click, New System.Windows.RoutedEventHandler(AddressOf Me._miFileOpenPlugin_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 2) Then
            Me._miFileExit = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",21)
            AddHandler Me._miFileExit.Click, New System.Windows.RoutedEventHandler(AddressOf Me._miFileExit_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 3) Then
            Me._miViewDirectInput = CType(target,System.Windows.Controls.MenuItem)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",24)
            AddHandler Me._miViewDirectInput.Click, New System.Windows.RoutedEventHandler(AddressOf Me.MenuItem_Click)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 4) Then
            Me._mailList = CType(target,System.Windows.Controls.ComboBox)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",28)
            AddHandler Me._mailList.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me._mailList_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 5) Then
            Me._targets = CType(target,System.Windows.Controls.ListBox)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",29)
            AddHandler Me._targets.SelectionChanged, New System.Windows.Controls.SelectionChangedEventHandler(AddressOf Me._targets_SelectionChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 6) Then
            Me._directInput = CType(target,System.Windows.Controls.TextBox)
            
            #ExternalSource("..\..\..\Support\PluginViewerWindow.xaml",31)
            AddHandler Me._directInput.TextChanged, New System.Windows.Controls.TextChangedEventHandler(AddressOf Me._directInput_TextChanged)
            
            #End ExternalSource
            Return
        End If
        If (connectionId = 7) Then
            Me._content = CType(target,CurePLUSMailerLib.MailContentView)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

