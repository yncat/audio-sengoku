# -*- coding: utf-8 -*-
# Session manager
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

class Session(object):
	def __init__(self,player1,player2):
		self.players=[player1,player2]
		self.field=[]
		for i in range(constants.FIELD_SIZE_X):
			self.field.append([None]*constants.FIELD_SIZE_Y)
			self.turn=1
			self.current_player=0

	def start(self):
		globalVars.app.playSound("general/Jingle_Start.ogg")
		while(True):
			globalVars.app.say("player 1 の先述")
			p=self.players[self.current_player]
			lane=p.getLane()
			if lane==-1: break

			#ターン進める
			self.current_player+=1
			if self.current_player==2: self.advanceTurn()
		#end while
	#end start

	def advanceTurn(self):
		self.current_player=0
		self.turn+=1
