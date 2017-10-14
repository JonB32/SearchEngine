FROM microsoft/iis
RUN mkdir C:\searchengineservice
RUN powershell -NoProfile -Command \
    Import-module IISAdministration; \
    New-IISSite -Name "SearchEngineService" -PhysicalPath C:\searchengineservice -BindingInformation "*:5001:"

EXPOSE 5001
ADD obj/Release/netcoreapp2.0 /searchengineservice