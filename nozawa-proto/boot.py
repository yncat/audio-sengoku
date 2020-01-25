# -*- coding: utf-8 -*-
# Project name: snyper
# Bootstrap
# Copyright (C) 2019 Yukio Nozawa <personal@nyanchangames.com>

import sys

import appMain
import globalVars

def main():
	skin="sengoku" if len(sys.argv)==1 else sys.argv[1]
	app=appMain.Application()
	app.initialize(skin)
	globalVars.app=app
	app.run()
#global schope
if __name__ == "__main__": main()