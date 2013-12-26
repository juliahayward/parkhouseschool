@echo on

set dest=W:\parkhouseschool

xcopy /c /e /r /y bin %dest%\bin
xcopy /c /e /r /y images %dest%\images
xcopy /c /e /r /y *.aspx %dest%
xcopy /c /e /r /y *.Master %dest%
xcopy /c /e /r /y *.css %dest%

pause
