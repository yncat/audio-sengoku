# -*- coding: utf-8 -*-
# Project name: snyper
# Bootstrap
# Copyright (C) 2019 Yukio Nozawa <personal@nyanchangames.com>

import appMain
import game
import globalVars

def main():
	app=appMain.Application()
	app.initialize()
	root=game.GameRoot()
	globalVars.app=app
	globalVars.game=root
	app.run()
#global schope
if __name__ == "__main__": main()