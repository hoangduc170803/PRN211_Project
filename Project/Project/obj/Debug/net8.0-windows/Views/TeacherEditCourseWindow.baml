   M S B A M L   `   `   `  ����B  >Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Project.ConvertersProject.Converters  5   	   +    %Project.Views.TeacherEditCourseWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1conv clr-namespace:Project.Converters     5   	     � Title$  Sửa Khóa Học��5   	   $	��400��6   $	��400��6+    � WindowStartupLocation$ CenterScreen=�67    � 	Resources 5      4   .Project.Converters.DateOnlyToDateTimeConverter   DateOnlyToDateTimeConverter��%   &	          5   
   6+   5      .��5	      � $��10q�6   .+�5
   
   �� .+�5      �� $��Sửa Khóa Học��6   $��20(�5      $
��Bold"�6&   $��0,0,0,20q�5      $��Center=�6*   �� 5       �� Orientation$ 
Horizontal=�6   $	��0,5q�62   .+�5      �� $��Tên khóa học:��6   $	��120��60   �� 5      -   5         ��Name$ txtCourseName��5      $	��200��61   ��6=   �� *
CourseName+ �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5      $ 
Horizontal=�6   $	��0,5q�62   .+�5      �� $��Ngày bắt đầu:��6   $	��120��60   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpStartDate��5      $	��200��62      SelectedDate 5      �� *	StartDate+ �� 	Converter# [  $ PropertyChanged=�5      �� 5      $ 
Horizontal=�6   $	��0,5q�62   .+�5      �� $��Ngày kết thúc:��6   $	��120��61     5      -   5        $ 	dpEndDate��5      $	��200��60    5      �� *	EndDate+# [  $ PropertyChanged=�5      �� 5      $ 
Horizontal=�6   $��Center=�62   $
��0,20q�6O   .+�5      �� -   5        $
��Lưu��5      $	��100��6'   $
��10,0q�63   6A   �� 5      -   5        $��Hủy��5      $	��100��6'   $
��10,0q�63   6A   5      5       5!      5"      