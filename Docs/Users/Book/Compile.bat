@echo off

pdflatex SciSharp
makeindex SciSharp
pdflatex SciSharp

@echo on