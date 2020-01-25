# -*- coding: utf-8 -*-
# audio skin manager
# Copyright (C) 2020 Yukio Nozawa <personal@nyanchangames.com>
import constants
import globalVars

class SkinManager(object):
	"""�I�[�f�B�I�E�X�L�����Ǘ����܂��B"""
	def __init__(self,skin_name):
		globalVars.app.loadSoundFolder(skin_name)
		self.current_skin=skin_name

	def playOneShot(self,file_name):
		globalVars.app.playOneShot(globalVars.app.sounds["%s/%s" % (self.current_skin,file_name))

