   M S B A M L   `   `   `  ���� H  DAGVManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AGVManagementAGVManagement      AGVManagement.Main   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:AGVManagement       � Title$  AGV Management��$
��1043��$
��1520�� � Icon$ Images/机器人.png�� � WindowState$ 	Maximized=�$	��800��$	��700��.��� .+��  � RowDefinitions �� $��35 ��� $��45 ��� $	��60* ��� $��3 ��� $	��30* � � ColumnDefinitions �� $��8* ��� $��3 ��� $��2* �.+�o� -   $
��Menu��$��3��$��20(����	#FFE6E3E3$��35��$��Microsoft YaHei)�.@��� $��20��m� $��用户��$��35�� m� Icon �� $��/Images/用户.png��.@�m� $��注销�� �� $��/Images/退出.png��m� $��用户管理�� �� $��/Images/退出.png��m� -   $��PortDern��$��串口设置�� �� $��/Images/串口.png���� $��10���� $��10��m� -   $��Map_Add��$��新建地图�� �� $��/Images/添加.png��m� -   $��Map_btn��$��编辑地图�� �� $��/Images/地图.png��m� -   $��Circui��$��编辑线路�� �� $��/Images/线路.png��m� -   $��beaconCR��$��编辑信标�� �� $��/Images/信标.png���� $��10���� $��10��m� $��历史���� $��10��m� $��关于���� $��1��$��3�����#FFE4C4 �� Orientation$ 
Horizontal=�.+��� -   $
��Open��$��20(�{�.  $��打开串口��$��34��$	��110��   btn-success#���   $��20,0,0,0q��� -    ��Name$ 	SwitchImg��$#��Images/电子元器件红.png��$��24��$
��10,0q��� -	    ��Name$ 
SwitchText��$��20(�$��串口状态：关��$��25��$
��0,10q��� -
   	 ��Name$	 	Btnswitch��$��20(�{�.  $��关闭串口��$��35��$	��110��  
btn-danger#���  $��20,0,0,0q��� $��20(�$��	地图：��$��25��$��
20,10,0,10q��� -   
 ��Name$
 Maplistq��$��18(�$��宋体)�$��Black"� �� SelectedIndex$ 0��$	��160��$��10,0,0,0q��� $��20(�$��调试��$��80��$��35��  btn-info#���  $��50,0,0,0q��� $��2�����#F2F2F2$
��Auto=�.���� -   $��MapIN�����#F2F2F2�� $��2��$��2��$��16(�.@��� $��	AGV信息��.��&   System.Windows.Controls.DataGrid  -     Name$ 
TabAgvData������ �� $��串口信息��.��  -   $ TabSerialPortData������ �� $��16(�$��4��$��3��.@��� $��运行信息��.��  -   $ TabAgvMoveData�� � Cursor$
 Hand��   SelectionMode$ Single=�   
IsReadOnly . ����    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 状态��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5*    CanUserReorder .     CellTemplate �� .R��� $��
CheckBoxDN��
���  ��  checkbox has-error#���   J� IsHitTestVisible .  ���� *type+ d� 	IsChecked Z� 0  *System.Windows.Controls.DataGridTextColumn  $ 	AGV编号��$ *    
Foreground
 �   �   
FontWeight$ Black"�   FontSize$ 20Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	TagName+  $ 速度��$ * 
 �   � �� *Speed+  $ 转弯��$ * 
 �   � �� *turn+  $ 方向��$ * 
 �   � �� *Dir+  $
 Hook��$ * 
 �   � �� *Hook+  $
 RFID��$ * 
 �   � �� *Rfid+  $ Program��$ * 
 �   � �� *	Program+  $
 Step��$ * 
 �   � �� *Step+�� $��通道信息��.���� $
��Auto=�$
��Auto=�.���� -   $	��Log���� $��2��$��1��$��Stretch=�
�������$��3���� $��3��$��Stretch=�$��5��
�������$��3��