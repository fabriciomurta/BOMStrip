# BOMStrip
Tool to strip rogue UTF8 BOM from merged text files.

# Introduction

This is a very simple tool to remove misplaced BOM bytes from text files.

This usually happens when merging UTF8 w/BOM text files to a single _jumbo_ file e.g.
when merging different JavaScript files into one.

On linux, this task can be easily performed by the `sed` tool. On windows vista or newer,
PowerShell does the job.

The aim of this project is when you don't want to rely on powershell on windows (scripts
can be denied to run in default settings), for example making the merge using batch files.

The batch files are trickier to work with, but works out of the box on every windows setup
so in some situations the 7kb overhead of this tool might be worth.

# More info

This README was made in a haste so you might miss a lot of information from it. If you feel 
I should add some information here, please let me know via message or an issue/pull request.

# License

So far, seems it will be available subject to GPLv2. Although I might decide to migrate this to
open domain later. (too simple to claim anything from it)
