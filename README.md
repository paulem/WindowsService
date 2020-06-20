# Windows service

Based on: https://stackoverflow.com/a/7764451/980530

Usage:

WindowsService /i
WindowsService /u

WindowsService /i /servicename="WindowsService1" /displayname="Windows Service 1"
WindowsService /u /servicename="WindowsService2"

installutil /servicename="WindowsService1" WindowsService.exe
installutil /u /servicename="WindowsService1" WindowsService.exe
