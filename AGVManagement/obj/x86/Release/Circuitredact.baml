   M S B A M L   `   `   `  ���� H  DAGVManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AGVManagementAGVManagement  !    AGVManagement.Circuitredact   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:AGVManagement       � Title$  Circuitredact��$	��840��$
��1550�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � Icon$ Images/线路.png��.��� .+��  � RowDefinitions �� $
��0.5* ��� $��2* ��� $
��7.5* � � ColumnDefinitions �� $
��3.5* ��� $��6 ��� $
��6.5* �.+��� $��2��$��线路信息��$��15(�.���  �� $
��3.5* ��� $
��3.5* ��� $��3* � �� $��1* ��� $��3* ��� $��1* ��� $��3* ��� $��3* �.+��� $��地图:��$��35��$��16���� -   $��maplist��$��1�� �� SelectedIndex$ 0��$��5,0,5,0q��� $��线路:��$��35��$��16��$��2��$��5,0,5,0q��� -   $��lineRo��$��3��$��34��$��5,0,5,0q��� $��4��$��	比例尺��$��16��$��45��$��0,0,0,30q��� -   $��SliMax��$��4�� � Cursor$
 Hand�� � Maximum$ 5Q�	 � Minimum$	 0Q�
 �� TickFrequency$
 1Q�$��5,30,5,0q��� -   $��	ProgramNO��$�� ��
 � Tag$ 
线路号:��$��1��$��2��   input-group-addon left#���   $��5,0,5,0q��� -   $��ProgramName��$�� ��$ 
线路名:��$��1��$��2��$��2��#���   $��5,0,5,0q��� -   $��	SubmitPro��$��保存��$��1��$��4��$��18(��� -   $��AddPro��$��新建��$��2��$��1��$��18(��� -   $��DelPro��$��删除��$��2��$��3��$��18(��� -	   $
��Grop��$��线路编辑��$
 Hand��$��15(�$��2��.��&   System.Windows.Controls.DataGrid  -
   $��EditlineData��   SelectionMode$ Single=�   
IsReadOnly .    FrozenColumnCount$ 1��   CanUserAddRows .     CanUserDeleteRows .  ����    RowStyle ��  �� 
TargetType)  #System.Windows.Controls.DataGridRow"  .���  �� Property � �� �� Value �#F2F2F2��  � ��$ 30����  � ��
 �   � �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  � �� �#191970��  � �� �#191970P�  �@ AlternationIndex$ 0��. ���  � �� �#e7e7e7P�  �@ AlternationIndex$ 1��. ���  � �� �#f2f2f2P�  �@J�IsMouseOver . . ���  � ��
 �����P�  �@ 
IsSelected . . ���  � ��
 �   ���  � �� �#40E0D0   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 信标��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 60    
Foreground
 �   �   
FontWeight$ Black"�   FontSize$ 20Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Tag+  $ 速度��$ *  �� *Speed+  $	 PBS��$ *  �� *Pbs+  $ 转向��$ *  �� *Turn+  $ 方向��$ *  �� *	Direction+  $
 Hook�� �� *Hook+  $ 停止时间��$
 1.2*  �� *Stop+  $ 程序��$ *  �� *ChangeProgram+�� $��3��$��3��$��地图信息��$��15(�.���� -   $��	GetScroll�����#F2F2F2$
��Auto=�.���� -   $��MapIN�����#F2F2F2�� $��1��$��3��$��2��$��2,0,2,0q�