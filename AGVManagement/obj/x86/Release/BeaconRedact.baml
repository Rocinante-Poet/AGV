   M S B A M L   `   `   `  ���� H  DAGVManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AGVManagementAGVManagement       AGVManagement.BeaconRedact   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:AGVManagement       � Title$  BeaconRedact��$��833.43��$��1541.756�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.��� .+��  � RowDefinitions �� $��80 ���  � ColumnDefinitions �� $	��600 ��� $��10 ��� .+��� $��1��$��3��$��0��$��0,0,5,0q�$��Stretch=����#F2F2F2$��1���� $��地图信息��$��10,5,5,5q�$��15(�$	��585��$
��Left=�.���  ��  �� $��50 ��� �� �� $��60 ��� .+��� $��地图:��$��Center=�$��Center=��� -   $��maplist��$��1�� �� SelectedIndex$ 0���� -   $��Submit��$��保存��$��2��$	��100��$��18(��� $��3��$��
比例尺:��$��Center=�$��Center=��� -    ��Name$ SliMax��$��4��$��Center=� � Cursor$
 Hand�� � Maximum$ 5Q�	 � Minimum$	 0Q�
 �� TickFrequency$
 1Q��� -   $
��Grop��$��信标编辑��$
 Hand��$��15(�$��0��$��1��$��	10,5,5,10q�.��&   System.Windows.Controls.DataGrid  -   $��Beacon��   SelectionMode$ Single=�   
IsReadOnly .    FrozenColumnCount$ 1��   CanUserAddRows .     CanUserDeleteRows .  ����    RowStyle ��  �� 
TargetType)  #System.Windows.Controls.DataGridRow"  .���  �� Property � �� �� Value �#F2F2F2��  � ��$ 30����  � ��
 �   � �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  � �� �#191970��  � �� �#191970P�  �@ AlternationIndex$ 0��. ���  � �� �#e7e7e7P�  �@ AlternationIndex$ 1��. ���  � �� �#f2f2f2P�  �@J�IsMouseOver . . ���  � ��
 �����P�  �@ 
IsSelected . . ���  � ��
 �   ���  � �� �#40E0D0   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 信标��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 80    
Foreground
 �   �   
FontWeight$ Black"�   FontSize$ 20Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	TagName+  $ 前方��$ 80  �� *PreTag+  $ 	左前方��$ 80  �� *
PreLeftTag+  $ 	右前方��$ 80  �� *PreRightTag+  $ 后方��$ 80  �� *	NextTag+  $ 	左后方��$ 80  �� *NextLeftTag+  $ 	右后方��$ 80  �� *NextRightTag+  $ 前进速度��$ 80  �� *Speed+  $ 后退速度��$ 80  �� *
SpeedRev+  $ 停止时间��$ 80  �� *
StopTime+  $ 前PBS��$ 80  �� *Pbs+  $ 后PBS��$ 80  �� *PbsRev+  $ 站点��$ 80  �� *TagTerminal+��    panel-default#��[  $��0��$��2��$��40��$��
0,-13,10,0q�.���� .+��� -   $��MapName�����#DBDBDB$��Black"�  panel-heading-default#��[ $��地图信息���� -   $��	ScrollMap�����#F2F2F2$��0��$
��Auto=�$
��Auto=�$��2��$��2��$��	0,53,5,10q�.���� -   $��MapIN�����#F2F2F2