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
		winner=None
		while(True):
			p=self.players[self.current_player]
			winner=self.moveUnits(p)
			if winner: break
			globalVars.app.playSound("turn.ogg")
			globalVars.app.say("%sの戦術" % p.name)
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
		if winner:
			winner.processWin()
			for elem in self.players:
				if elem is not winner: elem.processLose()
			#end lose
			globalVars.app.wait(2500)
			globalVars.app.say("%sの勝利!" % winner.name)
			globalVars.app.wait(3000)
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
					if newpos==0 or newpos==constants.FIELD_SIZE_Y-1:
						if newpos!=u.owner.y: win=u.owner
					#end プレイヤーの勝利判定
				else:
					self.field[i][j]=None
					self.field[i][newpos]=None
					globalVars.app.playSound("unit_combat.ogg")
					globalVars.app.wait(3000)
				#end ユニットの勝敗
			#end フィールドスキャン
		#end フィールドスキャン
		return win

	def sendMessenger(self,p,lane):
		v=[]
		globalVars.app.playSound("messenger.ogg")
		pos=p.y
		dir=p.direction
		found=[]
		for i in range(constants.FIELD_SIZE_Y-1):
			if dir==constants.DIRECTION_UP:
				pos+=1
			else:
				pos-=1
			#end どっちに進むか
			unit=self.field[lane][pos]
			if unit is None: continue
			unit_str="friendly" if unit.owner is p else "opponent"
			v.append("v_forward%d.ogg" % abs(pos-p.y))
			v.append("v_%s.ogg" % unit_str)
		#end for
		globalVars.app.wait(1200)
		if len(v)==0:
			globalVars.app.playSound("v_nothing.ogg")
			return
		#end だれもおりません
		v[len(v)-1]=v[len(v)-1].replace(".ogg","_last.ogg")
		globalVars.app.playSoundList(v)
		return

	def sendUnit(self,p,lane):
		u=units.Generic(p)
		self.field[lane][p.y]=u
		u.playSpawnSound()

	def advanceTurn(self):
		self.current_player=0
		self.turn+=1
