# -*- coding: utf-8 -*-
# unit objects
# Copyright (C) 2020 Yukio Nozawa <personal@nyanchangames.com>
import constants
import globalVars
import logging
from logging import getLogger, FileHandler, Formatter
import os
import sound_lib.sample
import bgtsound
import buildSettings
import keyCodes
import window

class UnitBase(object):
	def __init__(self,owner):
		self.owner=owner

	def playSpawnSound(self):
		pass

class Ashigaru(UnitBase):
	def playSpawnSound(self):
		globalVars.app.playSound("general/ashigaru_select.ogg")
		globalVars.app.wait(600)
		globalVars.app.playSound("general/ashigaru_spawn.ogg")
		globalVars.app.wait(4000)

