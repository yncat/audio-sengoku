# -*- coding: utf-8 -*-
# Player controller
# Copyright (C) 2019 Yukio Nozawa <personal@nyanchangames.com>
import constants
import globalVars
import logging
import glob
from logging import getLogger, FileHandler, Formatter
import os
import sound_lib.sample
import bgtsound
import buildSettings
import keyCodes
import window

class Player(object):
	def __init__(self,keymap,name,y,direction):
		self.keymap=keymap
		self.name=name
		self.y=y
		self.direction=direction
		self.cursor=0

	def getLane(self):
		app=globalVars.app
		while(True):
			app.frameUpdate()
			if app.keyPressed(keyCodes.K_ESCAPE): return -1
			if self.cursor!=0 and app.keyPressed(self.keymap['left']): self.moveCursor(-1)
			if self.cursor!=constants.FIELD_SIZE_X-1 and app.keyPressed(self.keymap['right']): self.moveCursor(1)
			if app.keyPressed(self.keymap['up']): return self.cursor
		#end while
	#end getLane

	def moveCursor(self,val):
		self.cursor+=val
		globalVars.app.playOneShot(globalVars.app.sounds["general/select.ogg"], pitch=70+(self.cursor*15))

