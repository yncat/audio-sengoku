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
			p=self.players[self.current_player]
			globalVars.app.say("%sの先述" % p.name)
			lane=p.getLane()
			if lane==-1: break
			self.sendMessenger(p,lane)
			#ターン進める
			self.current_player+=1
			if self.current_player==2: self.advanceTurn()
		#end while
	#end start

	def sendMessenger(self,p,lane):
		globalVars.app.playSound("general/messenger.ogg")
		pos=p.y-1
		dir=p.direction
		found=[]
		for i in range(constants.FIELD_SIZE_Y):
			if dir==constants.DIRECTION_UP:
				pos+=1
			else:
				pos-=1
			#end どっちに進むか
			unit=self.field[lane][pos]
			if unit is None: continue
			unit_str="自軍の兵" if unit.owner is p else "敵軍の兵"
			found.append("%d歩先に%s" % unit_str)
		#end for
		globalVars.app.wait(1200)
		if len(found)==0:
			globalVars.app.say("この場所にはだれも折りませんでした!")
			return
		#end だれもおりません
		s="、".join(found).rstrip("、")
		found_str="%sがおりました!" % s
		globalVars.app.say(found_str)
		return

	def advanceTurn(self):
		self.current_player=0
		self.turn+=1
