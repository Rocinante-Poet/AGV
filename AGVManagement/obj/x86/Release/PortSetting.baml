   M S B A M L   `   `   `  ���� H  DAGVManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AGVManagementAGVManagement      AGVManagement.PortSetting   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:AGVManagement       � Title$  串口设置��$	��450��$��851.136�� � Icon$ /Images/串口.png�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=�.��� .+��  � RowDefinitions ��  � ColumnDefinitions �� �� .+�� $��0�� ��  �� $��80 ��� .+��� $��地图��$��5q�$��15(�.���  �� $��80 ��� .+��� $��	地图：��$��Center=�$��Center=��� -   $��Maplistq��$��1��$��18(�$��宋体)�$��Black"�$��Center=� �� SelectedIndex$ 0���� $��串口信息��$��5q�$��15(�$��0��$��1��.��&   System.Windows.Controls.DataGrid  -   $��	PortTable��   SelectionMode$ Single=� � Cursor$
 Hand��	   RowStyle	 �� 
 �� 
TargetType)  #System.Windows.Controls.DataGridRow"
  .���  �� Property � �� �� Value �#F2F2F2��  � ��$ 30����  � ��
 �   � �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  � �� �#191970��  � �� �#191970P�  �@ AlternationIndex$ 0��. ���  � �� �#e7e7e7P�  �@ AlternationIndex$ 1��. ���  � �� �#f2f2f2P�  �@J�IsMouseOver . . ���  � ��
 �����P�  �@ 
IsSelected . . ���  � ��
 �   ���  � �� �#009966   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	串口号��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5* 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Com+  $ 	波特率��$
 0.5*  �� *Baud+  $ 
AGV/其他��$
 0.5*  �� *Agv+�� $��1��$��5q�$��串口设置��$��15(�.��� $��8,0,8,0q� �� �� �� �� ��  �� $	��100 ��� .+��� $��串口号：��$��0��$��0��$��Center=�$��Center=��� -   $	��Com��$��1��$��0���� $��波特率：��$��0��$��1��$��Center=�$��Center=��� -   $
��Baud��$��1��$��1�� �� 
IsEditable . $ 0��.@��� .��7200�� .��14400�� .��19200�� .��38400�� .��9600�� $��AGV/其他：��$��0��$��2��$��Center=�$��Center=��� -   $	��Agv��$��1��$��2�� . .@��� .��按钮�� .��	充电机�� -   $��PortAdd��$��3��$��添加��$
 Hand��$��2��$��	0,0,250,0q�$	��100��$��40��$��18(��� -   $��
PortUpdate��$��3��$��修改��$
 Hand��$��2��$	��100��$��40��$��18(��� -   $��
PortDelete��$��3��$��删除��$
 Hand��$��2��$��0,0,-250,0 q�$	��100��$��40��$��18(��� -	   $��Insert��$��4��$��提交��$
 Hand��$��Center=�$��18(�$��0,0,0,0q�$	��353��$��50��$��2��