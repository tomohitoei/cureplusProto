   M S B A M L   `   `   `  ����H  DWpfSandboxLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WpfSandboxApplicationWpfSandboxApplication  5   	   !    WpfSandboxLib.SettingWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:WpfSandboxApplication     5   	     � Title$  設定��5   	   $��403.024��6   $��530.67��6%    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6J    � WindowStartupLocation$ CenterOwner=�6`   .��5	      �  � ColumnDefinitions 5
   
    � RowDefinitions 5   
   �� 5      $
��336* �6   �� 5      $	��37* �6   .+�5   
   �� $��0��6   .@�5      �� $$��ユーザープロフィール��6   .��5      �  J� RenderTransformOrigin$ 
0.345,0.51&�6    5      �� 5      $	��128 �6+   �� 5      $��* �6+    5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       .+�5"      �� $��	なまえ��6   $��Right=�6*   $��0��6F   $��0��6S   $��Center=�6c   �� 5#      $��0��68   $��1��6E   $��Center=�6U   $��10,0,0,0q�6p   $	��128��6�   $
��Left=�6�   ��6   �� *
Username+6�   �� 5$      $��あだな１��6   $��Right=�6+   $��1��6G   $��0��6T   $��Center=�6d   �� 5%      $��1��69   $��1��6F   $��Center=�6V   $��10,0,0,0q�6q   $	��128��6�   $
��Left=�6�   ��6   �� *	Nickname1+6�   �� 5&      $��あだな２��6   $��Right=�6+   $��2��6G   $��0��6T   $��Center=�6d   �� 5'      $��2��69   $��1��6F   $��Center=�6V   $��10,0,0,0q�6q   $	��128��6�   $
��Left=�6�   ��6   �� *	Nickname2+6�   �� 5(      $��たんじょうび��6   $��Right=�6-   $��3��6I   $��0��6V   $��Center=�6f   �� 5)      $��3��6!   $��1��6.    �� Orientation$ 
Horizontal=�6>   $
��Left=�6W   .+�5*      �� $��32��5,   >   $��Center=�6I   $��10,0,0,0q�6d   ��5*   "   �� *
BirthMonth+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� ValidatesOnExceptions	 . 5,   d   �� 5-      $��がつ��6    $��Center=�6-   �� 5.      $��32��50   >   $��Center=�6I   ��5.   "   �� *
BirthDay+$ PropertyChanged=�	 . 50   I   �� 51      $��にち��6    $��Center=�6-   52      53      54      �� 55      $��メーラー画面��6   59      �� 5:      $��マスコット��6   6'   �� 5;      $��サウンド��6   6&   �� 5<      $��ゲームデータ��6   6(   5=      �� 5>   
   $��1��6   $��LeftToRight=�6"   $ 
Horizontal=�6>   $��Right=�6W   .+�5?      �� -   5        
 ��Name$
 _acceptButton��5?      $��設定��6,   $��64��69   $��5q�6D   6O   �� 5@      -   5        $
 _cancelButton��5@      $��ｷｬﾝｾﾙ��6,   $��64��6<   $��5q�6G   6R   5A      5B      5C      