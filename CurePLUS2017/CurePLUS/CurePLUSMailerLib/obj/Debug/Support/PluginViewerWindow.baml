   M S B A M L   `   `   `  ����L  HCurePLUSMailerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CurePLUSMailerLibCurePLUSMailerLib  5   	   *    $CurePLUSMailerLib.PluginViewerWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CurePLUSMailerLib     5   	     � Title$  CurePLUS Pluginビュア-��5   	   $	��391��6%   $	��553��62    � 
ResizeMode$ 	CanResize=�6>   ���#FFF0F0$	��553��6j   $	��553��6y   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��143* �6   �� 5      $	��330 �6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   o� $
��Left=�6   $	��Top=�6*   $
��auto��6B   $��2��6O   .@�5      m� $��ファイル(_F)��6   .@�5      m� -   5         m�Name$ _miFileOpenPlugin��5      $"��プラグインを開く(_O)��66   6L   m� 5      -   5        $ _miFileExit��5      $��閉じる(_X)��60   6A   5      m� 5      $��
表示(_V)��6   $y�Hidden=�6'   .@�5      m� -   5        $ _miViewDirectInput��5      $��直接入力(_D)��67   6I   5      5      �� 5   
   $��1��6   $��0��6"   ���#FFE0E0$��
10,0,10,10q�6G   .+�5      �� -   5         ��Name$ 	_mailList��5      $��10q�6*   $y�Visible=�6d   6{   �� 5      -   5         ��Name$ _targets��5      $	�� 10q�6(    �� SelectionMode$ Single=�65   $y�Visible=�6z   6�   ]� 5      $
��auto��6   .+�5      �� -   5         ��Name$ _directInput��5      $
��auto��60   $y�Visible=�6>   $��10q�6z   	 �� AcceptsReturn	 . 6�   5       5!      �� 5"   
   $��1��6   $��1��6"   $��Center=�62   .+�5#      '   !CurePLUSMailerLib.MailContentView  -   5        
  Name$
 _content��5#   $   $	��320��67   $	��274��6D   ���#FFE0E06P   5$      5%      5&      