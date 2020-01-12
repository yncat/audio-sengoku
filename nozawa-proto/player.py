# -*- coding: utf-8 -*-
# Player controller
# Copyright (C) 2019 Yukio Nozawa <personal@nyanchangames.com>
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
	def __init__(self,keymap):
		self.keymap=keymap

