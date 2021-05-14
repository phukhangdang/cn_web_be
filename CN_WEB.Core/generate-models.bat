@ECHO OFF
ECHO FPTCS - TOOL
SET mypath=%cd%

CALL:CYAN "1. Generate database context"
CMD /c dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=123456;database=cn_web" MySql.Data.EntityFrameworkCore -o Model -c SysDbContext -f
CALL:GREEN "Build succeeded"

CALL:CYAN "2. Restore model"
xcopy /s CustomModel Model /Y

PAUSE

:GREEN
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Green %1
goto:eof
:CYAN
%Windir%\System32\WindowsPowerShell\v1.0\Powershell.exe write-host -foregroundcolor Cyan %1
goto:eof
