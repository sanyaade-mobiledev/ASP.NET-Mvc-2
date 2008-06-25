IF EXIST %1\NORTHWND.MDF GOTO Done
md %1
copy "%~dp0NORTHWND.MDF" %1
:Done
