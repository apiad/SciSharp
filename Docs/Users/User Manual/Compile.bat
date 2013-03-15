@echo off

pdflatex UserManual
makeindex UserManual
pdflatex UserManual

@echo on