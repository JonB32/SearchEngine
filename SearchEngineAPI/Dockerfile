FROM microsoft/iis
RUN mkdir C:\searchengineapi
RUN powershell -NoProfile -Command \
    Import-module IISAdministration; \
    New-IISSite -Name "SearchEngineAPI" -PhysicalPath C:\searchengineapi -BindingInformation "*:5000:"

EXPOSE 5000
ADD obj/Release/netcoreapp2.0/PubTmp/Out /searchengineapi