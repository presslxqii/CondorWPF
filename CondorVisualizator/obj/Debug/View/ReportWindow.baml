   M S B A M L   `   `   `  ����  CondorVisualizator*    $CondorVisualizator.View.ReportWindow   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Протокол��5   	   $	��219��6   $	��212��6'    � Icon$( "../Resources/Iconic_e027(0)_64.png��63    � 
ResizeMode$ NoResize=�6]    � WindowStartupLocation$ CenterScreen=�6s    � WindowStyle$
 None=�6�    � DataContext 5      �� 5   
    �� Path$ Report�6      Locator �� Source# [  6    .��5      �� 
�������$��3q�6)   ._�5	   
   � -   5         �Name$ WindowReport��5	      ���#035E81.+�5
      �� -   5        $��Density��5
      $'��!Таблица плотности��6I   $
��Left=�6e   $��	29,54,0,0q�6�   $	��Top=�6�   	 d� 	IsChecked	 6&   �� $ Density�6�   �� 5      $,��&Протокол измерения - ��6   $
��Left=�64   $��	10,10,0,0q�6O   $	��Top=�6b   $	��135��6z   
 J� RenderTransformOrigin$
 0.273,0.462&�6�   �� 5      $
��Left=�69   $��23��6T   $��
151,10,0,0q�6`    �� TextWrapping$
 Wrap=�6t   $	��Top=�6�   $��41��6�   ��6   �� $ TextNumMess�6�   �� 5      $��Применить��6   $
��Left=�6)   $��
10,167,0,0q�6D   $	��Top=�6X   $��83��6p   ��6{   �� *DataContext.OkRelayCommand+ �� ElementName$ WindowReport���� 5      $��Отмена��6   $
��Left=�6&   $��110,167,0,0q�6A   $	��Top=�6V   $��82��6n   ��6y   �� *DataContext.CloseRelayCommand+$ WindowReport���� 5      -   5        $��Thicknesses��5      $,��&Таблица толщин слоёв��6Q   $
��Left=�6p   $��
29,118,0,0q�6�   $	��Top=�6�   	 6*   �� $ Thicknesses�6�   �� 5      -   5        $��Rutting��5      $)��#Таблица колейности��6J   $
��Left=�6g   $��	29,86,0,0q�6�   $	��Top=�6�   	 6&   �� $ Rutting�6�   5      5      5      