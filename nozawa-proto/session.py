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


