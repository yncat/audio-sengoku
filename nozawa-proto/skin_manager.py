# -*- coding: utf-8 -*-
# audio skin manager
# Copyright (C) 2020 Yukio Nozawa <personal@nyanchangames.com>
import constants
import globalVars

class SkinManager(object):
	"""オーディオ・スキンを管理します。"""
	def __init__(self,app,skin_name):
		#まだ globalVars が app を参照していないので
		app._loadSoundFolder(skin_name)
		self.current_skin=skin_name

	def playOneShot(self,file_name,pan=0,vol=0,pitch=100):
		globalVars.app._playOneShot(globalVars.app.sounds["%s/%s" % (self.current_skin,file_name)],pan,vol,pitch)

