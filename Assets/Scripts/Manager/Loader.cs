﻿using System.Collections;
using System.Collections.Generic;



{
  public GameObject gameManager;


  void Awake()
  {
    if (GameManager.instance == null)
      Instantiate(gameManager);
    if (SoundManager.instance == null)
      Instantiate(soundManager);
  }
