# -*- coding: utf-8 -*-
# Application entry point
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
import player
import session
import window

class Application(window.SingletonWindow):
	"""
	The game's main application class.

	Instantiate this class, call initialize method, then call run method to start the application. Other methods are internally used and should not be called from outside of the class.
	"""
	def __init__(self):
		super().__init__()
	def initialize(self):
		super().initialize(1200, 800, buildSettings.GAME_NAME+" ("+str(buildSettings.GAME_VERSION)+")")
		self.initLogger()
		self.sounds={}
		self._loadSoundFolder("general")
	def initLogger(self):
		self.hLogHandler=FileHandler("debug.log", mode="w", encoding="UTF-8")
		self.hLogHandler.setLevel(logging.DEBUG)
		self.hLogFormatter=Formatter("%(name)s - %(levelname)s - %(message)s")
		self.hLogHandler.setFormatter(self.hLogFormatter)
		self.log=getLogger("snyper")
		self.log.setLevel(logging.DEBUG)
		self.log.addHandler(self.hLogHandler)
		self.log.info("Starting.")

	def run(self):
		p1=player.Player({'left': keyCodes.K_LEFT, 'right': keyCodes.K_RIGHT, 'up': keyCodes.K_UP},"信長",0,constants.DIRECTION_UP)
		p2=player.Player({'left': keyCodes.K_a, 'right': keyCodes.K_d, 'up': keyCodes.K_w},"光秀",constants.FIELD_SIZE_Y-1,constants.DIRECTION_DOWN)
		localSession=session.Session(p1,p2)
		localSession.start()
		#while(True):
			#self.frameUpdate()
			#if self.keyPressed(keyCodes.K_ESCAPE): break

	def _loadSoundFolder(self,path):
		files=glob.glob("fx/"+path+"/*.ogg")
		self.log.info("loading sound folder: fx/%s (%d files)" % (path,len(files)))
		for elem in files:
			self.sounds[path+"/"+os.path.basename(elem)]=sound_lib.sample.Sample(elem)
	# end loadSounds

	def playSound(self,key):
		bgtsound.playOneShot(self.sounds[key])

	def playOneShot(self,key,pan=0,vol=0,pitch=100,wait=False):
		s=bgtsound.playOneShot(key,pan,vol,pitch)
		if wait:
			while(s.playing is True):
				self.frameUpdate()
			#end while playing
		#end wait is True
	#end playOneShot

	def message(self,msg):
		"""
		Shows a simple message dialog. This method is blocking; it won't return until user dismisses the dialog. While this method is blocking, onExit still works as expected.

		:param msg: Message to show.
		:type msg: str
		"""
		self.say(msg)
		while(True):
			self.frameUpdate()
			if True in (self.keyPressed(keyCodes.K_LEFT), self.keyPressed(keyCodes.K_RIGHT), self.keyPressed(keyCodes.K_UP), self.keyPressed(keyCodes.K_DOWN)): self.say(msg)#Message repeat
			if self.keyPressed(keyCodes.K_RETURN): break
		#end frame update
		bgtsound.playOneShot(self.sounds["UI/decide.ogg"])
	#end message
