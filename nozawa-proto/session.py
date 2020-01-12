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
import units
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
		win=None
		while(True):
			p=self.players[self.current_player]
			win=self.moveUnits(p)
			if win: break
			globalVars.app.playSound("general/Jingle_Start.ogg")
			globalVars.app.say("%sの先述" % p.name)
			lane=p.getLane()
			if lane==-1: break
			self.sendMessenger(p,lane)
			lane=p.getLane()
			if lane==-1: break
			self.sendUnit(p,lane)

			#ターン進める
			self.current_player+=1
			if self.current_player==2: self.advanceTurn()
		#end while
		if win:
			app.playSound("general/Jingle_End_Win.ogg")
			app.wait(2500)
			app.say("%sの勝利!" % win.name)
		#end 勝利
	#end start

	def moveUnits(self,p):
		moved_units=[]#同じユニットを2回動かさないように
		win=None
		for i in range(constants.FIELD_SIZE_X):
			for j in range(constants.FIELD_SIZE_Y):
				u=self.field[i][j]
				if u is None: continue
				if u.owner is not p: continue#そのターンのプレイヤーの兵だけ動かす
				if u in moved_units: continue#すでに動かした
				moved_units.append(u)
				newpos=j+1 if u.owner.direction==constants.DIRECTION_UP else j-1
				u2=self.field[i][newpos]
				if u2 is None:
					self.field[i][newpos]=u
					self.field[i][j]=None
					if newpos==0 or newpos==constants.FIELD_SIZE_Y:
						if newpos!=u.owner.y: win=u.owner
					#end プレイヤーの勝利判定
				else:
					self.field[i][j]=None
					self.field[i][newpos]=None
					globalVars.app.playSound("general/ashigaru_combat.ogg")
					globalVars.app.wait(3000)
				#end ユニットの勝敗
			#end フィールドスキャン
		#end フィールドスキャン
		return win

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
			found.append("%d歩先に%s" % (abs(pos-p.y),unit_str))
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

	def sendUnit(self,p,lane):
		u=units.Ashigaru(p)
		self.field[lane][p.y]=u
		u.playSpawnSound()

	def advanceTurn(self):
		self.current_player=0
		self.turn+=1
