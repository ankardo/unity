﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlan
{
    public Skill skill;
    public SkillAffectsTypeEnum targetType;
    public Vector3Int movePos;
    public Vector3Int skillTargetPos;
    public char direction = 'S';
}
